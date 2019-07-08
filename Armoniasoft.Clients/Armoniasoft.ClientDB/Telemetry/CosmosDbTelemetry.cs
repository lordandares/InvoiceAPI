using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;

namespace Armoniasoft.ClientDB.Telemetry
{
    public class CosmosDbTelemetry : IDbTelemetry
    {
        private readonly TelemetryClient telemetryClient;

        public CosmosDbTelemetry(TelemetryClient telemetryClient)
        {
            this.telemetryClient = telemetryClient;
        }

        public void TrackDbRequest(
           string callerName,
           double RU,
           TimeSpan elapsed,
           string requestContent,
           string tenantId = null,
           bool isCrossPartition = false,
           bool isAudit = false
        )
        {
            if (telemetryClient != null)
            {
                string env = ConfigurationManager.AppSettings["Environment"];
                if (env == "Local")
                    Debug.WriteLine($"[TrackRU] {env} {callerName} > {elapsed.Milliseconds} MS -- {RU} RU -- Cross:{isCrossPartition} Audit:{isAudit} SQL: {requestContent}");

                telemetryClient.TrackEvent(
                    "cosmosDbRequest",
                    new Dictionary<string, string>
                    {
                        { "methodName", callerName },
                        { "tenantId", tenantId },
                        { "isCrossPartition", isCrossPartition.ToString()   },
                        { "isAudit", isAudit.ToString() },
                        { "isBulk", false.ToString() },
                        { "requestContent", requestContent },
                        { "environment", env }
                    },
                    new Dictionary<string, double>
                    {
                        { "resourceCharge", RU },
                        { "elapsed", elapsed.Milliseconds }
                    }
                );
            };
        }
    }
}
