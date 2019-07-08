using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Armoniasoft.Invoices.Mapping.Models
{
    public interface IHeader
    {
        [JsonProperty(PropertyName = "nitEmisor")]
        string NitEmisor { get; set; }

        [JsonProperty(PropertyName = "numeroFactura")]
        long NumeroFactura { get; set; }

        [JsonProperty(PropertyName = "cuf")]
        string Cuf { get; set; }

        [JsonProperty(PropertyName = "cufd")]
        string Cufd { get; set; }

        [JsonProperty(PropertyName = "codigoSucursal")]
        long CodigoSucursal { get; set; }

        [JsonProperty(PropertyName = "direccion")]
        string Direccion { get; set; }

        [JsonProperty(PropertyName = "codigoPuntoVenta")]
        long CodigoPuntoVenta { get; set; }

        [JsonProperty(PropertyName = "fechaEmision")]
        DateTimeOffset FechaEmision { get; set; }

        [JsonProperty(PropertyName = "nombreRazonSocial")]
        string NombreRazonSocial { get; set; }

        [JsonProperty(PropertyName = "codigoTipoDocumentoIdentidad")]
        int CodigoTipoDocumentoIdentidad { get; set; }

        [JsonProperty(PropertyName = "numeroDocumento")]
        string NumeroDocumento { get; set; }

        [JsonProperty(PropertyName = "complemento")]
        bool Complemento { get; set; }

        [JsonProperty(PropertyName = "codigoCliente")]
        string CodigoCliente { get; set; }

        [JsonProperty(PropertyName = "codigoMetodoPago")]
        int CodigoMetodoPago { get; set; }

        [JsonProperty(PropertyName = "numeroTarjeta")]
        string NumeroTarjeta { get; set; }

        [JsonProperty(PropertyName = "montoTotal")]
        long MontoTotal { get; set; }

        [JsonProperty(PropertyName = "montoDescuento")]
        bool MontoDescuento { get; set; }

        [JsonProperty(PropertyName = "codigoMoneda")]
        long CodigoMoneda { get; set; }

        [JsonProperty(PropertyName = "tipoCambio")]
        long TipoCambio { get; set; }

        [JsonProperty(PropertyName = "montoTotalMoneda")]
        long MontoTotalMoneda { get; set; }

        [JsonProperty(PropertyName = "leyenda")]
        string Leyenda { get; set; }

        [JsonProperty(PropertyName = "usuario")]
        string Usuario { get; set; }

        [JsonProperty(PropertyName = "codigoDocumentoSector")]
        long CodigoDocumentoSector { get; set; }
    }
}
