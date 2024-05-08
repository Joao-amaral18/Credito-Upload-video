using Crefaz.Documento.Infra.Data;
using System.Linq;

namespace CodenApp.Sdk.Infrastructure.Extensions
{
    internal static class IQuerableExtension
    {
        internal static IQueryable<TEntity> ApplayPagination<TEntity>(this IQueryable<TEntity> query, IPageFilter filter)
        {
            var skip = (filter.Page - 1) * filter.Size;
            query = query.Skip(skip).Take(filter.Size);
            return query;
        }
    }
}
