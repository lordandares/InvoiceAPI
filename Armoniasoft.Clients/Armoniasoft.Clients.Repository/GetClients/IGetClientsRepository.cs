using System.Collections.Generic;
using System.Threading.Tasks;

namespace Armoniasoft.Clients.Repository
{
    public interface IGetClientsRepository<T>
    {
        Task<IEnumerable<T>> Get(string tenantId);
    }
}