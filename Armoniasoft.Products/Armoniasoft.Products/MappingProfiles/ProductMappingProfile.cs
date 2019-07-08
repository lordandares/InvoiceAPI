using Armoniasoft.Products.DTOs;
using Armoniasoft.Products.Mapping.Models.Product;
using AutoMapper;

namespace Armoniasoft.Products.MappingProfiles
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductDto>();
                
        }
    }
}
