using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Armoniasoft.ClientDB;
using Armoniasoft.Invoices.Helpers;
using Armoniasoft.Invoices.Repository.CreateInvoices;
using Armoniasoft.Invoices.Mapping.Models;
using Armoniasoft.Invoices.Service.CreateInvoices;
using Armoniasoft.Invoices.Service.Helper;
using Armoniasoft.Invoices.Service.GetReportInvoice.Helpers;
using Armoniasoft.Invoices.Service.GetReportInvoice;

namespace Armoniasoft.Invoices.Functions
{
    public static class CreateInvoice
    {
        private static readonly TelemetryClient telemetry = new TelemetryClient(TelemetryConfiguration.Active);

        [FunctionName("CreateInvoice")]
        [Display(Name = "Create Task", Description = "Create Product")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "invoices")] HttpRequest req,
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
                ICreateInvoiceRepository<Invoice> createInvoiceRepository = new CreateInvoiceRepository<Invoice>(documentClientFactory);
                ICreateInvoiceQuery<Invoice> createProductQuery = new CreateInvoiceQuery<Invoice>(createInvoiceRepository);
                IInvoiceFactory InvoiceFactory = new InvoiceFactory();
                ITenantExtractor tenantExtractor = new HttpRequestTenantExtractor(req);
                IRequestBodyExtractor<Invoice> requestBodyExtractor = new HttpRequestBodyExtractor<Invoice>(req);
                IDocumenFormater documentFormater = new DocumentFormater();
                IGetReportInvoice<Invoice> getReportInvoice = new GetReportInvoice<Invoice>(documentFormater);
                return await CreateInvoiceProxy.RunProxy(
                    tenantExtractor,
                    log,
                    createProductQuery,
                    requestBodyExtractor,
                    InvoiceFactory,
                    getReportInvoice
                    );
            }
            catch (Exception ex)
            {
                log.LogError(ex.Message, ex);
                telemetry.TrackException(ex);
                throw;
            }
        }
    }
    public static class CreateInvoiceProxy
    {
        public static async Task<IActionResult> RunProxy(ITenantExtractor tenantExtractor, ILogger logger, ICreateInvoiceQuery<Invoice> createInvoiceQuery, IRequestBodyExtractor<Invoice> requestBodyExtractor, IInvoiceFactory invoiceFactory, IGetReportInvoice<Invoice> getReportInvoice)
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

            Invoice body = invoiceFactory.IdentifyTypeofInvoice("Estandar");
            try
            {
                body = await requestBodyExtractor.GetBody();
            }
            catch (RequestBodyInvalidException ex)
            {
                logger.LogWarning(ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }

            body.TenantId = tenantId;
            Invoice createdInvoice = await createInvoiceQuery.Create(body);

            byte[] document = getReportInvoice.getReport(createdInvoice);

            
            
            FileContentResult result = new FileContentResult(document, System.Net.Mime.MediaTypeNames.Application.Pdf);
            return result;

            // object createdObject = new { value = createdTaskDto };

        }
    }
}
