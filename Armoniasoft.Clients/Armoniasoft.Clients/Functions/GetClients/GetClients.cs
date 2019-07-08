using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Armoniasoft.ClientDB;
using Armoniasoft.Clients.DTOs;
using Armoniasoft.Clients.Helpers;
using Armoniasoft.Clients.Mapping;
using Armoniasoft.Clients.MappingProfiles;
using Armoniasoft.Clients.Repository;
using Armoniasoft.Products.ProductService.GetClients;
using DefaultNamespace;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Armoniasoft.Clients.Functions
{
    public class GetClients
    {
        private static readonly TelemetryClient telemetry = new TelemetryClient(TelemetryConfiguration.Active);

        [FunctionName("GetClients")]
        [Display(Name = "Get Clients", Description = "Get List of Clients")]
        [ProducesResponseType(200, Type = typeof(ClientDto))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "clients")] HttpRequest req,
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
                IGetClientsRepository<Client> getClientsRepository = 
                    new GetClientsRepository<Client>(documentClientFactory);
                IGetClientsQuery<Client> getClientsQuery = new GetClientsQuery<Client>(getClientsRepository);

                ITenantExtractor tenantExtractor = new HttpRequestTenantExtractor(req);

                return await GetClientProxy.RunProxy(
                    tenantExtractor,
                    log,
                    getClientsQuery);
            }
            catch (Exception ex)
            {
                log.LogError(ex.StackTrace);
                log.LogError(ex.Message, ex);
                telemetry.TrackException(ex);
                throw;
            }
        }
        
        public static class GetClientProxy
        {
            public static async Task<IActionResult> RunProxy(ITenantExtractor tenantExtractor, ILogger logger, 
                IGetClientsQuery<Client> getClientsQuery)
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

                IEnumerable<Client> allClients = await getClientsQuery.GetClients(tenantId);

                IEnumerable<ClientDto> allClientsDto;
                if (allClients == null)
                {
                    allClientsDto = new List<ClientDto>();
                }
                else
                {
                    allClientsDto = MappingHelper.Map<IEnumerable<Client>, IEnumerable<ClientDto>>(allClients, new ClientMappingProfile());
                    allClientsDto = allClientsDto.ToList().Select<ClientDto, ClientDto>(i =>
                    {
                        //On purpose we are faking some things here. This will go away when additional functionality is added.
                        ClientDto ClientDto = i;
//                        ProductDto.Created = DateTimeOffset.UtcNow;
//                        ProductDto.CreatedBy = "me";
//                        ProductDto.Modified = DateTimeOffset.UtcNow;
//                        ProductDto.ModifiedBy = "me";
                        return ClientDto;
                    });
                }

                return new OkObjectResult(allClientsDto);

            }
        }
    }
}