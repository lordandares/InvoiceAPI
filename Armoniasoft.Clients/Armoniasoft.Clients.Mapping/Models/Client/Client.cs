using Newtonsoft.Json;

namespace DefaultNamespace
{
    public class Client : IClient
    {
        public Client(string tenantId, string name, string nit)
        {
            TenantId = tenantId;
            Name = name;
            Nit = nit;
        }

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