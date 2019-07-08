using System.Threading.Tasks;

namespace Armoniasoft.Products.ProductService.CreateClient
{
    public interface ICreateClientQuery<T>
    {
        Task<T> Create(string tenantId, T createProductRequest);
    }
}