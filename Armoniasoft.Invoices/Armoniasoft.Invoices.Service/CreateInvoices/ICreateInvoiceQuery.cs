using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Armoniasoft.Invoices.Service.CreateInvoices
{
    public interface ICreateInvoiceQuery<T>
    {
        Task<T> Create(T invoice);
    }
}
