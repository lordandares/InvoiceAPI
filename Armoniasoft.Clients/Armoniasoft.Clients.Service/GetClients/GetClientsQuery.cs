using System.Collections.Generic;
using System.Threading.Tasks;
using Armoniasoft.Clients.Repository;
using DefaultNamespace;
using Microsoft.Azure.Documents;

namespace Armoniasoft.Products.ProductService.GetClients
{
    public class GetClientsQuery<T> : IGetClientsQuery<T> where T: IClient
    {
        private readonly IGetClientsRepository<T> getClientsRepository;
        
        public GetClientsQuery(IGetClientsRepository<T> getClientsRepository)
        {
            this.getClientsRepository = getClientsRepository;
        }
        
        public async Task<IEnumerable<T>> GetClients(string tenantId)
        {
            try
            {
                return await this.getClientsRepository.Get(tenantId);
            }
            catch (DocumentClientException)
            {
                return new List<T>();
            }
        }
    }
}