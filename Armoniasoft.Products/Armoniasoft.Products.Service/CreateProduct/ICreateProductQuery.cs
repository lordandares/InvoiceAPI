using System.Threading.Tasks;

namespace Armoniasoft.Products.ProductService.CreateProduct
{
    public interface ICreateProductQuery<T>
    {
        Task<T> Create(string tenantId, T createProductRequest);
    }
}
