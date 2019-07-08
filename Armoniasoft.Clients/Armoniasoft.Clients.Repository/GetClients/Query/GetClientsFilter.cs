using System.Linq;
using DefaultNamespace;

namespace Armoniasoft.Clients.Repository.Query
{
    public class GetClientsFilter<T> : IFilterAppender<T> where T : IClient
    {
//        private const string ITEM_TYPE = "product";

        public IQueryable<T> AppendTo(IQueryable<T> queryable)
        {
            return queryable.Where(item => item.Name != null);
        }
    }
}