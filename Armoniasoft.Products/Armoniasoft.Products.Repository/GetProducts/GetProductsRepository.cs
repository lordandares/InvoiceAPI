using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Armoniasoft.ClientDB;
using Microsoft.Azure.Documents;
using System.Linq;
using Armoniasoft.Products.Mapping.Models.Product;
using Armoniasoft.Products.Repository.Query;

namespace Armoniasoft.Products.Repository
{
    public class GetProductsRepository<T> : IGetProductRepository<T> where T : IProduct
    {
        private readonly IClientDBDocumentClient client;

        public GetProductsRepository(IDocumentClientFactory documentClientFactory)
        {
            client = documentClientFactory.Get();
        }

        public Task<T> Get(string tenantId, string id)
        {
            try
            {
                return client.GetDocument<T>(tenantId, id);
            }
            catch (DocumentClientException documentClientException)
            {
                Console.WriteLine($"documentClientException: {documentClientException.Error}");
                
                if (documentClientException.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw documentClientException;
                }
                else
                    throw;
            }
        }

        public async Task<IEnumerable<T>> Get(string tenantId)
        {
            IFilterAppender<T> filter = new GetProductsFilter<T>();
            
            IQueryable<T> appender(IQueryable<T> qry) => qry = filter.AppendTo(qry);
            try
            {
                return await client.GetDocuments<T>(tenantId, appender);
            }
            catch (DocumentClientException documentClientException)
            {
                Console.WriteLine($"documentClientException: {documentClientException.Error}");
                
                if (documentClientException.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw documentClientException;
                }
                else
                    throw;
            }
        }
    }
}
