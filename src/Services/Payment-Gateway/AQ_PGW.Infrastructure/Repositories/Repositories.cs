using AQ_PGW.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AQ_PGW.Infrastructure.Repositories
{
    public class Host
    {
        public static string HostName { get; set; }
    }
    public class Repository<T> : IRepository<T> where T : class
    {
        public DbContext _context;
        public DbSet<T> _dbset;

        public Repository(DbContext context)
        {
            this._context = context;
            _dbset = context.Set<T>();
        }

        public T Single(System.Linq.Expressions.Expression<Func<T, bool>> where)
        {
            return _dbset.Single(where);
        }

        public T FirstOrDefault(System.Linq.Expressions.Expression<Func<T, bool>> where)
        {
            return _dbset.FirstOrDefault(where);
        }

        public IQueryable<T> AsQueryable()
        {
            return _dbset;
        }

        public IEnumerable<T> GetAll()
        {
            return _dbset.ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbset.ToListAsync();
        }

        public IEnumerable<T> FindByQuery(System.Linq.Expressions.Expression<Func<T, bool>> where)
        {
            return _dbset.Where(where);
        }

        public IQueryable<T> Where(System.Linq.Expressions.Expression<Func<T, bool>> where)
        {
            return _dbset.Where(where);
        }

        public bool Any(System.Linq.Expressions.Expression<Func<T, bool>> where)
        {
            return _dbset.Any(where);
        }

        public void Add(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                this._dbset.Add(entity);
                this._context.SaveChanges();
            }
            catch (ValidationException dbEx)
            {
                var msg = dbEx.InnerException.Message;

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }
        
        public void AddRange(IEnumerable<T> entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                this._dbset.AddRange(entity);
                this._context.SaveChanges();
            }
            catch (ValidationException dbEx)
            {
                var msg = dbEx.InnerException.Message;

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }


        public void Update(T entity, params Expression<Func<T, object>>[] updatedProperties)
        {

            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }

                this._context.Set<T>().Update(entity);

                this._context.SaveChanges();

                //Ensure only modified fields are updated.
                //var dbEntityEntry = _context.Entry(entity);
                //_context.Entry(entity).State = EntityState.Modified;

                //if (updatedProperties.Any())
                //{
                //    //update explicitly mentioned properties
                //    foreach (var property in updatedProperties)
                //    {
                //        dbEntityEntry.Property(property).IsModified = true;
                //    }
                //}
                //else {
                //    //no items mentioned, so find out the updated entries
                //    foreach (var property in dbEntityEntry.OriginalValues.PropertyNames)
                //    {
                //        var original = dbEntityEntry.OriginalValues.GetValue<object>(property);
                //        var current = dbEntityEntry.CurrentValues.GetValue<object>(property);
                //        if (original != null && !original.Equals(current))
                //            dbEntityEntry.Property(property).IsModified = true;
                //    }
                //}

                //this._context.SaveChanges();
            }
            catch (ValidationException dbEx)
            {
                var msg = dbEx.InnerException.Message;

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        public void Update(List<T> entities, params Expression<Func<T, object>>[] updatedProperties)
        {

            try
            {
                if (entities == null)
                {
                    throw new ArgumentNullException("entity");
                }

                this._context.Set<T>().UpdateRange(entities);

                this._context.SaveChanges();
            }
            catch (ValidationException dbEx)
            {
                var msg = dbEx.InnerException.Message;

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        public void Delete(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                this._dbset.Remove(entity);
                this._context.SaveChanges();
            }
            catch (ValidationException dbEx)
            {
                var msg = dbEx.InnerException.Message;

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        public void Delete(IEnumerable<T> entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                this._dbset.RemoveRange(entity);
                this._context.SaveChanges();
            }
            catch (ValidationException dbEx)
            {
                var msg = dbEx.InnerException.Message;

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        public async Task<int> UpdateAsync(T entity, params Expression<Func<T, object>>[] updatedProperties)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }

                this._context.Set<T>().Update(entity);

                int count = await this._context.SaveChangesAsync();
                return count;
            }
            catch (ValidationException dbEx)
            {
                var msg = dbEx.InnerException.Message;

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        public async Task<int> UpdateAsync(List<T> entities, params Expression<Func<T, object>>[] updatedProperties)
        {
            try
            {
                if (entities == null)
                {
                    throw new ArgumentNullException("entity");
                }

                this._context.Set<T>().UpdateRange(entities.ToArray());

                int count = await this._context.SaveChangesAsync();
                return count;
            }
            catch (ValidationException dbEx)
            {
                var msg = dbEx.InnerException.Message;

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }
        public async Task<int> AddAsync(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                this._dbset.Add(entity);
                int count = await this._context.SaveChangesAsync();
                return count;
            }
            catch (Exception dbEx)
            {
                var msg = dbEx.InnerException.Message;

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        public async Task<int> AddRangeAsync(IEnumerable<T> entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                this._dbset.AddRange(entity);
                int count = await this._context.SaveChangesAsync();
                return count;
            }
            catch (ValidationException dbEx)
            {
                var msg = dbEx.InnerException.Message;

                var fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

    }
}
