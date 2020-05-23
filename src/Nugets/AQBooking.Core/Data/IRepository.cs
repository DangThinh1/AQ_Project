using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQBooking.Repositories.Data
{
    public interface IRepository<T> where T:class
    {
        #region No Asyn
        void SaveChanges();
        Task<int> SaveChangesAsync();
        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        T GetById(object id);

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        bool Insert(T entity);

        /// <summary>
        /// InnsertAsync entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> InsertAsync(T entity);

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        bool Insert(IEnumerable<T> entities);

        /// <summary>
        /// InsertAsync entities
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<bool> InsertAsync(IEnumerable<T> entities);

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Update(T entity);

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entites"></param>
        /// <returns></returns>
        bool Update(IEnumerable<T> entites);

        /// <summary>
        /// UpdateAsync entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(T entity);

        /// <summary>
        /// UpdateAsync entities
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(IEnumerable<T> entities);

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        bool Delete(T entity);

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        bool Delete(IEnumerable<T> entities);

        /// <summary>
        /// DeleteAsync entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(T entity);

        /// <summary>
        /// DeleteAsync entities
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(IEnumerable<T> entities);

        /// <summary>
        /// Gets a table
        /// </summary>
        IQueryable<T> Table { get; }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        IQueryable<T> TableNoTracking { get; }
        IDbContextTransaction BeginTransaction();

        #endregion


    }
}
