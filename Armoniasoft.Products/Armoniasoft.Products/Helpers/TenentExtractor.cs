using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Armoniasoft.Products.Helpers
{
       public class HttpRequestTenantExtractor : ITenantExtractor
        {
            private readonly HttpRequest httpRequest;
            public HttpRequestTenantExtractor(HttpRequest httpRequest)
            {
                this.httpRequest = httpRequest;
            }

            public string GetTenantId()
            {
                try
                {
                    if (httpRequest.Headers.All(x => x.Key.Equals("tenantId", StringComparison.InvariantCultureIgnoreCase)))
                        throw new TenantIdNotFoundException();

                    string tenantId = httpRequest.Headers["tenantId"].ToString();
                    if (string.IsNullOrWhiteSpace(tenantId))
                    {
                        throw new TenantIdBlankException();
                    }
                    return tenantId;
                }
                catch (TenantIdNotFoundException)
                {
                    throw;
                }
                catch (TenantIdBlankException)
                {
                    throw;
                }
            }
        }

        public interface ITenantExtractor
        {
            string GetTenantId();
        }

        public abstract class TenantExtractorException : Exception
        {
            protected TenantExtractorException(string message) : base(message) { }
        }
        public class TenantIdNotFoundException : TenantExtractorException
        {
            const string message = "TenantId missing in request headers";
            public TenantIdNotFoundException() : base(message) { }
        }

        public class TenantIdBlankException : TenantExtractorException
        {
            const string message = "TenantId present in request headers, but is blank";
            public TenantIdBlankException() : base(message) { }
        }
    }

