using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Armoniasoft.ClientDB;
using Armoniasoft.Clients.DTOs;
using Armoniasoft.Clients.Functions.CreateClient.DTOs;
using Armoniasoft.Clients.Helpers;
using Armoniasoft.Clients.Mapping;
using Armoniasoft.Clients.Repository.CreateClient;
using Armoniasoft.Products.ProductService.CreateClient;
using DefaultNamespace;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Armoniasoft.Clients.Functions.CreateClient
{
    public class CreateClient
    {
        private static readonly TelemetryClient telemetry = new TelemetryClient(TelemetryConfiguration.Active);

        [FunctionName("CreateClient")]
        [Display(Name = "Create Task", Description = "Create Client")]
        [ProducesResponseType(201, Type = typeof(ClientDto))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function,  "post", Route = "clients")] HttpRequest req,
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
                ICreateClientRepository<Client> createClientsRepository = 
                    new CreateClientRepository<Client>(documentClientFactory);
                ICreateClientQuery<Client> createClientQuery = 
                    new CreateClientQuery<Client>(createClientsRepository);

                ITenantExtractor tenantExtractor = new HttpRequestTenantExtractor(req);
                IRequestBodyExtractor<CreateClientRequestBodyDto> requestBodyExtractor = 
                    new HttpRequestBodyExtractor<CreateClientRequestBodyDto>(req);

                return await CreateClientProxy.RunProxy(
                    tenantExtractor,
                    log,
                    createClientQuery,
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
    
    public static class CreateClientProxy
    {
        public static async Task<IActionResult> RunProxy(ITenantExtractor tenantExtractor, ILogger logger, 
            ICreateClientQuery<Client> createClientQuery, 
            IRequestBodyExtractor<CreateClientRequestBodyDto> requestBodyExtractor)
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

            CreateClientRequestBodyDto body;
            try
            {
                body = await requestBodyExtractor.GetBody();
            }
            catch (RequestBodyInvalidException ex)
            {
                logger.LogWarning(ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
  
            Client createClientRequest = MappingHelper.Map<CreateClientRequestBodyDto, Client>(body);
            createClientRequest = await createClientQuery.Create(tenantId, createClientRequest);

            ClientDto clientDto = MappingHelper.Map<Client, ClientDto>(createClientRequest);
            ObjectResult createdObjectResult = new ObjectResult(clientDto)
            {
                StatusCode = 201,
            };
            
            return createdObjectResult;
        }
    }
}