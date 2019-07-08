using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Armoniasoft.ClientDB.Telemetry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Armoniasoft.ClientDB.Models;

namespace Armoniasoft.ClientDB
{
    public class ClientDBDocumentClient : IClientDBDocumentClient, IDisposable
    {
        private bool disposed = false;
        private readonly string databaseId;
        private readonly string collectionId;
        private IDocumentClient documentClient;
        private IAuditProcessor auditProcessor;
        private readonly IDbTelemetry dbTelemetry;
        private readonly ITimer timer;

        public ClientDBDocumentClient(
            string databaseId,
            string collectionId,
            IDocumentClient documentClient,
            IAuditProcessor auditProcessor,
            IDbTelemetry dbTelemetry,
            ITimer timer = null
        )
        {
            this.databaseId = databaseId;
            this.collectionId = collectionId;
            this.documentClient = documentClient;
            this.auditProcessor = auditProcessor;
            this.dbTelemetry = dbTelemetry;
            this.timer = timer ?? new Timer();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                (documentClient as DocumentClient).Dispose();
            }

            disposed = true;
        }

        public async Task<T> UpdateDocument<T>(
            string tenantId,
            string documentId,
            T document,
            string ETag,
            AuditUser auditUser = null,
            [CallerMemberName] string callerName = ""
        )
        {
            if (ETag == null) throw new ArgumentNullException(nameof(ETag));

            if (string.IsNullOrEmpty(tenantId))
                throw new ArgumentException("TenantID cannot be null or empty", nameof(tenantId));

            if (string.IsNullOrEmpty(documentId))
                throw new ArgumentException("DocumentID cannot be null or empty", nameof(documentId));

            RequestOptions requestOptions = new RequestOptions() { PartitionKey = new PartitionKey(tenantId) };
            Uri uri = CreateUri(documentId);

            if (!string.IsNullOrEmpty(ETag))
                requestOptions.AccessCondition = new AccessCondition
                {
                    Condition = ETag,
                    Type = AccessConditionType.IfMatch
                };

            T readResource = default(T);
            if (auditUser != null)
            {
                timer.Start();
                ResourceResponse<Document> readResponse = await documentClient.ReadDocumentAsync(uri, requestOptions);
                readResource = (dynamic)readResponse.Resource;
                timer.Stop();
                dbTelemetry.TrackDbRequest(callerName, readResponse.RequestCharge, timer.Elapsed, "ReadDocument " + uri.ToString(), tenantId, false, true);
            }

            timer.Start();
            ResourceResponse<Document> response = await documentClient.ReplaceDocumentAsync(uri, document, requestOptions);
            timer.Stop();
            dbTelemetry.TrackDbRequest(callerName, response.RequestCharge, timer.Elapsed, uri.ToString(), tenantId);
            T updatedResource = (dynamic)response.Resource;

            if (auditUser != null)
            {
                timer.Start();
                ResourceResponse<Document> auditResponse = await auditProcessor.OnDocumentUpdated(auditUser, readResource, updatedResource);
                timer.Stop();
                dbTelemetry.TrackDbRequest(callerName, auditResponse.RequestCharge, timer.Elapsed, "OnDocumentUpdated " + uri.ToString(), tenantId, false, true);
            }

            return updatedResource;
        }

        public async Task<T> CreateDocument<T>(string tenantId, T document, AuditUser auditUser = null, [CallerMemberName] string callerName = "")
        {
            if (string.IsNullOrEmpty(tenantId))
                throw new ArgumentException("TenantID cannot be null or empty", nameof(tenantId));

            RequestOptions requestOptions = new RequestOptions() { PartitionKey = new PartitionKey(tenantId) };
            Uri uri = CreateUri();

            timer.Start();
            ResourceResponse<Document> response = await documentClient.CreateDocumentAsync(uri, document, requestOptions);
            timer.Stop();

            dbTelemetry.TrackDbRequest(callerName, response.RequestCharge, timer.Elapsed, uri.ToString(), tenantId);
            T resource = (dynamic)response.Resource;

            if (auditUser != null)
            {
                timer.Start();
                ResourceResponse<Document> auditResponse = await auditProcessor.OnDocumentCreated(auditUser, resource);
                timer.Stop();
                dbTelemetry.TrackDbRequest(callerName, auditResponse.RequestCharge, timer.Elapsed, "OnDocumentCreated " + uri.ToString(), tenantId, false, true);
            }

            return resource;
        }

        public async Task<List<T>> GetDocuments<T>(string tenantId, Func<IQueryable<T>, IQueryable<T>> appender, [CallerMemberName] string callerName = "")
        {
            if (string.IsNullOrEmpty(tenantId))
                throw new ArgumentException("TenantID cannot be null or empty", nameof(tenantId));
            FeedOptions options = new FeedOptions { MaxItemCount = -1, PartitionKey = new PartitionKey(tenantId) };
            return await GetDocumentsQuery(appender, options, tenantId, callerName);
        }

