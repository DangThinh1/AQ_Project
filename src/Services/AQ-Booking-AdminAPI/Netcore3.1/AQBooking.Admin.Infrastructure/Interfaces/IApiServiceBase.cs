using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Paging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQBooking.Admin.Infrastructure.Services
{
    public interface IApiServiceBase<TViewModel, TCreateModel, TUpdateModel, TEntity>
         where TViewModel : class, new()
         where TCreateModel : class, new()
         where TUpdateModel : class, new()
         where TEntity : class, new()
    {
        void InitController(ControllerBase controller);

        Task<List<TViewModel>> GetAllAsync(bool includeDeleted = false);
        Task<IQueryable<TEntity>> GetAllQueryableAsync(bool includeDeleted = false);

        Task<(List<TViewModel> List, PagingInfo PagingInfo)> SearchAsync(SearchModel searchModel);
        Task<TViewModel> FindByIdAsync(int id, bool includeDeleted = false);
        Task<ApiActionResult> CreateAsync(TCreateModel model);
        Task<ApiActionResult> CreateRangeAsync(List<TCreateModel> models);
        Task<ApiActionResult> UpdateAsync(TUpdateModel restaurantMerchant);
        Task<ApiActionResult> DeleteAsync(int id);
    }
}
