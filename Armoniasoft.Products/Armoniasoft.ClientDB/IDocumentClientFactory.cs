using Microsoft.Azure.Documents;

namespace Armoniasoft.ClientDB
{
    public interface IDocumentClientFactory
    {
        IDocumentClient GetDocumentClient();
        IClientDBDocumentClient Get();
    }
}
