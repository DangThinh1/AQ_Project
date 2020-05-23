using System;
using System.Linq;
using APIHelpers.Response;
using AQDiningPortal.Infrastructure.Interfaces;
using AQDiningPortal.Core.Models.RestaurantMerchants;
using System.Collections.Generic;
using AQDiningPortal.Infrastructure.Database;
using AQEncrypts;

namespace AQDiningPortal.Infrastructure.Services
{
    public class RestaurantMerchantService : IRestaurantMerchantService
    {
        private readonly DiningSearchContext _searchContext;
        public RestaurantMerchantService(DiningSearchContext searchContext)
        {
            _searchContext = searchContext;
        }

        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public BaseResponse<List<RestaurantMerchantViewModel>> GetResMerchantsByDisplayNumber(int DisplayNumber=0, int ImageType = 4)
        {
            List<RestaurantMerchantViewModel> merchantList = new List<RestaurantMerchantViewModel>();
            try
            {
                merchantList = (_searchContext.RestaurantMerchants
                    .Where(k => k.Deleted == false && DateTime.Now.Date <=k.ExpiredDate)
                    .OrderBy(x => Guid.NewGuid()).Take(DisplayNumber)
                    .Select(i=> new RestaurantMerchantViewModel
                    {
                         Id      = i.Id,
                        UniqueId    = i.UniqueId,
                        ZoneFid = i.ZoneFid,
                        MerchantName    = i.MerchantName,
                        Address1    = i.Address1,
                        Address2    = i.Address2,
                        Country = i.Country,
                        City    = i.City,
                        State   = i.State,
                        ZipCode = i.ZipCode,
                        ContactNumber1  = i.ContactNumber1,
                        ContactNumber2  = i.ContactNumber2,
                        EmailAddress1   = i.EmailAddress1,
                        EmailAddress2   = i.EmailAddress2,
                        AccountSize = i.AccountSize,
                        Remark  = i.Remark,
                        ExpiredDate = i.ExpiredDate,
                        FileStreamId=_searchContext.GetfnResMerchantImageIDVal(i.Id,4)
                    }
                    )).ToList();
                return BaseResponse<List<RestaurantMerchantViewModel>>.Success(merchantList);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<RestaurantMerchantViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public BaseResponse< RestaurantMerchantViewModel> GetResMerchantsById(string Id)
        {

            RestaurantMerchantViewModel merchant = new RestaurantMerchantViewModel();
            try
            {
                int merChantId = Convert.ToInt32(Terminator.Decrypt(Id));
                 
                merchant = _searchContext.RestaurantMerchants
                    .Where(k=>k.Id== merChantId && k.Deleted==false && DateTime.Now.Date<=k.ExpiredDate)
                    .Select(i=> new RestaurantMerchantViewModel {
                        Id=i.Id,
                        UniqueId= i.UniqueId,
                        ZoneFid=i.ZoneFid,
                        MerchantName=i.MerchantName,
                        Address1=i.Address1,
                        Address2=i.Address2,
                        Country=i.Country,
                        City=i.City,
                        State=i.State,
                        ZipCode=i.ZipCode,
                        ContactNumber1=i.ContactNumber1,
                        ContactNumber2=i.ContactNumber2,
                        EmailAddress1=i.EmailAddress1,
                        EmailAddress2=i.EmailAddress2,
                        AccountSize=i.AccountSize,
                        Remark=i.Remark,
                        ExpiredDate=i.ExpiredDate                       
                    }).FirstOrDefault();

                return BaseResponse<RestaurantMerchantViewModel>.Success(merchant);
                   
            }
            catch (Exception ex)
            {
                return BaseResponse<RestaurantMerchantViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}




