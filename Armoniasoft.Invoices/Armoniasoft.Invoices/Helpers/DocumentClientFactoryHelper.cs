using Armoniasoft.ClientDB;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Armoniasoft.Invoices.Helpers
{
    class DocumentClientFactoryHelper
    {
        private static readonly TelemetryClient telemetryClient = new TelemetryClient(TelemetryConfiguration.Active);

        private static IDocumentClientFactory documentClientFactory;
        private static readonly object lockObj = new object();

        public DocumentClientFactoryHelper(IConfigurationRoot config)
        {
            lock (lockObj)
            {
                if (documentClientFactory == null)
                {
                    ConfigureDocumentClientFactory(config);
                }
            }
        }

        private void ConfigureDocumentClientFactory(IConfigurationRoot config)
        {
            bool.TryParse(config["TRACK_COSMOS_TELEMETRY"], out bool cosmosDbTelemetry);

            string cosmosEndpoint = config["COSMOS_ENDPOINT"];
            string cosmosDatabaseId = config["COSMOS_DATABASEID"];
            string cosmosCollectionId = config["COSMOS_COLLECTIONID"];
            string cosmosAuthKey = config["COSMOS_AUTHKEY"];
            string resourceTokenEndpoint = config["COSMOS_RESOURCE_TOKEN_ENDPOINT"];
            string resourceTokenAPIKey = config["COSMOS_RESOURCE_TOKEN_KEY"];

            Uri endpointUri = new Uri(cosmosEndpoint);
            documentClientFactory = new DocumentClientFactory(
                    endpointUri,
                    cosmosDatabaseId,
                    cosmosCollectionId,
                    cosmosAuthKey,
                    cosmosDbTelemetry ? telemetryClient : null
                );
        }

        public IDocumentClientFactory GetFactory()
        {
            return documentClientFactory;
        }
    }
}

