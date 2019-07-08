using System.Threading.Tasks;
using Armoniasoft.Clients.Mapping;
using Armoniasoft.Clients.Repository.CreateClient;
using DefaultNamespace;

namespace Armoniasoft.Products.ProductService.CreateClient
{
    public class CreateClientQuery<T> : ICreateClientQuery<T> where T : Client
    {
        private readonly ICreateClientRepository<T> createProductRepository;

        public CreateClientQuery(ICreateClientRepository<T> createProductRepository)
        {
            this.createProductRepository = createProductRepository;
        }

        public async  Task<T> Create(string tenantId, T createProductRequest)
        {
            MappingHelper.Map<T, Client>(createProductRequest);

            return await this.createProductRepository.Create(createProductRequest);
        }
    }
}