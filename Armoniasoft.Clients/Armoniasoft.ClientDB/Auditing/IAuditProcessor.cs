using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Armoniasoft.ClientDB.Models;
using System.Threading.Tasks;

namespace Armoniasoft.ClientDB
{
    public interface IAuditProcessor
    {
        Task<ResourceResponse<Document>> OnDocumentUpdated<T>(AuditUser user, T oldDocument, T newDocument);
        Task<ResourceResponse<Document>> OnDocumentCreated<T>(AuditUser user, T newDocument);
    }
}