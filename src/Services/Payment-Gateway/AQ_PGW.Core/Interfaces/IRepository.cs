using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AQ_PGW.Core.Interfaces
{
    public interface IRepository<T>
    {
        T Single(Expression<Func<T, bool>> where);
        T FirstOrDefault(Expression<Func<T, bool>> where);
        IQueryable<T> AsQueryable();
        IEnumerable<T> GetAll();
        IEnumerable<T> FindByQuery(Expression<Func<T, bool>> where);
        IQueryable<T> Where(Expression<Func<T, bool>> where);
        bool Any(Expression<Func<T, bool>> where);

        Task<IEnumerable<T>> GetAllAsync();


        void Add(T entity);
        void AddRange(IEnumerable<T> entity);
        void Update(T entity, params Expression<Func<T, object>>[] updatedProperties);
        void Update(List<T> entities, params Expression<Func<T, object>>[] updatedProperties);
        void Delete(T entity);
        void Delete(IEnumerable<T> entity);

        Task<int> UpdateAsync(T entity, params Expression<Func<T, object>>[] updatedProperties);
        Task<int> UpdateAsync(List<T> entities, params Expression<Func<T, object>>[] updatedProperties);

        Task<int> AddAsync(T entity);
        Task<int> AddRangeAsync(IEnumerable<T> entity);
    }
}
