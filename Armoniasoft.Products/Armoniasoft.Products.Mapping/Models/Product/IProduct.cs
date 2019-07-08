using Newtonsoft.Json;

namespace Armoniasoft.Products.Mapping.Models.Product
{
    public interface IProduct
    {
        [JsonProperty(PropertyName = "id")]
        string Id { get; set; }

        [JsonProperty(PropertyName = "tenantId")]
        string TenantId { get; set; }

        [JsonProperty(PropertyName = "name")]
        string Nombre { get; set; }

        [JsonProperty(PropertyName = "description")]
        string Descripotion { get; set; }

        [JsonProperty(PropertyName = "totalAvailable")]
        string TotalAvailable { get; set; }

        [JsonProperty(PropertyName = "image")]
        string Image { get; set; }

        [JsonProperty(PropertyName = "price")]
        double Price { get; set; }

        [JsonProperty(PropertyName = "cod")]
        string Cod { get; set; }

        [JsonProperty(PropertyName = "type")]
        string Type { get; set; }
    }
}