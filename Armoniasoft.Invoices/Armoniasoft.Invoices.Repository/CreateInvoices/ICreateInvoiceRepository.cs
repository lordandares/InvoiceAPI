using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Armoniasoft.Invoices.Repository.CreateInvoices
{
    public interface ICreateInvoiceRepository<T>
    {
        Task<T> Create(T model);
    }
}
