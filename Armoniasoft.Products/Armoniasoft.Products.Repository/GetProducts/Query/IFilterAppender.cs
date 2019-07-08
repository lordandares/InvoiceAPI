using System.Linq;

namespace Armoniasoft.Products.Repository.Query
{
    public interface IFilterAppender<T>
    {
        IQueryable<T> AppendTo(IQueryable<T> queryable);
    }
}