using Armoniasoft.Invoices.Mapping.Models;
using Armoniasoft.Invoices.Repository.CreateInvoices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Armoniasoft.Invoices.Mapping;

namespace Armoniasoft.Invoices.Service.CreateInvoices
{
    public class CreateInvoiceQuery<T> : ICreateInvoiceQuery<T> where T : Invoice
    {
        private readonly ICreateInvoiceRepository<T> createInvoiceRepository;

        public CreateInvoiceQuery(ICreateInvoiceRepository<T> createInvoiceRepository)
        {
            this.createInvoiceRepository = createInvoiceRepository;
        }

        public async Task<T> Create(T invoice)
        {

            return await this.createInvoiceRepository.Create(invoice);
        }
    }
}
