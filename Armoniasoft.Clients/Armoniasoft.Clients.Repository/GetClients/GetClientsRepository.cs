using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Armoniasoft.ClientDB;
using Armoniasoft.Clients.Repository.Query;
using DefaultNamespace;
using Microsoft.Azure.Documents;

namespace Armoniasoft.Clients.Repository
{
    public class GetClientsRepository<T> : IGetClientsRepository<T> where T : IClient
    {
        private readonly IClientDBDocumentClient clientDbDocumentClient;
        
        public GetClientsRepository(IDocumentClientFactory documentClientFactory)
        {
            clientDbDocumentClient = documentClientFactory.Get();
        }
        
        public async Task<IEnumerable<T>> Get(string tenantId)
        {
            IFilterAppender<T> filter = new GetClientsFilter<T>();
            
            IQueryable<T> appender(IQueryable<T> qry) => qry = filter.AppendTo(qry);
            try
            {
                return await clientDbDocumentClient.GetDocuments<T>(tenantId, appender);
            }
            catch (DocumentClientException documentClientException)
            {
                Console.WriteLine($"documentClientException: {documentClientException.Error}");
                
                if (documentClientException.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw documentClientException;
                }
                else
                    throw;
            }
        }
    }
}