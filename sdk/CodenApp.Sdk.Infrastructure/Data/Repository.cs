using CodenApp.Sdk.Domain.Abstraction.Entities;
using CodenApp.Sdk.Infrastructure.Extensions;
using CodenApp.Sdk.Infrastructure.Abstraction.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace CodenApp.Sdk.Infrastructure.Data
{
    public class Repository<TEntity, TPrimaryKey> :
        IRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>

    {
        public DbContext Context { get; protected set; }

        public DbSet<TEntity> Set { get; protected set; }

        public Repository(DbContext context)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
            this.Set = context.Set<TEntity>();
            this.Context.Database.SetCommandTimeout(300);
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

        public async Task<IPageQuery<TEntity>> GetAllAsync(IPageFilter filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            var pageReturn = new PageQuery<TEntity>
            {
                CurrentPage = filter.CurrentPage,
                QuantityPerPage = filter.QuantityPerPage
            };

            var taskCount = this.Set.CountAsync();
            var query = this.Set.ApplyPagination(filter);

            pageReturn.Itens = query.AsEnumerable();
            pageReturn.Total = await taskCount;

            double total = (double)pageReturn.Total / (double)filter.QuantityPerPage;
            int totalPages = Convert.ToInt32(Math.Ceiling(total));

            pageReturn.Next = filter.CurrentPage < totalPages ? Convert.ToString(filter.CurrentPage + 1) : null;
            pageReturn.Previous = filter.CurrentPage == 1 ? null : Convert.ToString(filter.CurrentPage - 1);

            pageReturn.TotalPages = totalPages;

            return pageReturn;
        }

        public IPageQuery<TEntity> GetAllPage(int currentPage, int quantityPerPage)
        {
            return this.GetAllPageAsync(currentPage, quantityPerPage).GetAwaiter().GetResult();
        }

        public async Task<IPageQuery<TEntity>> GetAllPageAsync(int currentPage, int quantityPerPage)
        {
            var pageReturn = new PageQuery<TEntity>
            {
                CurrentPage = currentPage,
                QuantityPerPage = quantityPerPage
            };

            var taskCount = this.Set.CountAsync();
            var query = this.Set.ApplyPagination(currentPage, quantityPerPage);

            pageReturn.Itens = query.AsEnumerable();
            pageReturn.Total = await taskCount;

            double total = (double)pageReturn.Total / (double)quantityPerPage;
            int totalPages = Convert.ToInt32(Math.Ceiling(total));

            pageReturn.Next = currentPage < totalPages ? Convert.ToString(currentPage + 1) : null;
            pageReturn.Previous = currentPage == 1 ? null : Convert.ToString(currentPage - 1);
            pageReturn.TotalPages = totalPages;

            return pageReturn;
        }

        public IPageQuery<TEntity> GetPageOrderAndClause(
            int currentPage,
            int quantityPerPage,
            Expression<Func<TEntity, bool>>? clause,
            Expression<Func<TEntity, object>> order
        )
        {
            return this.GetPageOrderAndClauseAsync(
                currentPage,
                quantityPerPage,
                clause,
                order
            ).GetAwaiter().GetResult();
        }

        public async Task<IPageQuery<TEntity>> GetPageOrderAndClauseAsync(
            int currentPage,
            int quantityPerPage,
            Expression<Func<TEntity, bool>> clause,
            Expression<Func<TEntity, object>> order
        )
        {
            var pageReturn = new PageQuery<TEntity>
            {
                CurrentPage = currentPage,
                QuantityPerPage = quantityPerPage
            };

            var taskCount = await this.Set.CountAsync();
            IQueryable<TEntity> query;

            query = this.Set.Where(clause).OrderBy(order).ApplyPagination(currentPage, quantityPerPage);

            pageReturn.Itens = query.AsEnumerable();
            pageReturn.Total = query.Count();

            double total = (double)pageReturn.Total / (double)quantityPerPage;
            int totalPages = Convert.ToInt32(Math.Ceiling(total));

            pageReturn.Next = currentPage < totalPages ? Convert.ToString(currentPage + 1) : null;
            pageReturn.Previous = currentPage == 1 ? null : Convert.ToString(currentPage - 1);
            pageReturn.TotalPages = totalPages;

            return pageReturn;
        }

        public IPageQuery<TEntity> GetPageOrderAndClause(
            int currentPage,
            int quantityPerPage,
            Expression<Func<TEntity, bool>>? clause,
            Expression<Func<TEntity, object>> order,
            bool asc
        )
        {
            IOrderedQueryable<TEntity> query = this.Set.Where(clause).OrderBy(order);

            return this.GetPageOrderAndClauseAsync(
                currentPage,
                quantityPerPage,
                query
            ).GetAwaiter().GetResult();
        }

        public async Task<IPageQuery<TEntity>> GetPageOrderAndClauseAsync(
            int currentPage,
            int quantityPerPage,
            IOrderedQueryable<TEntity> orderedQueryable)
        {
            var pageReturn = new PageQuery<TEntity>
            {
                CurrentPage = currentPage,
                QuantityPerPage = quantityPerPage
            };

            var taskCount = await this.Set.CountAsync();
            IQueryable<TEntity> query;

            query = orderedQueryable.ApplyPagination(currentPage, quantityPerPage);

            pageReturn.Itens = query.AsEnumerable();
            pageReturn.Total = orderedQueryable.Count();

            double total = (double)pageReturn.Total / (double)quantityPerPage;
            int totalPages = Convert.ToInt32(Math.Ceiling(total));

            pageReturn.Next = currentPage < totalPages ? Convert.ToString(currentPage + 1) : null;
            pageReturn.Previous = currentPage == 1 ? null : Convert.ToString(currentPage - 1);
            pageReturn.TotalPages = totalPages;

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

        public bool IfExists(TPrimaryKey id)
        {
            return this.Set.Any(x => x.Id.Equals(id));
        }

        public async Task<IEnumerable<TEntity>> GetByExpression(Expression<Func<TEntity, bool>> predicate)
        {
            return await this.Set.AsNoTracking().Where(predicate).ToListAsync();
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
