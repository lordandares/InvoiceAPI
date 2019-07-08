using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Microsoft.ApplicationInsights;
using System;
using System.Linq;
using Armoniasoft.ClientDB;
using Armoniasoft.Products.Helpers;
using Armoniasoft.Products.ProductService.GetProducts;
using Armoniasoft.Products.Repository;
using Armoniasoft.Products.DTOs;
using System.Collections.Generic;
using Armoniasoft.Products.Mapping;
using Armoniasoft.Products.Mapping.Models.Product;
using Armoniasoft.Products.MappingProfiles;

namespace Armoniasoft.Products.Functions.GetProducts
{
    public static class GetProducts
    {
        private static readonly TelemetryClient telemetry = new TelemetryClient(TelemetryConfiguration.Active);

        [FunctionName("GetProducts")]
        [Display(Name = "Get Products", Description = "Get List of Products")]
        [ProducesResponseType(200, Type = typeof(ProductDto))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "products")] HttpRequest req,
            ILogger log, ExecutionContext context)
        {
            try
            {
                IConfigurationRoot config = new ConfigurationBuilder()
                    .SetBasePath(context.FunctionAppDirectory)
                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

           
                IDocumentClientFactory documentClientFactory = (new DocumentClientFactoryHelper(config)).GetFactory();
                IGetProductRepository<Product> getProductsRepository = new GetProductsRepository<Product>(documentClientFactory);
                IGetProductsQuery<Product> getProductsQuery = new GetProductsQuery<Product>(getProductsRepository);

                ITenantExtractor tenantExtractor = new HttpRequestTenantExtractor(req);

                return await GetProductProxy.RunProxy(
                    tenantExtractor,
                    log,
                    getProductsQuery);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
                telemetry.TrackException(ex);
                throw;
            }
        }
    }

    public static class GetProductProxy
    {
        public static async Task<IActionResult> RunProxy(ITenantExtractor tenantExtractor, ILogger logger, IGetProductsQuery<Product> getProductsQuery)
        {
            string tenantId;
            try
            {
                tenantId = tenantExtractor.GetTenantId();
            }
            catch (TenantExtractorException)
            {
                logger.LogWarning("TenantId missing in request headers");
                return new BadRequestObjectResult("TenantId missing in request headers");
            }

            IEnumerable<Product> allProducts = await getProductsQuery.GetProducts(tenantId);

            IEnumerable<ProductDto> allProductsDto;
            if (allProducts == null)
            {
                allProductsDto = new List<ProductDto>();
            }
            else
            {
                allProductsDto = MappingHelper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(allProducts, new ProductMappingProfile());
                allProductsDto = allProductsDto.ToList().Select<ProductDto, ProductDto>(i =>
                {
                    //On purpose we are faking some things here. This will go away when additional functionality is added.
                    ProductDto ProductDto = i;
                    ProductDto.Created = DateTimeOffset.UtcNow;
                    ProductDto.CreatedBy = "me";
                    ProductDto.Modified = DateTimeOffset.UtcNow;
                    ProductDto.ModifiedBy = "me";
                    return ProductDto;
                });
            }

            return new OkObjectResult(allProductsDto);

        }
    }


}
