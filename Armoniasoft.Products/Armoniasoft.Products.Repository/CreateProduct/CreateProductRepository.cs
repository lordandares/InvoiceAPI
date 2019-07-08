using System;
using Armoniasoft.ClientDB;
using System.Threading.Tasks;
using Armoniasoft.Products.Mapping.Models.Product;
using Microsoft.Azure.Documents;

namespace Armoniasoft.Products.Repository.CreateProductRepository
{
    public class CreateProductRepository<T> : ICreateProductRepository<T> where T : IProduct
    {
        private readonly IClientDBDocumentClient client;

        public CreateProductRepository(IDocumentClientFactory documentClientFactory)
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
