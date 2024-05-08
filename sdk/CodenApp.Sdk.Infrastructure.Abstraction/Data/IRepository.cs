using CodenApp.Sdk.Domain.Abstraction.Entities;
using Microsoft.EntityFrameworkCore;

namespace CodenApp.Sdk.Infrastructure.Abstraction.Data;

public interface IRepository<TEntity, TPrimaryKey> :
    IReadRepository<TEntity, TPrimaryKey>,
    IWriteRepository<TEntity, TPrimaryKey>
    where TEntity : class, IEntity<TPrimaryKey>
{
    DbContext Context { get; }
    DbSet<TEntity> Set { get; }
}
