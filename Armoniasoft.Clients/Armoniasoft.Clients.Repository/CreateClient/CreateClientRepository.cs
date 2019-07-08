using System;
using System.Threading.Tasks;
using Armoniasoft.ClientDB;
using DefaultNamespace;
using Microsoft.Azure.Documents;

namespace Armoniasoft.Clients.Repository.CreateClient
{
    public class CreateClientRepository<T> : ICreateClientRepository<T> where T : IClient
    {
        private readonly IClientDBDocumentClient clientDbDocumentClient;

        public CreateClientRepository(IDocumentClientFactory documentClientFactory)
        {
            clientDbDocumentClient = documentClientFactory.Get();
        }

        public async Task<T> Create(T model)
        {
            try
            {
                return await clientDbDocumentClient.CreateDocument(model.TenantId, model);
            }
            catch (DocumentClientException documentClientException)
            {
                Console.WriteLine($"documentClientException: {documentClientException.Error}");

                throw documentClientException;
            }
        }
    }
}