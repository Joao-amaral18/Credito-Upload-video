using CodenApp.Sdk.Domain.Abstraction.Entities;
using CodenApp.Sdk.Infra.Data;
using CodenApp.Sdk.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CodenApp.Sdk.Infrastructure.Data
{
    public class Repository<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>

    {
        public DbContext Context { get; protected set; }

        public DbSet<TEntity> Set { get; protected set; }

        public Repository(DbContext context)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
            this.Set = context.Set<TEntity>();
        }

        #region ReadRepository

        public IEnumerable<TEntity> GetAll()
        {
            return this.GetAllAsync().GetAwaiter().GetResult();
        }

        public IPageQuery<TEntity> GetAll(IPageFilter filter)
        {
            return this.GetAllAsync(filter).GetAwaiter().GetResult();
        }

        public Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                return this.Set.AsEnumerable();
            }, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
        }

        // public async Task<IEnumerable<TEntity>> GetByExpression(Expression<Func<TEntity, bool>> predicate)
        // {
        //     return await this.Set.AsNoTracking().Where(predicate).ToListAsync();
        // }

        public async Task<IPageQuery<TEntity>> GetAllAsync(IPageFilter filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            var pageReturn = new PageQuery<TEntity>
            {
                Page = filter.Page,
                Size = filter.Size
            };

            var taskCount = this.Set.CountAsync();
            var query = this.Set.ApplayPagination(filter);

            pageReturn.Itens = query.AsEnumerable();
            pageReturn.Total = await taskCount;

            return pageReturn;
        }

        public TEntity GetById(TPrimaryKey id)
        {
            return this.GetByIdAsync(id).GetAwaiter().GetResult();
        }

        public Task<TEntity> GetByIdAsync(TPrimaryKey id)
        {
            return this.Set.FindAsync(id).AsTask();
        }

        #endregion

        #region WriteRepository

        public void Delete(TPrimaryKey id)
        {
            this.DeleteAsync(id).GetAwaiter().GetResult();
        }

        public void Delete(TEntity entity)
        {
            this.DeleteAsync(entity).GetAwaiter().GetResult();
        }

        public Task DeleteAsync(TPrimaryKey id)
        {
            var entity = this.Set.Find(id);
            return this.DeleteAsync(entity);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            if (entity != null && entity is IDeletable)
            {
                ((IDeletable)entity).Delete();
                await this.UpdateAsync(entity);
            }
            else
            {
                this.Set.Attach(entity).State = EntityState.Deleted;
            }
        }

        public void Insert(TEntity entity)
        {
            this.InsertAsync(entity).GetAwaiter().GetResult();
        }

        public Task InsertAsync(TEntity entity)
        {
            return this.Set.AddAsync(entity).AsTask();
        }

        public int SaveChanges()
        {
            return this.SaveChangesAsync().GetAwaiter().GetResult();
        }

        public Task<int> SaveChangesAsync()
        {
            return this.Context.SaveChangesAsync();
        }

        public void Update(TEntity entity)
        {
            this.UpdateAsync(entity).GetAwaiter().GetResult();
        }

        public Task UpdateAsync(TEntity entity)
        {
            return Task.Factory.StartNew(() =>
            {
                this.Set.Update(entity);
            }, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default); ;
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Context.Dispose();
            }
        }
        #endregion
    }
}
