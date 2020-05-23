using APIHelpers.Response;
using AQDiningPortal.Core.Models.PagingPortal;
using AQDiningPortal.Core.Models.RestaurantMerchants;
using AQDiningPortal.Core.Models.Restaurants;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace AQDiningPortal.Infrastructure.Interfaces
{
    public interface IRestaurantMerchantService
    {
        BaseResponse<List<RestaurantMerchantViewModel>> GetResMerchantsByDisplayNumber(int DisplayNumber = 0, int ImageType = 4);
        BaseResponse<RestaurantMerchantViewModel> GetResMerchantsById(string Id);

    }
}
