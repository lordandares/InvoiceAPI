using System.Threading.Tasks;

namespace Armoniasoft.Clients.Repository.CreateClient
{
    public interface ICreateClientRepository<T>
    {
        Task<T> Create(T model);
    }
}