        // NOTE: Can't write unit tests for multitype queries, need to make a mock IQueryable wrapper with custom AsDocumentQuery() conversion
        public async Task<List<R>> GetDocuments<T, R>(string tenantId, Func<IQueryable<T>, IQueryable<R>> appender, [CallerMemberName] string callerName = "")
        {
            if (string.IsNullOrEmpty(tenantId))
                throw new ArgumentException("TenantID cannot be null or empty", nameof(tenantId));
            FeedOptions options = new FeedOptions { MaxItemCount = -1, PartitionKey = new PartitionKey(tenantId) };
            return await GetDocumentsQuery(appender, options, tenantId, callerName);
        }

        public async Task<List<T>> GetDocumentsCrossPartition<T>(Func<IQueryable<T>, IQueryable<T>> appender, [CallerMemberName] string callerName = "")
        {
            FeedOptions options = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };
            return await GetDocumentsQuery(appender, options, "", callerName);
        }

        // NOTE: Can't write unit tests for multitype queries, need to make a mock IQueryable wrapper with custom AsDocumentQuery() conversion
        public async Task<List<R>> GetDocumentsCrossPartition<T, R>(Func<IQueryable<T>, IQueryable<R>> appender, [CallerMemberName] string callerName = "")
        {
            FeedOptions options = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };
            return await GetDocumentsQuery(appender, options, "", callerName);
        }

        public async Task<T> GetDocument<T>(string tenantId, string documentId, [CallerMemberName] string callerName = "")
        {
            if (string.IsNullOrEmpty(tenantId))
                throw new ArgumentException("TenantID cannot be null or empty", nameof(tenantId));

            if (string.IsNullOrEmpty(documentId))
                throw new ArgumentException("DocumentID cannot be null or empty", nameof(documentId));

            RequestOptions requestOptions = new RequestOptions { PartitionKey = new PartitionKey(tenantId) };
            Uri uri = CreateUri(documentId);
            timer.Start();

            ResourceResponse<Document> response = await documentClient.ReadDocumentAsync(uri, requestOptions);

            timer.Stop();
            dbTelemetry.TrackDbRequest(callerName, response.RequestCharge, timer.Elapsed, uri.ToString(), tenantId);

            return (dynamic)response.Resource;
        }

        public async Task<T> GetDocumentOrDefault<T>(string tenantId, string documentId, [CallerMemberName] string caller = "")
        {
            try
            {
                return await GetDocument<T>(tenantId, documentId, caller);
            }
            catch (DocumentClientException ex)
            {
                // Return null if NotFound
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return default(T);
                else
                    throw;
            }
        }

        public async Task<int> CountDocuments<T>(string tenantId, Func<IQueryable<T>, IQueryable<T>> appender, [CallerMemberName] string callerName = "")
        {
            IQueryable<T> queryable = documentClient.CreateDocumentQuery<T>(
                CreateUri(),
                new FeedOptions { MaxItemCount = -1, PartitionKey = new PartitionKey(tenantId) });

            queryable = appender(queryable);

            double RU = 0.0;
            timer.Start();
            int result = await queryable.CountAsync();
            timer.Stop();

            // TODO: how can we calc RUs?
            // Append IQueryable to SQL string?
            // NOTE: Big count queries can generate a lot of RUs
            //dbTelemetry.TrackDbRequest(callerName, response.RequestCharge, timer.Elapsed, "", tenantId);

            return result;
        }

        #region Private
        private Uri CreateUri(string documentId = null)
        {
            if (string.IsNullOrEmpty(documentId))
                return UriFactory.CreateDocumentCollectionUri(databaseId, collectionId);
            else
                return UriFactory.CreateDocumentUri(databaseId, collectionId, documentId);
        }

        private async Task<List<R>> GetDocumentsQuery<T, R>(Func<IQueryable<T>, IQueryable<R>> appender, FeedOptions options, string tenantId, string callerName)
        {
            IQueryable<T> queryable = documentClient.CreateDocumentQuery<T>(CreateUri(), options);
            IDocumentQuery<R> documentQuery = appender(queryable).AsDocumentQuery();

            List<R> list = new List<R>();
            double RU = 0.0;
            timer.Start();

            while (documentQuery.HasMoreResults)
            {
                FeedResponse<R> feed = await documentQuery.ExecuteNextAsync<R>();
                RU = RU + feed.RequestCharge;
                list.AddRange(feed);
            }

            timer.Stop();
            dbTelemetry.TrackDbRequest(callerName, RU, timer.Elapsed, documentQuery.ToString(), tenantId, options.EnableCrossPartitionQuery);

            return list;
        }

        #endregion
    }
}
