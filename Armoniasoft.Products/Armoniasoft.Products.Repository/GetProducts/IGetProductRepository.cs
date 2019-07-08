using System.Collections.Generic;
using System.Threading.Tasks;

namespace Armoniasoft.Products.Repository
{
    public interface IGetProductRepository<T>
    {
        Task<IEnumerable<T>> Get(string tenantId);
        
        Task<T> Get(string tenantId, string id);
    }
}