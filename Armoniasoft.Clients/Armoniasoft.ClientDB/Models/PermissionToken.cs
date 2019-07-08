using Newtonsoft.Json;

namespace Armoniasoft.ClientDB.Models
{
    public class PermissionToken
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "tenantId")]
        public string TenantId { get; set; }
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }
        [JsonProperty(PropertyName = "expires")]
        public int Expires { get; set; }
        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }
    }
}
