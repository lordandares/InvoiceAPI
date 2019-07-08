using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Armoniasoft.Invoices.Mapping.Models.Headers
{
    public class HeaderFacturaComputarizadaEstandar : IHeader
    {
        [JsonProperty(PropertyName = "nitEmisor")]
        public string NitEmisor { get; set; }

        [JsonProperty(PropertyName = "numeroFactura")]
        public long NumeroFactura { get; set; }

        [JsonProperty(PropertyName = "cuf")]
        public string Cuf { get; set; }

        [JsonProperty(PropertyName = "cufd")]
        public string Cufd { get; set; }

        [JsonProperty(PropertyName = "codigoSucursal")]
        public long CodigoSucursal { get; set; }

        [JsonProperty(PropertyName = "direccion")]
        public string Direccion { get; set; }

        [JsonProperty(PropertyName = "codigoPuntoVenta")]
        public long CodigoPuntoVenta { get; set; }

        [JsonProperty(PropertyName = "fechaEmision")]
        public DateTimeOffset FechaEmision { get; set; }

        [JsonProperty(PropertyName = "nombreRazonSocial")]
        public string NombreRazonSocial { get; set; }

        [JsonProperty(PropertyName = "codigoTipoDocumentoIdentidad")]
        public int CodigoTipoDocumentoIdentidad { get; set; }

        [JsonProperty(PropertyName = "numeroDocumento")]
        public string NumeroDocumento { get; set; }

        [JsonProperty(PropertyName = "complemento")]
        public bool Complemento { get; set; }

        [JsonProperty(PropertyName = "codigoCliente")]
        public string CodigoCliente { get; set; }

        [JsonProperty(PropertyName = "codigoMetodoPago")]
        public int CodigoMetodoPago { get; set; }

        [JsonProperty(PropertyName = "numeroTarjeta")]
        public string NumeroTarjeta { get; set; }

        [JsonProperty(PropertyName = "montoTotal")]
        public long MontoTotal { get; set; }

        [JsonProperty(PropertyName = "montoDescuento")]
        public bool MontoDescuento { get; set; }

        [JsonProperty(PropertyName = "codigoMoneda")]
        public long CodigoMoneda { get; set; }

        [JsonProperty(PropertyName = "tipoCambio")]
        public long TipoCambio { get; set; }

        [JsonProperty(PropertyName = "montoTotalMoneda")]
        public long MontoTotalMoneda { get; set; }

        [JsonProperty(PropertyName = "leyenda")]
        public string Leyenda { get; set; }

        [JsonProperty(PropertyName = "usuario")]
        public string Usuario { get; set; }

        [JsonProperty(PropertyName = "codigoDocumentoSector")]
        public long CodigoDocumentoSector { get; set; }
    }
}
