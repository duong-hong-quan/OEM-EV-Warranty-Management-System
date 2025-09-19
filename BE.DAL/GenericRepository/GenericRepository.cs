using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using BE.DAL.DTO;
using BE.DAL.Models;

namespace BE.DAL.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(IDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = context.Set<T>();
        }

        #region Core Helpers

        private IQueryable<T> ApplyIncludes(IQueryable<T> query, List<Func<IQueryable<T>, IQueryable<T>>>? includeFuncs)
        {
            if (includeFuncs != null)
            {
                foreach (var include in includeFuncs)
                {
                    query = include(query);
                }
            }
            return query;
        }

        private IQueryable<T> ApplyFilter(IQueryable<T> query, Expression<Func<T, bool>>? filter)
        {
            return filter != null ? query.Where(filter) : query;
        }

        private IQueryable<T> ApplyDynamicOrdering(IQueryable<T> query, IEnumerable<(string PropertyName, bool Ascending)>? orderBy)
        {
            if (orderBy == null || !orderBy.Any())
                return query;

            var parameter = Expression.Parameter(typeof(T), "x");
            IOrderedQueryable<T>? orderedQuery = null;

            foreach (var (propertyName, ascending) in orderBy)
            {
                var property = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (property == null) continue;

                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var lambda = Expression.Lambda(propertyAccess, parameter);

                var methodName = orderedQuery == null
                    ? ascending ? "OrderBy" : "OrderByDescending"
                    : ascending ? "ThenBy" : "ThenByDescending";

                var method = typeof(Queryable).GetMethods()
                    .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), property.PropertyType);

                orderedQuery = (IOrderedQueryable<T>)method.Invoke(null, new object[] { orderedQuery ?? query, lambda })!;
            }

            return orderedQuery ?? query;
        }

        private async Task<PagedResult<T>> Paginate(IQueryable<T> query, int pageNumber, int pageSize)
        {
            var totalItems = await query.CountAsync();
            var totalPages = pageSize > 0 ? (int)Math.Ceiling(totalItems / (double)pageSize) : 0;

            var items = pageSize > 0 && pageNumber > 0
                ? await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync()
                : await query.ToListAsync();

            return new PagedResult<T>
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                Items = items
            };
        }

        #endregion

        #region Public API

        public async Task<PagedResult<T>> GetAllDataByExpression(QueryOptions<T> options)
        {
            IQueryable<T> query = options.UseIdentityResolution
                ? _dbSet.AsNoTrackingWithIdentityResolution()
                : _dbSet.AsNoTracking();

            if (options.IgnoreQueryFilters)
                query = query.IgnoreQueryFilters();

            query = ApplyIncludes(query, options.Includes);
            query = ApplyFilter(query, options.Filter);
            query = ApplyDynamicOrdering(query, options.OrderBy);

            return await Paginate(query, options.PageNumber, options.PageSize);
        }

        public async Task<T?> GetById(object id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
                _context.Entry(entity).State = EntityState.Detached;

            return entity;
        }

        public async Task<T> Insert(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task<T> Update(T entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                var pk = _context.Model.FindEntityType(typeof(T))?.FindPrimaryKey()?.Properties.FirstOrDefault();
                if (pk == null) throw new InvalidOperationException("Entity has no primary key defined.");

                var key = pk.PropertyInfo?.GetValue(entity);
                var tracked = await _dbSet.FindAsync(key)
                             ?? throw new InvalidOperationException($"Entity with key {key} not found.");

                _context.Entry(tracked).CurrentValues.SetValues(entity);
                return tracked;
            }

            entry.State = EntityState.Modified;
            return entity;
        }

        public async Task<T?> DeleteById(object id)
        {
            if (id == null) return null;

            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return null;

            _dbSet.Remove(entity);
            return entity;
        }

        public async Task<T?> GetByExpression(Expression<Func<T, bool>> filter, List<Func<IQueryable<T>, IQueryable<T>>>? includeFuncs)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();
            query = ApplyIncludes(query, includeFuncs);
            return await query.FirstOrDefaultAsync(filter);
        }

        public async Task<IReadOnlyList<T>> InsertRange(IEnumerable<T> entities)
        {
            if (entities == null || !entities.Any())
                return Array.Empty<T>();

            await _dbSet.AddRangeAsync(entities);
            return entities.ToList().AsReadOnly();
        }

        public async Task<IReadOnlyList<T>> UpdateRange(IEnumerable<T> entities)
        {
            if (entities == null || !entities.Any())
                return Array.Empty<T>();

            var list = entities.ToList();
            var entityType = _context.Model.FindEntityType(typeof(T));
            var pkProperties = entityType?.FindPrimaryKey()?.Properties
                               ?? throw new InvalidOperationException("Entity has no primary key defined.");

            Func<T, string> getKeyString = entity =>
                string.Join("::", pkProperties.Select(p => p.PropertyInfo?.GetValue(entity)?.ToString() ?? "null"));

            var keys = list.Select(getKeyString).ToHashSet();

            var parameter = Expression.Parameter(typeof(T), "e");
            Expression body = Expression.Constant(false);

            // Build OR expression for matching composite keys
            foreach (var key in keys)
            {
                Expression current = null;
                var values = key.Split("::");
                for (int i = 0; i < pkProperties.Count; i++)
                {
                    var prop = pkProperties[i];
                    var propType = prop.ClrType;

                    var targetType = Nullable.GetUnderlyingType(propType) ?? propType;
                    object typedValue;
                    try
                    {
                        typedValue = Convert.ChangeType(values[i], targetType);
                    }
                    catch (Exception)
                    {
                        throw new InvalidCastException($"Cannot convert '{values[i]}' to {targetType.Name} for key '{prop.Name}'");
                    }

                    var valueExpression = Expression.Constant(typedValue, targetType);
                    var entityProp = Expression.Property(parameter, prop.PropertyInfo.Name);

                    Expression left = entityProp.Type != targetType
                        ? Expression.Convert(entityProp, targetType)
                        : entityProp;

                    var equal = Expression.Equal(left, valueExpression);
                    current = current == null ? equal : Expression.AndAlso(current, equal);
                }

                body = Expression.OrElse(body, current);
            }

            var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);

            var tracked = await _dbSet.Where(lambda).ToListAsync();
            var trackedDict = tracked.ToDictionary(getKeyString);

            foreach (var entity in list)
            {
                var key = getKeyString(entity);
                if (trackedDict.TryGetValue(key, out var trackedEntity))
                {
                    _context.Entry(trackedEntity).CurrentValues.SetValues(entity);
                }
                else
                {
                    throw new InvalidOperationException($"Entity with key {key} not found.");
                }
            }

            return list.AsReadOnly();
        }


        public async Task<IReadOnlyList<T>> DeleteRange(IEnumerable<T> entities)
        {
            if (entities == null || !entities.Any())
                return Array.Empty<T>();

            _dbSet.RemoveRange(entities);
            return entities.ToList().AsReadOnly();
        }

        public async Task<int> Update(
            Expression<Func<T, bool>>? filter,
            Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> updateExpression)
        {
            var query = filter != null ? _dbSet.Where(filter) : _dbSet;
            return await query.ExecuteUpdateAsync(updateExpression);
        }
        public async Task<int> Delete(Expression<Func<T, bool>> filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            return await _dbSet.Where(filter).ExecuteDeleteAsync();
        }

        #endregion
    }

}
