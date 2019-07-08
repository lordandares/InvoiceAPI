using System.Collections.Generic;
using System.Threading.Tasks;

namespace Armoniasoft.Products.ProductService.GetProducts
{
    public interface IGetProductsQuery<T>
    {
        Task<IEnumerable<T>> GetProducts(string tenantId);
        Task<T> GetProduct(string tenantId, string id);
    }
}
