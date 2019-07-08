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
using Armoniasoft.ClientDB;
using Armoniasoft.Products.Helpers;
using Armoniasoft.Products.ProductService.GetProducts;
using Armoniasoft.Products.Repository;
using Armoniasoft.Products.DTOs;
using Armoniasoft.Products.Mapping;
using Armoniasoft.Products.Mapping.Models.Product;

namespace Armoniasoft.Products.Functions.GetProduct
{
    public static class GetProduct
    {
        private static readonly TelemetryClient telemetry = new TelemetryClient(TelemetryConfiguration.Active);

        [FunctionName("GetProduct")]
        [Display(Name = "Get Products", Description = "Get Product")]
        [ProducesResponseType(200, Type = typeof(ProductDto))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "products/{id}")] HttpRequest req,
            string id, ILogger log, ExecutionContext context)
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
                    getProductsQuery,
                    id);
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
        public static async Task<IActionResult> RunProxy(ITenantExtractor tenantExtractor, ILogger logger, IGetProductsQuery<Product> getProductsQuery, string id)
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

            Product Product = await getProductsQuery.GetProduct(tenantId,id);

            ProductDto ProductDto = MappingHelper.Map<Product, ProductDto>(Product); 
                    //On purpose we are faking some things here. This will go away when additional functionality is added.
            ProductDto.Created = DateTimeOffset.UtcNow;
            ProductDto.CreatedBy = "me";
            ProductDto.Modified = DateTimeOffset.UtcNow;
            ProductDto.ModifiedBy = "me";
            
            return new OkObjectResult(ProductDto);

        }
    }


}
