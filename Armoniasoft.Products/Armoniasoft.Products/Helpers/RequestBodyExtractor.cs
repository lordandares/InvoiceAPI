using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Armoniasoft.Products.Helpers
{
    public class HttpRequestBodyExtractor<T> : IRequestBodyExtractor<T>
    {
        private readonly HttpRequest httpRequest;

        public HttpRequestBodyExtractor(HttpRequest httpRequest)
        {
            this.httpRequest = httpRequest;
        }

        public async Task<T> GetBody()
        {
            try
            {
                string requestBody = await new StreamReader(httpRequest.Body).ReadToEndAsync();
                JsonSerializerSettings settings = new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore
                };
                T data = JsonConvert.DeserializeObject<T>(requestBody, settings);
                return data;
            }
            catch (Exception ex)
            {
                throw new RequestBodyInvalidException(ex);
            }
        }
    }

    public interface IRequestBodyExtractor<T>
    {
        Task<T> GetBody();
    }

    public abstract class HttpRequestBodyException : Exception
    {
        protected HttpRequestBodyException(string message, Exception ex) : base(message, ex) { }
    }
    public class RequestBodyInvalidException : HttpRequestBodyException
    {
        const string message = "Request body is in an invalid format";
        public RequestBodyInvalidException(Exception ex) : base(message, ex) { }
    }
}
