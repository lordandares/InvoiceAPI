using Newtonsoft.Json;

namespace Armoniasoft.Clients.DTOs
{
    public class ClientDto
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        
        [JsonProperty(PropertyName = "tenantId")]
        public string TenantId { get; set; }
        
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        
        [JsonProperty(PropertyName = "nit")]
        public string Nit { get; set; }
    }
}