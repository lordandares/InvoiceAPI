using System.Linq;
using Armoniasoft.Products.Mapping.Models.Product;

namespace Armoniasoft.Products.Repository.Query
{
    public class GetProductsFilter<T> : IFilterAppender<T> where T : IProduct
    {
        private const string ITEM_TYPE = "product";

        public IQueryable<T> AppendTo(IQueryable<T> queryable)
        {
            return queryable.Where(item => item.Type == ITEM_TYPE);
        }
    }
}