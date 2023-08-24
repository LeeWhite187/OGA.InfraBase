using Microsoft.EntityFrameworkCore;
using OGA.DomainBase.QueryHelpers;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using OGA.InfraBase.QueryHelpers;

namespace OGA.DomainBase.Repositories
{
    public class Repository<TEntity, TId> : OGA.DomainBase.Repositories.IRepository<TEntity, TId> where TEntity : class, OGA.DomainBase.Models.IAggregateRoot<TId> where TId : IEquatable<TId>
    {
        protected readonly DbContext Context;

        protected readonly DbSet<TEntity> _dbSet;

        public Repository(DbContext context)
        {
            Context = context;

            if (context != null)
            {
                _dbSet = context.Set<TEntity>();
            }
            else
            {
                throw new Exception("Context is null");
            }
        }

        public virtual bool Add(TEntity entity)
        {
            try
            {
                _dbSet.Add(entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual async Task<bool> AddAsync(TEntity entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual async Task<bool> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            try
            {
                await _dbSet.AddRangeAsync(entities);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public virtual bool RemoveById(TId id)
        {
            try
            {
                // Removal requires getting a reference to the instance.

                // Get the instance to remove...
                var inst = GetById(id);
                if (inst == null)
                    return true;

                // If here, we have the instance, and can remove it.
                // Remove the instance...
                _dbSet.Remove(inst);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public virtual bool Remove(TEntity entity)
        {
            try
            {
                _dbSet.Remove(entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveRange(IEnumerable<TEntity> entities)
        {
            try
            {
                _dbSet.RemoveRange(entities);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public virtual bool Update_ById(TId id, Dictionary<string, string> changes)
        {
            // Is entity type specific.
            throw new OGA.SharedKernel.Exceptions.BusinessRuleBrokenException("Type not supported.");
        }

        public virtual bool Update(TEntity entity)
        {
            try
            {
                _dbSet.Update(entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual async Task<bool> Upsert(TEntity entity)
        {
            try
            {
                // Attempt to locate the current entity...
                // This equality comparator was added so that we could continue to treat the entity ID as a generic type.
                // Otherwise, the simple equals comparison creates a compiler error.
                var existingUser = await _dbSet.Where(x => EqualityComparer<TId>.Default.Equals(x.Id, entity.Id))
                                                .FirstOrDefaultAsync();
                //var existingUser = await _dbSet.Where(x => x.Id == entity.Id)
                //                                .FirstOrDefaultAsync();

                // See if the entity was found...
                if (existingUser == null)
                    // Not found, add the new entity...
                    return await AddAsync(entity);

                // Entity was found.
                // Update the existing entity...
                return Update(entity);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public TEntity GetById(TId id)
        {
            return _dbSet.Find(id);
        }

        public async Task<TEntity> GetByIdAsync(TId id)
        {
            return await _dbSet.FindAsync(id).ConfigureAwait(false);
        }
        public IEnumerable<TEntity> Get_byIDList(IEnumerable<TId> ids)
        {
            IEnumerable<TEntity> fff = this._dbSet.Where(m => ids.Contains(m.Id));
            return fff;
        }

        public IEnumerable<TEntity> GetAll()
        {
            try
            {
                return _dbSet.ToList();
            }
            catch (Exception)
            {
                return new List<TEntity>();
            }
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            try
            {
                return await _dbSet.ToListAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
                return new List<TEntity>();
            }
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync<TProperty>(Expression<Func<TEntity, TProperty>> include)
        {
            IQueryable<TEntity> query = _dbSet.Include(include);

            return await query.ToListAsync().ConfigureAwait(false);
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.SingleOrDefault(predicate);
        }
        public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.SingleOrDefaultAsync(predicate).ConfigureAwait(false);
        }


        public virtual async Task<IPaginatedList<TEntity>> GetPageAsync(QueryObjectParams queryObjectParams)
        {
            return await GetOrderedPageQueryResultAsync(queryObjectParams, _dbSet).ConfigureAwait(false);
        }

        public virtual async Task<IPaginatedList<TEntity>> GetPageAsync(QueryObjectParams queryObjectParams, Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = _dbSet;

            if (predicate != null)
                query = query.Where(predicate);

            return await GetOrderedPageQueryResultAsync(queryObjectParams, query).ConfigureAwait(false);
        }

        public virtual async Task<IPaginatedList<TEntity>> GetPageAsync(QueryObjectParams queryObjectParams, List<Expression<Func<TEntity, object>>> includes)
        {
            IQueryable<TEntity> query = _dbSet;

            query = includes.Aggregate(query, (current, include) => current.Include(include));

            return await GetOrderedPageQueryResultAsync(queryObjectParams, query).ConfigureAwait(false);
        }

        public virtual async Task<IPaginatedList<TEntity>> GetPageAsync<TProperty>(QueryObjectParams queryObjectParams,
                                                                                Expression<Func<TEntity, bool>> predicate,
                                                                                List<Expression<Func<TEntity, TProperty>>> includes = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await GetOrderedPageQueryResultAsync(queryObjectParams, query).ConfigureAwait(false);
        }

        public virtual async Task<IPaginatedList<TEntity>> GetOrderedPageQueryResultAsync(QueryObjectParams queryObjectParams, IQueryable<TEntity> query)
        {
            // Accept the incoming queried dbset...
            IQueryable<TEntity> OrderedQuery = query;

            // Apply sorting to the query if defined...
            if (queryObjectParams.SortingParams != null && queryObjectParams.SortingParams.Count > 0)
            {
                // Sorting is defined.

                // Apply sorting...
                OrderedQuery = SortingExtension.GetOrdering(query, queryObjectParams.SortingParams);
            }

            // Check if the sorted result is defined...
            if (OrderedQuery != null)
            {
                // Query is defined.

                var oqpl = await PaginatedList<TEntity>.CreateAsync(OrderedQuery, queryObjectParams.pageNumber, queryObjectParams.pageSize);
                return oqpl;

                //var fecthedItems = await GetPagePrivateQuery(OrderedQuery, queryObjectParams).ToListAsync().ConfigureAwait(false);

                //return new PaginatedList<TEntity>(fecthedItems, totalCount);
            }
            // If here, the query became null after the sorting extension.
            // So, the code is doing a simple skip and take.

            // Tell the paginated list to give us the desired page for the caller...
            var pl = await PaginatedList<TEntity>.CreateAsync(_dbSet, queryObjectParams.pageNumber, queryObjectParams.pageSize);
            return pl;

            //return new PaginatedList<TEntity>(await GetPagePrivateQuery(_dbSet, queryObjectParams).ToListAsync().ConfigureAwait(false), totalCount);
        }

        private IQueryable<TEntity> GetPagePrivateQuery(IQueryable<TEntity> query, QueryObjectParams queryObjectParams)
        {
            return query.Skip((queryObjectParams.pageNumber - 1) * queryObjectParams.pageSize).Take(queryObjectParams.pageSize);
        }
    }
}
