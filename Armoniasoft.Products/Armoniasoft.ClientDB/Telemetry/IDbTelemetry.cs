using System;

namespace Armoniasoft.ClientDB.Telemetry
{
    public interface IDbTelemetry
    {
        void TrackDbRequest(
          string callerName,
          double RU,
          TimeSpan elapsed,
          string requestContent,
          string tenantId = null,
          bool isCrossPartition = false,
          bool isAudit = false
       );
    }
}
