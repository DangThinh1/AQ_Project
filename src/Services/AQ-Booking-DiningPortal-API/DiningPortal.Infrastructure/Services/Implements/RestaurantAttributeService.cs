using APIHelpers.Response;
using AQDiningPortal.Core.Models.RestaurantAttributes;
using AQDiningPortal.Infrastructure.Database;
using AQDiningPortal.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AQDiningPortal.Infrastructure.Services
{
    public class RestaurantAttributeService : IRestaurantAttributeService
    {
        private readonly AQDiningPortal.Infrastructure.Database.DiningSearchContext _searchContext;

        public RestaurantAttributeService(DiningSearchContext searchContext)
        {
            _searchContext = searchContext;
        }

        public BaseResponse<List<RestaurantAttributeViewModel>> GetAllByCategoryFid(int categoryFid)
        {
            try
            {
                var data = _searchContext.RestaurantAttributes
                            .Where(k => k.Deleted == false && k.AttributeCategoryFid.GetValueOrDefault() == categoryFid)
                            .Select(a=> new RestaurantAttributeViewModel()
                            { 
                                Id = a.Id,
                                UniqueId = a.UniqueId,
                                AttributeCategoryFid = a.AttributeCategoryFid,
                                AttributeName = a.AttributeName,
                                IconCssClass = a.IconCssClass,
                                ResourceKey = a.ResourceKey,
                                Remarks = a.Remarks,
                                IsDefault = a.IsDefault,
                                OrderBy = a.OrderBy
                            })
                            .OrderBy(a=>a.OrderBy)
                            .ToList();

                if(data == null)
                    return BaseResponse<List<RestaurantAttributeViewModel>>.NotFound();
                return BaseResponse<List<RestaurantAttributeViewModel>>.Success(data);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<RestaurantAttributeViewModel>>.InternalServerError(message:ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<RestaurantAttributeViewModel>> GetAllByListCategoryFids(List<int> listCatagoryFids)
        {
            try
            {
                var data = _searchContext.RestaurantAttributes
                            .Where(k => k.Deleted == false && listCatagoryFids.Contains(k.AttributeCategoryFid.GetValueOrDefault()))
                            .Select(a => new RestaurantAttributeViewModel()
                            {
                                Id= a.Id,
                                UniqueId = a.UniqueId,
                                AttributeCategoryFid = a.AttributeCategoryFid,
                                AttributeName = a.AttributeName,
                                IconCssClass = a.IconCssClass,
                                ResourceKey = a.ResourceKey,
                                Remarks = a.Remarks,
                                IsDefault = a.IsDefault,
                                OrderBy = a.OrderBy
                            })
                            .OrderBy(a => a.OrderBy)
                            .ToList();

                if (data == null)
                    return BaseResponse<List<RestaurantAttributeViewModel>>.NotFound();
                return BaseResponse<List<RestaurantAttributeViewModel>>.Success(data);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<RestaurantAttributeViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}
