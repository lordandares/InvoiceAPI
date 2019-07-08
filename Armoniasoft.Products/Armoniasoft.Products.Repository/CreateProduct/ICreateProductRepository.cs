using System.Threading.Tasks;

namespace Armoniasoft.Products.Repository.CreateProductRepository
{
    public interface ICreateProductRepository<T>
    {
        Task<T> Create(T model);
    }
}