using Newtonsoft.Json;
using Armoniasoft.ClientDB.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Armoniasoft.ClientDB.Providers
{
    public static class ResourceTokenProvider
    {
        private static HttpClient httpClient = new HttpClient();

        public static async Task<PermissionToken> GetResourceToken(string resourceTokenEndpoint, string resourceTokenApiKey, string tenantId, string collectionName, string permissionName)
        {
            try
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri(resourceTokenEndpoint + $"resourceToken/{tenantId}/{collectionName}/{permissionName}?code={resourceTokenApiKey}"),
                    Method = HttpMethod.Get,
                };
                HttpResponseMessage responseMessage = await httpClient.SendAsync(requestMessage);
                responseMessage.EnsureSuccessStatusCode();
                string responseContent = await responseMessage.Content.ReadAsStringAsync();
                PermissionToken permissionToken = JsonConvert.DeserializeObject<PermissionToken>(responseContent);
                return permissionToken;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the resource token: " + ex.Message, ex);
            }
        }
    }
}
