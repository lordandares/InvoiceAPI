using Newtonsoft.Json;

namespace Armoniasoft.ClientDB.Models
{
    public class AuditUser
    {
        public AuditUser()
        {
        }

        [JsonIgnore]
        public string TenantId { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}