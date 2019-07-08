using System;
using System.Collections.Generic;
using System.Text;

namespace Armoniasoft.Invoices.Service.GetReportInvoice
{
    public interface IGetReportInvoice<T>
    {
        byte[] getReport(T invoice);
    }
}
