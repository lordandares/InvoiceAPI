using Microsoft.ApplicationInsights;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using Armoniasoft.ClientDB.Telemetry;

namespace Armoniasoft.ClientDB
{
    public class DocumentClientFactory : IDocumentClientFactory
    {
        private readonly Uri cosmosEndpoint;
        private readonly string collectionId;
        private readonly string databaseId;
        private readonly IDocumentClient documentClient;
        private readonly IClientDBDocumentClient clientDBDocumentClient;
        private readonly TelemetryClient telemetryClient = null;

        public DocumentClientFactory(
            Uri cosmosEndpoint, 
            string databaseId,
            string collectionId,
            string authKey, 
            TelemetryClient telemetryClient = null
        )
        {
            this.cosmosEndpoint = cosmosEndpoint;
            this.collectionId = collectionId;
            this.databaseId = databaseId;
            this.telemetryClient = telemetryClient;
            documentClient = CreateDocumentClient(cosmosEndpoint, authKey);
            clientDBDocumentClient = CreateNextDocumentClient(databaseId, collectionId, telemetryClient);
        }

        public IDocumentClient GetDocumentClient() { return documentClient; }

        public IClientDBDocumentClient Get()
        {
            return clientDBDocumentClient;
        }

        public IClientDBDocumentClient CreateNextDocumentClient(string databaseId, string collectionId, TelemetryClient telemetry)
        {
            IAuditProcessor auditProcessor = new AuditProcessor(databaseId, collectionId, documentClient);
            IDbTelemetry dbTelemetry = new CosmosDbTelemetry(telemetry);
            IClientDBDocumentClient nextDocumentClient = new ClientDBDocumentClient(databaseId, collectionId, documentClient, auditProcessor, dbTelemetry);
            return nextDocumentClient;
        }

        private IDocumentClient CreateDocumentClient(Uri endPoint, string authKey)
        {
            return new DocumentClient(endPoint, authKey,
                new ConnectionPolicy
                {
                    RetryOptions = new RetryOptions
                    {
                        MaxRetryAttemptsOnThrottledRequests = 5
                    },
                    ConnectionMode = ConnectionMode.Direct,    // Use Direct if possible for better performance
                    ConnectionProtocol = Protocol.Tcp          // Use TCP if possible for better performance
                }
            );
        }
    }
}
