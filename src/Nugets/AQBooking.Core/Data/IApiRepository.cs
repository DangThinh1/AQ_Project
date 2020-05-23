using AQBooking.Core.Models;
using AQBooking.Core.Paging;
using Omu.ValueInjecter.Injections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AQBooking.Repositories.Data
{
    public interface IApiRepository<T>
        where T : class
    {
        Task<(List<T> Data, PagingInfo PagingInfo)> SearchEntitiesAsync(SearchModel searchModel);

        Task<T> FindEntityById(object id, string idName = "Id");

        Task<ApiActionResult> CreateEntityAsync(T entity, object userId);

        Task<ApiActionResult> UpdateEntityAsync(T entity, object userId, LoopInjection ignoreProperties = null);

        Task<ApiActionResult> DeleteEntityAsync(object id, object userId);

        Task<ApiActionResult> DeleteEntitiesAsync(List<T> entities, object userId);
    }
}
