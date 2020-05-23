using APIHelpers.Response;
using AQBooking.YachtPortal.Core.Models.YachtMerchantProductInventories;
using AQBooking.YachtPortal.Infrastructure.Entities;
using AQBooking.YachtPortal.Infrastructure.Interfaces;
using AQEncrypts;
using AutoMapper;
using ExtendedUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Services
{
    public class YachtMerchantProductInventoryService : IYachtMerchantProductInventoryService
    {
        private readonly AQYachtContext _aqYachtContext;
        private readonly IMapper _mapper;

        public YachtMerchantProductInventoryService(AQYachtContext aqYachtContext, IMapper mapper)
        {
            _aqYachtContext = aqYachtContext;
            _mapper = mapper;
        }

        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public BaseResponse<List<YachtMerchantProductInventoriesWithPriceViewModel>> GetPriceOfProductInventoryByArrayOfProductId(List<string> productId)
        {
            try
            {
                List<int> lstIntProuduceId = productId.Select(x => Terminator.Decrypt(x).ToInt32()).ToList();
                var result = (
                              from a in _aqYachtContext.YachtMerchantProductInventories

                              from productPricing in _aqYachtContext.YachtMerchantProductPricings
                               .Where(o => o.ProductFid == a.Id
                                && o.Deleted == false
                                && o.ProductFid == a.Id
                                && o.EffectiveDate <= DateTime.Now.Date
                                && (o.EffectiveEndDate == null || (o.EffectiveEndDate != null && o.EffectiveEndDate >= DateTime.Now.Date))
                                && o.EffectiveDate == _aqYachtContext.YachtMerchantProductPricings
                                                          .Where(x => x.ProductFid == a.Id
                                                            && x.EffectiveDate <= DateTime.Now.Date
                                                            && (x.EffectiveEndDate == null || (x.EffectiveEndDate != null && x.EffectiveEndDate >= DateTime.Now.Date))
                                                          ).OrderByDescending(e => e.EffectiveDate)
                                                          .Select(i => i.EffectiveDate).FirstOrDefault()
                                  ).DefaultIfEmpty().Take(1)

                              where
                              a.Deleted == false
                              // && a.IsActiveForSales == true
                              && lstIntProuduceId.Contains(a.Id)
                              select new YachtMerchantProductInventoriesWithPriceViewModel
                              {
                                  Id = Terminator.Encrypt(a.Id.ToString()),
                                  UniqueId = a.UniqueId,
                                  ProductCode = a.ProductCode,
                                  ProductName = a.ProductName,
                                  Price = productPricing == null ? 0 : productPricing.Price,
                                  CultureCode = productPricing == null ? "" : productPricing.CultureCode,
                                  CurrencyCode = productPricing == null ? "" : productPricing.CurrencyCode
                              }
                    ).ToList();

                if (result != null)
                    return BaseResponse<List<YachtMerchantProductInventoriesWithPriceViewModel>>.Success(result);
                else
                    return BaseResponse<List<YachtMerchantProductInventoriesWithPriceViewModel>>.NoContent();

            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtMerchantProductInventoriesWithPriceViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public BaseResponse<List<YachtMerchantProductInventoriesViewModel>> GetProductInventorByAdditionalFId(int additonalFId)
        {
            try
            {
                var result = (from a in _aqYachtContext.YachtAdditionalServiceDetails
                              join b in _aqYachtContext.YachtMerchantProductInventories
                              on a.ProductFid equals b.Id
                              where b.Deleted == false
                              && b.IsActiveForSales == true
                              && a.Deleted == false
                              && a.AdditionalServiceFid == additonalFId
                              && a.EffectiveDate <= DateTime.Now.Date
                             // && (a.EffectiveEndDate == null || (a.EffectiveEndDate != null && a.EffectiveEndDate >= DateTime.Now.Date))
                        
                              select _mapper.Map<YachtMerchantProductInventories, YachtMerchantProductInventoriesViewModel>(b)
                    );

                if (result != null)
                    return BaseResponse<List<YachtMerchantProductInventoriesViewModel>>.Success(result.ToList());
                else
                    return BaseResponse<List<YachtMerchantProductInventoriesViewModel>>.NoContent();

            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtMerchantProductInventoriesViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public BaseResponse<List<YachtMerchantProductInventoriesWithPriceViewModel>> GetProductInventoryPricingByAdditionalFId(string additonalFId)
        {
            try
            {
                var responseAddId = Terminator.Decrypt(additonalFId).ToInt32();
                var result = (from a in _aqYachtContext.YachtMerchantProductInventories

                              join su in _aqYachtContext.YachtAdditionalServiceDetails
                              on a.Id equals su.ProductFid


                              let productPricing = (_aqYachtContext.YachtMerchantProductPricings.OrderByDescending(x => x.EffectiveDate).FirstOrDefault(x =>
                                                      x.Deleted == false
                                                      && x.ProductFid == a.Id
                                                      && x.EffectiveDate <= DateTime.Now.Date
                                                      && (x.EffectiveEndDate == null || (x.EffectiveEndDate != null && x.EffectiveEndDate >= DateTime.Now.Date))
                                                  )
                                              )
                              where

                              a.Deleted == false
                              && a.IsActiveForSales == true
                               && su.Deleted == false
                                                      && su.AdditionalServiceFid == responseAddId
                                                      && su.EffectiveDate <= DateTime.Now.Date
                                                      && (su.EffectiveEndDate == null || (su.EffectiveEndDate != null && su.EffectiveEndDate >= DateTime.Now.Date))
                              select new YachtMerchantProductInventoriesWithPriceViewModel
                              {
                                  Id = Terminator.Encrypt(a.Id.ToString()),
                                  UniqueId = a.UniqueId,
                                  ProductCode = a.ProductCode,
                                  ProductName = a.ProductName,
                                  Price = productPricing == null ? 0 : productPricing.Price,
                                  CultureCode = productPricing == null ? "" : productPricing.CultureCode,
                                  CurrencyCode = productPricing == null ? "" : productPricing.CurrencyCode
                              }
                    ).ToList();

                if (result != null)
                    return BaseResponse<List<YachtMerchantProductInventoriesWithPriceViewModel>>.Success(result);
                else
                    return BaseResponse<List<YachtMerchantProductInventoriesWithPriceViewModel>>.NoContent();

            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtMerchantProductInventoriesWithPriceViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}
