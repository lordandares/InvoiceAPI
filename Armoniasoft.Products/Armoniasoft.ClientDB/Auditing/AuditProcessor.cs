using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Armoniasoft.ClientDB.Models;
using System;
using System.Threading.Tasks;

namespace Armoniasoft.ClientDB
{
    public class AuditProcessor : IAuditProcessor
    {
        private readonly string databaseId;
        private readonly string collectionId;
        private readonly IDocumentClient documentClient;

        public AuditProcessor(string databaseId, string collectionId, IDocumentClient documentClient)
        {
            this.databaseId = databaseId;
            this.collectionId = collectionId;
            this.documentClient = documentClient;
        }

        public async Task<ResourceResponse<Document>> OnDocumentCreated<T>(AuditUser user, T newDocument)
        {
            T oldDocument = default(T);
            return await SaveAuditRecord(user, oldDocument, newDocument);
        }

        public async Task<ResourceResponse<Document>> OnDocumentUpdated<T>(AuditUser user, T oldDocument, T newDocument)
        {
            return await SaveAuditRecord(user, oldDocument, newDocument);
        }

        private async Task<ResourceResponse<Document>> SaveAuditRecord<T>(AuditUser user, T oldDocument, T newDocument)
        {
            Type typeParameter = typeof(T);
            AuditRecord<T> auditRecord = new AuditRecord<T>()
            {
                Type = "audit",
                TenantId = user.TenantId,
                UpdatedDocumentType = typeParameter.Name,
                OldDocument = oldDocument,
                NewDocument = newDocument,
                Updated = DateTime.UtcNow,
                UpdatedBy = user
            };

            RequestOptions requestOptions = new RequestOptions() { PartitionKey = new PartitionKey(user.TenantId) };
            Uri collectionUri = UriFactory.CreateDocumentCollectionUri(databaseId, collectionId);
            return await documentClient.CreateDocumentAsync(collectionUri, auditRecord, requestOptions);
        }
    }
}
