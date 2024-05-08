using CodenApp.Sdk.Domain.Abstraction.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CodenApp.Sdk.Infrastructure.Abstraction.Data;

public interface IReadRepository<TEntity, TPrimaryKey>
    where TEntity : IEntity<TPrimaryKey>
{
    #region Sync

    TEntity GetById(TPrimaryKey id);
    IEnumerable<TEntity> GetAll();
    IPageQuery<TEntity> GetAll(IPageFilter filter);
    IPageQuery<TEntity> GetAllPage(int currentPage, int quantityPerPage);

    IPageQuery<TEntity> GetPageOrderAndClause(
        int currentPage,
        int quantityPerPage,
        Expression<Func<TEntity, bool>>? clause,
        Expression<Func<TEntity, object>> order
    );

    Task<IPageQuery<TEntity>> GetPageOrderAndClauseAsync(
        int currentPage,
        int quantityPerPage,
        Expression<Func<TEntity, bool>> clause,
        Expression<Func<TEntity, object>> order
    );

    bool IfExists(TPrimaryKey id);

    #endregion

    #region Assync

    Task<TEntity> GetByIdAsync(TPrimaryKey id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IPageQuery<TEntity>> GetAllAsync(IPageFilter filter);
    Task<IEnumerable<TEntity>> GetByExpression(Expression<Func<TEntity, bool>> predicate);

    #endregion
}
