using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BE.DAL.DTO;
using Microsoft.EntityFrameworkCore.Query;

namespace BE.DAL.GenericRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<PagedResult<T>> GetAllDataByExpression(QueryOptions<T> options);

        Task<T?> GetById(object id);

        public Task<T?> GetByExpression(Expression<Func<T, bool>> filter, List<Func<IQueryable<T>, IQueryable<T>>>? includeFuncs);

        Task<T> Insert(T entity);

        Task<IReadOnlyList<T>> InsertRange(IEnumerable<T> entities);

        Task<T> Update(T entity);

        Task<IReadOnlyList<T>> UpdateRange(IEnumerable<T> entities);

        Task<T?> DeleteById(object id);

        Task<IReadOnlyList<T>> DeleteRange(IEnumerable<T> entities);

        Task<int> Update(
            Expression<Func<T, bool>>? filter,
            Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> updateExpression);

        public Task<int> Delete(Expression<Func<T, bool>> filter);

    }

}
