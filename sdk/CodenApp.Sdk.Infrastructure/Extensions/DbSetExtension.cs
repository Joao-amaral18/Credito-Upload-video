using CodenApp.Sdk.Infrastructure.Abstraction.Data;
using System.Linq;

namespace CodenApp.Sdk.Infrastructure.Extensions
{
    internal static class IQuerableExtension
    {
        internal static IQueryable<TEntity> ApplyPagination<TEntity>(this IQueryable<TEntity> query, IPageFilter filter)
        {
            var skip = (filter.CurrentPage - 1) * filter.QuantityPerPage;
            query = query.Skip(skip).Take(filter.QuantityPerPage);
            return query;
        }

        internal static IQueryable<TEntity> ApplyPagination<TEntity>(this IQueryable<TEntity> query, int currentPage, int quantityPerPage)
        {
            var skip = (currentPage - 1) * quantityPerPage;
            query = query.Skip(skip).Take(quantityPerPage);
            return query;
        }
    }
}
