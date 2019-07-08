using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Armoniasoft.Invoices.Mapping.Models.Details
{
    public class DetailFacturaComputarizadaEstandar : IDetail
    {
        [JsonProperty(PropertyName = "actividadEconomica")]
        public long ActividadEconomica { get; set; }

        [JsonProperty(PropertyName = "codigoProductoSin")]
        public long CodigoProductoSin { get; set; }

        [JsonProperty(PropertyName = "codigoProducto")]
        public string CodigoProducto { get; set; }

        [JsonProperty(PropertyName = "descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty(PropertyName = "cantidad")]
        public long Cantidad { get; set; }

        [JsonProperty(PropertyName = "unidadMedida")]
        public string UnidadMedida { get; set; }

        [JsonProperty(PropertyName = "precioUnitario")]
        public string PrecioUnitario { get; set; }

        [JsonProperty(PropertyName = "montoDescuento")]
        public bool MontoDescuento { get; set; }

        [JsonProperty(PropertyName = "subTotal")]
        public long SubTotal { get; set; }

        [JsonProperty(PropertyName = "numeroSerie")]
        public bool NumeroSerie { get; set; }

        [JsonProperty(PropertyName = "numeroImei")]
        public bool NumeroImei { get; set; }
    }
}
