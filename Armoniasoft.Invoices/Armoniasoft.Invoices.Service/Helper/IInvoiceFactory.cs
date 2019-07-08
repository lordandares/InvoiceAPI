using System;
using System.Collections.Generic;
using System.Text;
using Armoniasoft.Invoices.Mapping.Models;

namespace Armoniasoft.Invoices.Service.Helper
{
    public interface IInvoiceFactory
    {
        Invoice IdentifyTypeofInvoice(string type);
    }
}
