using Armoniasoft.ClientDB;
using Armoniasoft.Invoices.Mapping.Models;
using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Armoniasoft.Invoices.Repository.CreateInvoices
{
    public class CreateInvoiceRepository<T> : ICreateInvoiceRepository<T> where T : Invoice
    {
        private readonly IClientDBDocumentClient client;

        public CreateInvoiceRepository(IDocumentClientFactory documentClientFactory)
        {
            client = documentClientFactory.Get();
        }

        public async Task<T> Create(T model)
        {
            try
            {
                return await client.CreateDocument(model.TenantId, model);
            }
            catch (DocumentClientException documentClientException)
            {
                Console.WriteLine($"documentClientException: {documentClientException.Error}");

                throw documentClientException;
            }
        }
    }
}
