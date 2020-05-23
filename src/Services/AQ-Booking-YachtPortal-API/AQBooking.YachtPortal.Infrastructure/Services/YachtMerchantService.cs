using APIHelpers.Response;
using AQBooking.YachtPortal.Core.Models.YachtMerchants;
using AQBooking.YachtPortal.Infrastructure.Entities;
using AQBooking.YachtPortal.Infrastructure.Interfaces;
using AQEncrypts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Services
{
    public class YachtMerchantService : IYachtMerchantService
    {
        private readonly AQYachtContext _aqYachtContext;
        public YachtMerchantService(AQYachtContext aqYachtContext)
        {
            _aqYachtContext = aqYachtContext;
        }

        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public BaseResponse<List<YachtMerchantViewModel>> GetYachtMerchantsByDisplayNumber(int DisplayNumber = 0, int ImageType = 4)
        {
            List<YachtMerchantViewModel> merchantList = new List<YachtMerchantViewModel>();
            try
            {
                merchantList = (_aqYachtContext.YachtMerchants
                    .Where(k => k.Deleted == false && k.ExpiredDate >= DateTime.Now.Date)
                    .OrderBy(x => Guid.NewGuid()).Take(DisplayNumber)
                    .Select(i => new YachtMerchantViewModel
                    {
                        Id = i.Id,
                        UniqueId = i.UniqueId,
                        ZoneFid = i.ZoneFid,
                        MerchantName = i.MerchantName,
                        Address1 = i.Address1,
                        Address2 = i.Address2,
                        Country = i.Country,
                        City = i.City,
                        State = i.State,
                        ZipCode = i.ZipCode,
                        ContactNumber1 = i.ContactNumber1,
                        ContactNumber2 = i.ContactNumber2,
                        EmailAddress1 = i.EmailAddress1,
                        EmailAddress2 = i.EmailAddress2,
                        AccountSize = i.AccountSize,
                        Remark = i.Remark,
                        ExpiredDate = i.ExpiredDate,
                        FileStreamId = _aqYachtContext.GetfnYachtMerchantImageIDVal(i.Id, 4)
                    }
                    )).ToList();
                return BaseResponse<List<YachtMerchantViewModel>>.Success(merchantList);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtMerchantViewModel>>.InternalServerError(merchantList, message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public BaseResponse<YachtMerchantViewModel> GetYachtMerchantsById(string Id)
        {
            YachtMerchantViewModel merchant = new YachtMerchantViewModel();
            try
            {
                int merChantId = Terminator.Decrypt(Id) != "0" ? Convert.ToInt32(Terminator.Decrypt(Id)) : Int32.Parse(Id);

                merchant = _aqYachtContext.YachtMerchants
                    .Where(k => k.Id == merChantId && k.Deleted == false && k.ExpiredDate >= DateTime.Now.Date)
                    .Select(i => new YachtMerchantViewModel
                    {
                        Id = i.Id,
                        UniqueId = i.UniqueId,
                        ZoneFid = i.ZoneFid,
                        MerchantName = i.MerchantName,
                        Address1 = i.Address1,
                        Address2 = i.Address2,
                        Country = i.Country,
                        City = i.City,
                        State = i.State,
                        ZipCode = i.ZipCode,
                        ContactNumber1 = i.ContactNumber1,
                        ContactNumber2 = i.ContactNumber2,
                        EmailAddress1 = i.EmailAddress1,
                        EmailAddress2 = i.EmailAddress2,
                        AccountSize = i.AccountSize,
                        Remark = i.Remark,
                        ExpiredDate = i.ExpiredDate,
                        LandingPageOptionFid = i.LandingPageOptionFid,
                        //YachtFileStreamId = k.FileStreams.Where(c => c.YachtFid == k.Id && (c.FileTypeFid == 4 || c.FileTypeFid == 5) && c.Deleted == false && c.ActivatedDate <= DateTime.Now).OrderByDescending(d => d.ActivatedDate).FirstOrDefault().FileStreamFid,
                        MerchantFileStreamId = i.FileStreams.Where(c => c.MerchantFid == i.Id && c.Deleted == false && c.ActivatedDate <= DateTime.Now).OrderByDescending(d => d.ActivatedDate).FirstOrDefault().FileStreamFid,
                    }).FirstOrDefault();

                return BaseResponse<YachtMerchantViewModel>.Success(merchant);

            }
            catch (Exception ex)
            {
                return BaseResponse<YachtMerchantViewModel>.InternalServerError(merchant, message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public int GetMerchantLogoByMerchantId(int id)
        {
            var fileId = 0;
            var query = from ym in _aqYachtContext.YachtMerchants.AsNoTracking()
                        join ymf in _aqYachtContext.YachtMerchantFileStreams.AsNoTracking() on ym.Id equals ymf.MerchantFid
                        where ym.Deleted == false && ymf.Deleted == false && ym.Id == id && ymf.ActivatedDate <= DateTime.Now
                        orderby ymf.LastModifiedDate descending
                        select ymf.FileStreamFid;

            if (query.Count() > 0)
                fileId = query.FirstOrDefault();

            return fileId;
        }
    }
}
