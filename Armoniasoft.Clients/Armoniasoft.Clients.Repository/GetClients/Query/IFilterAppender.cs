using System.Linq;

namespace Armoniasoft.Clients.Repository.Query
{
    public interface IFilterAppender<T>
    {
        IQueryable<T> AppendTo(IQueryable<T> queryable);
    }
}