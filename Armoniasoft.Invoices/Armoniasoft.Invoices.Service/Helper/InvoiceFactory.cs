using System;
using System.Collections.Generic;
using System.Text;
using Armoniasoft.Invoices.Mapping.Models;
using Armoniasoft.Invoices.Mapping.Models.Details;
using Armoniasoft.Invoices.Mapping.Models.Headers;

namespace Armoniasoft.Invoices.Service.Helper
{
    public class InvoiceFactory: IInvoiceFactory
    {
        public Invoice IdentifyTypeofInvoice(string type)
        {
            Invoice invoice = new Invoice();
            switch (type)
            {   
                case "Estandar":
                    HeaderFacturaComputarizadaEstandar headerFacturaComputarizadaEstandar = new HeaderFacturaComputarizadaEstandar();
                    DetailFacturaComputarizadaEstandar detailFacturaComputarizadaEstandar = new DetailFacturaComputarizadaEstandar();
                    invoice.Header = headerFacturaComputarizadaEstandar;
                    List<DetailFacturaComputarizadaEstandar> details = new List<DetailFacturaComputarizadaEstandar>();
                    details.Add(detailFacturaComputarizadaEstandar);
                    invoice.Detail = details;
                    break;
                default:
                    break;
            }
            return invoice;
        }

    }
}
