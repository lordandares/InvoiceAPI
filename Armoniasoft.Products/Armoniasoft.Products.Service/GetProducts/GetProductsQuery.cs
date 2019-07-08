using System.Collections.Generic;
using System.Threading.Tasks;
using Armoniasoft.Products.Mapping.Models.Product;
using Armoniasoft.Products.Repository;
using Microsoft.Azure.Documents;

namespace Armoniasoft.Products.ProductService.GetProducts
{
    public class GetProductsQuery<T> : IGetProductsQuery<T> where T: IProduct
    {
        private readonly IGetProductRepository<T> getProductRepository;

        public GetProductsQuery(IGetProductRepository<T> getProductRepository)
        {
            this.getProductRepository = getProductRepository;
        }

        public Task<T> GetProduct(string tenantId, string id)
        {
            return this.getProductRepository.Get(tenantId, id);
        }

        public async Task<IEnumerable<T>> GetProducts(string tenantId)
        {
            try
            {
                return await this.getProductRepository.Get(tenantId);
            }
            catch (DocumentClientException)
            {
                return new List<T>();
            }
        }
    }
}
