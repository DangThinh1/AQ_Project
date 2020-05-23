using AQBooking.Core.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQBooking.Core.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _dataContext;
        private DbSet<T> _entities;

        public IDbContextTransaction BeginTransaction()
        {
            return _dataContext.Database.BeginTransaction();
        }

        public Repository()
        {
            _dataContext = ServiceHelper.GetDbContext();
        }

        #region Methods
        public void SaveChanges()
        {
            _dataContext.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _dataContext.SaveChangesAsync();
        }

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public virtual T GetById(object id)
        {
            //see some suggested performance optimization (not tested)
            //http://stackoverflow.com/questions/11686225/dbset-find-method-ridiculously-slow-compared-to-singleordefault-on-id/11688189#comment34876113_11688189
            return this.Entities.Find(id);
        }

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual bool Insert(T entity)
        {
            try
            {
                if (entity == null)
                {
                    return false;
                    throw new ArgumentNullException($"{entity}");
                }
                this.Entities.Add(entity);
                SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual async Task<bool> InsertAsync(T entity)
        {
            try
            {
                if (entity == null)
                {
                    return false;
                    throw new ArgumentNullException($"{entity}");
                }

                await this.Entities.AddAsync(entity);
                await SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual bool Insert(IEnumerable<T> entities)
        {
            try
            {
                if (entities.Count() == 0 || entities == null)
                {
                    return false;
                    throw new ArgumentNullException($"{entities}");
                }

                this.Entities.AddRange(entities);
                SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// InsertAsync
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual async Task<bool> InsertAsync(IEnumerable<T> entities)
        {
            try
            {
                if (entities.Count() == 0 || entities == null)
                {
                    return false;
                    throw new ArgumentNullException($"{entities}");
                }
                await this.Entities.AddRangeAsync(entities);
                SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool Update(T entity)
        {
            try
            {
                if (entity == null)
                {
                    return false;
                    throw new ArgumentNullException($"{entity}");
                }

                SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual bool Update(IEnumerable<T> entites)
        {
            try
            {
                if (entites.Count() == 0 || entites == null)
                {
                    return false;
                    throw new ArgumentNullException($"{entites}");
                }

                SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// UpdateAsync
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                if (entity == null)
                {
                    return false;
                    throw new ArgumentNullException($"{entity}");
                }

                await SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual async Task<bool> UpdateAsync(IEnumerable<T> entities)
        {
            try
            {
                if (entities.Count() == 0 || entities == null)
                {
                    return false;
                    throw new ArgumentNullException($"{entities}");
                }

                await SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual bool Delete(T entity)
        {
            try
            {
                if (entity == null)
                {
                    return false;
                    throw new ArgumentNullException($"{entity}");
                }

                this.Entities.Remove(entity);
                SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual bool Delete(IEnumerable<T> entities)
        {
            try
            {
                if (entities.Count() == 0 || entities == null)
                {
                    return false;
                    throw new ArgumentNullException($"{entities}");
                }

                this.Entities.RemoveRange(entities);
                SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual async Task<bool> DeleteAsync(T entity)
        {
            try
            {
                if (entity == null)
                {
                    return false;
                    throw new ArgumentNullException($"{entity}");
                }

                await Task.Run(() => this.Entities.Remove(entity));
                await SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual async Task<bool> DeleteAsync(IEnumerable<T> entities)
        {
            try
            {
                if (entities.Count() == 0 || entities == null)
                {
                    return false;
                    throw new ArgumentNullException($"{entities}");
                }

                await Task.Run(() => this.Entities.RemoveRange(entities));
                await SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<T> Table => this.Entities;

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        public virtual IQueryable<T> TableNoTracking => this.Entities.AsNoTracking();

        /// <summary>
        /// Entities
        /// </summary>
        protected virtual DbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                {
                    _entities = _dataContext.Set<T>();
                }

                return _entities;
            }
        }
        #endregion
    }
}
