using System.Threading.Tasks;
using Armoniasoft.Products.Mapping;
using Armoniasoft.Products.Mapping.Models.Product;
using Armoniasoft.Products.Repository.CreateProductRepository;

namespace Armoniasoft.Products.ProductService.CreateProduct
{
    public class CreateProductQuery<T> : ICreateProductQuery<T> where T : CreateProductRequest
    {
        private readonly ICreateProductRepository<T> createProductRepository;

        public CreateProductQuery(ICreateProductRepository<T> createProductRepository)
        {
            this.createProductRepository = createProductRepository;
        }

        public async  Task<T> Create(string tenantId, T createProductRequest)
        {
            MappingHelper.Map<T, Product>(createProductRequest);

            return await this.createProductRepository.Create(createProductRequest);
        }

    }
}
