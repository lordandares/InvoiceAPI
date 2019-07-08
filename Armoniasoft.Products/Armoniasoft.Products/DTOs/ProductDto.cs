using Newtonsoft.Json;
using System;

namespace Armoniasoft.Products.DTOs
{
    class ProductDto
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "tenantId")]
        public string TenantId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Nombre { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Descripotion { get; set; }

        [JsonProperty(PropertyName = "totalAvailable")]
        public string TotalAvailable { get; set; }

        [JsonProperty(PropertyName = "image")]
        public string Image { get; set; }

        [JsonProperty(PropertyName = "price")]
        public double Price { get; set; }

        [JsonProperty(PropertyName = "cod")]
        public string Cod { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "created")]
        public DateTimeOffset Created { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty(PropertyName = "modified")]
        public DateTimeOffset Modified { get; set; }

        [JsonProperty(PropertyName = "modifiedBy")]
        public string ModifiedBy { get; set; }


    }
}
