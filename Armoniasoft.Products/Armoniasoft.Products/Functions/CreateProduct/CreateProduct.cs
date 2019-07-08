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
using Armoniasoft.Products.ProductService.CreateProduct;
using Armoniasoft.Products.DTOs;
using Armoniasoft.Products.Mapping;
using Armoniasoft.Products.Mapping.Models.Product;
using Armoniasoft.Products.Repository.CreateProductRepository;
using Armoniasoft.Products.ProductService.CreateProduct.Models;

namespace Armoniasoft.Products.Functions.CreateProduct
{
    public static class CreateProduct
    {
        private static readonly TelemetryClient telemetry = new TelemetryClient(TelemetryConfiguration.Active);

        [FunctionName("CreateProduct")]
        [Display(Name = "Create Task", Description = "Create Product")]
        [ProducesResponseType(201, Type = typeof(ProductDto))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous,  "post", Route = "products")] HttpRequest req,
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
                ICreateProductRepository<CreateProductRequest> createProductsRepository = new CreateProductRepository<CreateProductRequest>(documentClientFactory);
                ICreateProductQuery<CreateProductRequest> createProductQuery = new CreateProductQuery<CreateProductRequest>(createProductsRepository);

                ITenantExtractor tenantExtractor = new HttpRequestTenantExtractor(req);
                IRequestBodyExtractor<CreateProductRequestBodyDto> requestBodyExtractor = new HttpRequestBodyExtractor<CreateProductRequestBodyDto>(req);

                return await CreateProductProxy.RunProxy(
                    tenantExtractor,
                    log,
                    createProductQuery,
                    requestBodyExtractor);
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
                telemetry.TrackException(ex);
                throw;
            }
        }
    }

    public static class CreateProductProxy
    {
        public static async Task<IActionResult> RunProxy(ITenantExtractor tenantExtractor, ILogger logger, ICreateProductQuery<CreateProductRequest> createProductQuery, IRequestBodyExtractor<CreateProductRequestBodyDto> requestBodyExtractor)
        {
            string tenantId;
            try
            {
                tenantId = tenantExtractor.GetTenantId();
            }
            catch (TenantExtractorException ex)
            {
                logger.LogWarning(ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }

            CreateProductRequestBodyDto body;
            try
            {
                body = await requestBodyExtractor.GetBody();
            }
            catch (RequestBodyInvalidException ex)
            {
                logger.LogWarning(ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }

            CreateProductRequest createProductRequest = MappingHelper.Map<CreateProductRequestBodyDto, CreateProductRequest>(body);
            createProductRequest = await createProductQuery.Create(tenantId, createProductRequest);


            ProductDto productDto = MappingHelper.Map<CreateProductRequest, ProductDto>(createProductRequest);
            productDto.Created = DateTimeOffset.UtcNow;
            productDto.CreatedBy = "Yours truly";

            //object createdObject = new { value = createdTaskDto };
            ObjectResult createdObjectResult = new ObjectResult(productDto)
            {
                StatusCode = 201,
            };
            return createdObjectResult;

        }
    }

}
