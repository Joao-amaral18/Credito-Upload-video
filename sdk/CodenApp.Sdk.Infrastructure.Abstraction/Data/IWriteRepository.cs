using CodenApp.Sdk.Domain.Abstraction.Entities;
using System;
using System.Threading.Tasks;

namespace CodenApp.Sdk.Infrastructure.Abstraction.Data;

public interface IWriteRepository<TEntity, TPrimaryKey>
    where TEntity : IEntity<TPrimaryKey>
{
    #region Sync

    void Insert(TEntity entity);
    void Update(TEntity entity);
    void Delete(TPrimaryKey id);
    void Delete(TEntity entity);
    int SaveChanges();

    #endregion

    #region Async

    Task InsertAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TPrimaryKey id);
    Task DeleteAsync(TEntity entity);
    Task<int> SaveChangesAsync();

    #endregion
}
