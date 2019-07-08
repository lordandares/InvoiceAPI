using System.Collections.Generic;
using System.Threading.Tasks;

namespace Armoniasoft.Products.ProductService.GetClients
{
    public interface IGetClientsQuery<T>
    {
        Task<IEnumerable<T>> GetClients(string tenantId);
    }
}