using AccommodationMerchant.Core.Models.HotelMerchants;
using AccommodationMerchant.Infrastructure.Databases;
using AccommodationMerchant.Infrastructure.Services.Interfaces;
using APIHelpers.Response;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccommodationMerchant.Infrastructure.Services.Implements
{
    public class HotelMerchantService : ServiceBase, IHotelMerchantService
    {
        public HotelMerchantService(AccommodationContext db, IMapper mapper) : base(db, mapper)
        {
        }

        /// <summary>
        /// Get UniqueId of Merchant
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<string>> GetMerchantUniqueID(int id)
        {
            var entity = await _db.HotelMerchants.FindAsync(id);
            if (entity != null)
                return BaseResponse<string>.Success(entity.UniqueId);
            return BaseResponse<string>.NotFound(string.Empty);
        }

        /// <summary>
        /// Get basic infomation of Merchant base on Merchant Id input
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        public BaseResponse<HotelMerchantBasicInfoModel> GetMerchantBasicInfoByMerchantId(int merchantId)
        {
            if (merchantId > 0)
            {
                var entity = _db.HotelMerchants.AsNoTracking().FirstOrDefault(k => k.Id == merchantId && k.Deleted == false);
                if (entity != null)
                    return BaseResponse<HotelMerchantBasicInfoModel>.Success(new HotelMerchantBasicInfoModel() { MerchantId = entity.Id, MerchantUniqueId = entity.UniqueId, MerchantName = entity.MerchantName, Country = entity.Country});
                return BaseResponse<HotelMerchantBasicInfoModel>.Success(new HotelMerchantBasicInfoModel() { MerchantId = merchantId, MerchantUniqueId = string.Empty, MerchantName = string.Empty, Country=string.Empty });
            }
            return BaseResponse<HotelMerchantBasicInfoModel>.Success(new HotelMerchantBasicInfoModel() { MerchantId = merchantId, MerchantUniqueId = string.Empty, MerchantName = string.Empty, Country = string.Empty });
        }


        /// <summary>
        /// Get all merchant of user have role Accommodation Account Manager ( AAM ) with UserId
        /// </summary>
        /// <returns></returns>
        public BaseResponse<List<Merchant>> GetListMerchantOfAccommodationAccountManager(string userId)
        {
            if (!String.IsNullOrEmpty(userId))
            {
                var query = (from m in _db.HotelMerchantAqmgts.AsNoTracking().Where(m => m.Deleted == false && m.AqadminUserFid.ToString().ToLower() == userId.ToLower())
                             join r in _db.HotelMerchants.AsNoTracking().Where(x => x.Deleted == false) on m.MerchantFid equals r.Id
                             select new Merchant()
                             {
                                 Value = m.MerchantFid.ToString(),
                                 Text = r.MerchantName
                             });
                if (query.Count() > 0)
                    return BaseResponse<List<Merchant>>.Success(query.ToList());
                return BaseResponse<List<Merchant>>.Success(new List<Merchant>());
            }

            return BaseResponse<List<Merchant>>.Success(new List<Merchant>());
        }

        /// <summary>
        /// Get Merchant of user have role Accommodation Merchant ( AM )
        /// </summary>
        /// <returns></returns>
        public BaseResponse<Merchant> GetMerchantOfUserRoleAccommodationMerchant(string userId)
        {
            if (!String.IsNullOrEmpty(userId))
            {
                var query = (from m in _db.HotelMerchantUsers.AsNoTracking().Where(m => m.Deleted == false && m.UserFid.ToString().ToLower() == userId.ToLower())

                             join r in _db.HotelMerchants.AsNoTracking().Where(m => m.Deleted == false) on m.MerchantFid equals r.Id
                             select new Merchant()
                             {
                                 Value = m.MerchantFid.ToString(),
                                 Text = r.MerchantName
                             });
                if (query != null)
                    return BaseResponse<Merchant>.Success(query.FirstOrDefault());
                return BaseResponse<Merchant>.Success(new Merchant());
            }
            else
                return BaseResponse<Merchant>.Success(new Merchant());
        }

        public BaseResponse<List<SelectListItem>> GetListHotelOfMerchant(int merchantId)
        {
            var query = (from r in _db.Hotels.AsNoTracking().Where(r => r.Deleted == false && r.MerchantFid == merchantId)
                         select new SelectListItem()
                         {
                             Value = r.Id.ToString(),
                             Text = r.HotelName
                         }).Take(1); //  One Merchant only get 1 Hotel
            if (query.Count() > 0)
                return BaseResponse<List<SelectListItem>>.Success(query.ToList());
            return BaseResponse<List<SelectListItem>>.Success(new List<SelectListItem>());
        }

        
        public async Task<BaseResponse<int>> GetMerchantIdByHotelId(int hotelId)
        {
            var entity = await _db.Hotels.FindAsync(hotelId);
            if (entity != null)
                return BaseResponse<int>.Success(entity.MerchantFid);
            return BaseResponse<int>.NotFound();
        }

        
    }
}