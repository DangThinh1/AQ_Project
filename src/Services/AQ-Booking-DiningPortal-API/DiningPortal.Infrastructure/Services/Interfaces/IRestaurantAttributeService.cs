using APIHelpers.Response;
using AQDiningPortal.Core.Models.RestaurantAttributes;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AQDiningPortal.Infrastructure.Interfaces
{
    public interface IRestaurantAttributeService
    {
        BaseResponse<List<RestaurantAttributeViewModel>> GetAllByCategoryFid(int categoryFid);
        BaseResponse<List<RestaurantAttributeViewModel>> GetAllByListCategoryFids(List<int> listCatagoryFids);
    }
}
