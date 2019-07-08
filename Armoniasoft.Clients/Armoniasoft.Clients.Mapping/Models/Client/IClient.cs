using Newtonsoft.Json;

namespace DefaultNamespace
{
    public interface IClient
    {
        [JsonProperty(PropertyName = "id")]
        string Id { get; set; }

        [JsonProperty(PropertyName = "tenantId")]
        string TenantId { get; set; }

        [JsonProperty(PropertyName = "name")]
        string Name { get; set; }
        
        [JsonProperty(PropertyName = "nit")]
        string Nit { get; set; }
    }
}