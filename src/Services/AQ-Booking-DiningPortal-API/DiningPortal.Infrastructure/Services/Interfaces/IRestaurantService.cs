using APIHelpers.Response;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using AQDiningPortal.Core.Models.Restaurants;
using AQDiningPortal.Core.Models.PagingPortal;

namespace AQDiningPortal.Infrastructure.Interfaces
{
    public interface IRestaurantService
    {
        BaseResponse<RestaurantDetailViewModel> GetDetail(int id, int languageId = 1, DateTime? activatedDate = null);
        BaseResponse<PageListPortal<RestaurantSearchViewModel>> Search(RestaurantSearchModel searchModel);
        BaseResponse<List<SelectListItem>> GetComboBindingByCityAndZone(string City, string Zone);
        BaseResponse<List<RestaurantPartnerViewModel>> GetRestaurantPartners(int DisplayNumber = 4, int ImageType = 4);
        BaseResponse<PageListPortal<RestaurantSearchViewModel>> GetRestaurantsByMerchantFId(SearchRestaurantWithMerchantIdModel searchModel);
    }
}
