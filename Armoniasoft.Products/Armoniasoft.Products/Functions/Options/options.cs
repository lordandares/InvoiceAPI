using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Armoniasoft.Products.Mapping.Models.Product;

namespace Armoniasoft.Products.Functions.Options
{
    public static class options
    {
        [FunctionName("options")]
        [Display(Name = "Options", Description = "Options")]
        [ProducesResponseType(200)]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "options", Route = "products")] HttpRequest req,
            ILogger log)
        {
            Product product = new Product();
            OkObjectResult result = new OkObjectResult(product);

            return result;
        }
    }
}
