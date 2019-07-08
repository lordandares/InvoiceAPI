using Newtonsoft.Json;
using Armoniasoft.ClientDB.Models;
using System;

namespace Armoniasoft.ClientDB
{
    internal class AuditRecord<T>
    {
        internal AuditRecord()
        {
        }

        [JsonProperty(PropertyName = "updatedBy")]
        public AuditUser UpdatedBy { get; internal set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; internal set; }
        [JsonProperty(PropertyName = "tenantId")]
        public string TenantId { get; internal set; }
        [JsonProperty(PropertyName = "old")]
        internal T OldDocument { get; set; }
        [JsonProperty(PropertyName = "new")]
        internal T NewDocument { get; set; }
        [JsonProperty(PropertyName = "updated")]
        internal DateTime Updated { get; set; }
        [JsonProperty(PropertyName = "updatedDocumentType")]
        internal string UpdatedDocumentType { get; set; }
    }
}