using AQBooking.Core.Extentions;
using AQBooking.Core.Models;
using AQBooking.Repositories.Entities;
using Omu.ValueInjecter;
using Omu.ValueInjecter.Injections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQBooking.Repositories.Data
{
    public class ApiRepository<T> : Repository<T>, IApiRepository<T>
        where T : class
    {
        public ApiRepository(AQ_BookingsContext db) : base(db)
        {
        }

        public virtual async Task<ApiActionResult> CreateEntityAsync(T entity, object userId)
        {
            try
            {
                if (entity == null)
                    return await ApiActionResult.FailedAsync("Entity can not be null.");
                var trySetCreatedBy = entity.TrySetPropertyValueByPropertyName("CreatedBy", userId);
                var trySetCreatedDate = entity.TrySetPropertyValueByPropertyName("CreatedDate", DateTime.Now);
                if(!trySetCreatedBy)
                {
                    //=>Log
                }
                if (!trySetCreatedDate)
                {
                    //=>Log
                }

                await Entities.AddAsync(entity);
                var result = await SaveChangesAsync();
                if (result == 1)
                {
                    return await ApiActionResult.SuccessAsync();
                }

                return await ApiActionResult.FailedAsync("Create Entity Error.");
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public virtual async Task<ApiActionResult> DeleteEntitiesAsync(List<T> entities, object userId)
        {
            try
            {
                if (entities == null)
                    return await ApiActionResult.FailedAsync("Entities can not be null.");

                Entities.RemoveRange(entities);
                var result = await SaveChangesAsync();
                if (result == 1)
                {
                    return await ApiActionResult.SuccessAsync();
                }

                return await ApiActionResult.FailedAsync("Delete Entities Error.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual async Task<ApiActionResult> DeleteEntityAsync(object id, object userId)
        {
            try
            {
                if (id == null)
                    return await ApiActionResult.FailedAsync("Entity can not be null.");
                var entity = await FindEntityById(id);

                if(entity.HasProperty("Deleted", typeof(bool)))
                {
                    entity.TrySetPropertyValueByPropertyName("Deleted", true);
                    entity.TrySetPropertyValueByPropertyName("LastModifiedBy", userId);
                    entity.TrySetPropertyValueByPropertyName("LastModifiedDate", DateTime.Now);
                }
                else
                {
                    Entities.Remove(entity);
                    var result = await SaveChangesAsync();
                    if (result == 1)
                    {
                        return await ApiActionResult.SuccessAsync();
                    }
                }

                return await ApiActionResult.FailedAsync("Delete Entity Error.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual async Task<ApiActionResult> UpdateEntityAsync(T entity, object userId, LoopInjection ignoreProperties = null)
        {
            try
            {
                if (entity == null)
                    return await ApiActionResult.FailedAsync("Entity can not be null.");

                var entityDb = await Entities.FindAsync(entity);
                if(ignoreProperties == null)
                {
                    ignoreProperties = new LoopInjection(new[] { "CreatedBy", "CreatedDate", "UniqueId", "Id" });
                }
                entityDb.InjectFrom(ignoreProperties, entity);
                entityDb.TrySetPropertyValueByPropertyName("LastModifiedBy", userId);
                entityDb.TrySetPropertyValueByPropertyName("LastModifiedDate", DateTime.Now);

                var result = await SaveChangesAsync();
                if (result == 1)
                {
                    return await ApiActionResult.SuccessAsync();
                }

                return await ApiActionResult.FailedAsync("Update Entities Error.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual async Task<(List<T> Data, PagingInfo PagingInfo)> SearchEntitiesAsync(SearchModel searchModel)
        {
            try
            {
                var query = await Task.Run(() => {
                    return Entities
                              .Where(e => e.IsNotDeleted())
                              .Where(e => e.ContainsString(searchModel.SearchString, null))
                              .Where(e => e.CompareTo(searchModel));
                });

                if (searchModel.PageIndex > 0 && searchModel.PageSize > 0)
                {
                    return await Task.Run(() => {
                        var source = query.ToPageList(searchModel.PageIndex, searchModel.PageSize);
                        return (Data: source.List, PagingInfo: source.GetPagingInfo());
                    });
                }
                else
                {
                    return await Task.Run(() => {
                        var source = query.ToList();
                        return (Data: source, PagingInfo: new PagingInfo(source.Count, 1, source.Count, 1));
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual async Task<T> FindEntityById(object id, string idName = "Id")
        {
            try
            {
                var query = await Task.Run(() => {
                    return Entities
                              .Where(e => e.IsNotDeleted())
                              .Where(e => e.GetPropertyValueByPropertyName(idName).Equals(id));
                });

                return query.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
