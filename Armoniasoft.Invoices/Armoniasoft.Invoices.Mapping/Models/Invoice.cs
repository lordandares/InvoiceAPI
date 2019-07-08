using Armoniasoft.Invoices.Mapping.Models.Details;
using Armoniasoft.Invoices.Mapping.Models.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Armoniasoft.Invoices.Mapping.Models
{
    public class Invoice
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "tenantId")]
        public string TenantId { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "header")]
        public HeaderFacturaComputarizadaEstandar Header { get; set; }

        [JsonProperty(PropertyName = "detail")]
        public List<DetailFacturaComputarizadaEstandar> Detail { get; set; }
    }
}
