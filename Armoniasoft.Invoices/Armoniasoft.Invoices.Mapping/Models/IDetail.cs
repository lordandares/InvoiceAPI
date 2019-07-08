using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Armoniasoft.Invoices.Mapping.Models
{
    public interface IDetail
    {
        [JsonProperty(PropertyName = "actividadEconomica")]
        long ActividadEconomica { get; set; }

        [JsonProperty(PropertyName = "codigoProductoSin")]
        long CodigoProductoSin { get; set; }

        [JsonProperty(PropertyName = "codigoProducto")]
        string CodigoProducto { get; set; }

        [JsonProperty(PropertyName = "descripcion")]
        string Descripcion { get; set; }

        [JsonProperty(PropertyName = "cantidad")]
        long Cantidad { get; set; }

        [JsonProperty(PropertyName = "unidadMedida")]
        string UnidadMedida { get; set; }

        [JsonProperty(PropertyName = "precioUnitario")]
        string PrecioUnitario { get; set; }

        [JsonProperty(PropertyName = "montoDescuento")]
        bool MontoDescuento { get; set; }

        [JsonProperty(PropertyName = "subTotal")]
        long SubTotal { get; set; }

        [JsonProperty(PropertyName = "numeroSerie")]
        bool NumeroSerie { get; set; }

        [JsonProperty(PropertyName = "numeroImei")]
        bool NumeroImei { get; set; }
    }
}
