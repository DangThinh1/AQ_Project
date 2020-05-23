using APIHelpers.Response;
using AQBooking.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YachtMerchant.Core.DTO;
using YachtMerchant.Core.Models.YachtMerchant;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Infrastructure.Services
{
    public class YachtMerchantService : ServiceBase, IYachtMerchantService
    {
        public YachtMerchantService(YachtOperatorDbContext dbcontext) : base(dbcontext)
        {
        }

        /// <summary>
        /// Get UniqueId of Merchant
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseResponse<string>> GetMerchantUniqueID(int id)
        {
            var entity = await _context.YachtMerchants.FindAsync(id);
            if (entity != null)
                return BaseResponse<string>.Success(entity.UniqueId);
            return BaseResponse<string>.NotFound(string.Empty);
        }

        /// <summary>
        /// Get basic infomation of Merchant base on Merchant Id input
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        public BaseResponse<YachtMerchantBasicInfoModel> GetMerchantBasicInfoByMerchantId(int merchantId)
        {
            if (merchantId > 0)
            {
                var entity = _context.YachtMerchants.AsNoTracking().FirstOrDefault(k => k.Id == merchantId && k.Deleted == false);
                if (entity != null)
                    return BaseResponse<YachtMerchantBasicInfoModel>.Success(new YachtMerchantBasicInfoModel() { MerchantId = entity.Id, MerchantUniqueId = entity.UniqueId, MerchantName = entity.MerchantName, Country = entity.Country});
                else
                    return BaseResponse<YachtMerchantBasicInfoModel>.Success(new YachtMerchantBasicInfoModel() { MerchantId = merchantId, MerchantUniqueId = string.Empty, MerchantName = string.Empty, Country=string.Empty });
            }
            return BaseResponse<YachtMerchantBasicInfoModel>.Success(new YachtMerchantBasicInfoModel() { MerchantId = merchantId, MerchantUniqueId = string.Empty, MerchantName = string.Empty, Country = string.Empty });
        }

        /// <summary>
        /// Get All Yacht Merchant
        /// </summary>
        /// <returns></returns>
        public BaseResponse<List<Merchant>> GetListYachtMerchants()
        {
            var query = (from c in _context.YachtMerchants.AsNoTracking().Where(k => k.Deleted == false)
                         select new Merchant()
                         {
                             Value = c.Id.ToString(),
                             Text = c.MerchantName
                         });
            if (query.Count() > 0)
                return BaseResponse<List<Merchant>>.Success(query.ToList());
            else
                return BaseResponse<List<Merchant>>.Success(new List<Merchant>());
        }

        /// <summary>
        /// Get All Merchant is Manager by Yacht Account Manager ( YAM )
        /// </summary>
        /// <returns></returns>
        public BaseResponse<List<Merchant>> GetListMerchantOfYachtAccountManager(string yamId)
        {
            if (!String.IsNullOrEmpty(yamId))
            {
                var query = (from m in _context.YachtMerchantAqmgts.AsNoTracking().Where(m => m.Deleted == false && m.AqadminUserFid.ToString().ToLower() == yamId.ToLower())
                             join r in _context.YachtMerchants.AsNoTracking().Where(x => x.Deleted == false) on m.MerchantFid equals r.Id
                             select new Merchant()
                             {
                                 Value = m.MerchantFid.ToString(),
                                 Text = r.MerchantName
                             });
                if (query.Count() > 0)
                    return BaseResponse<List<Merchant>>.Success(query.ToList());
                else
                    return BaseResponse<List<Merchant>>.Success(new List<Merchant>());
            }
            else
                return BaseResponse<List<Merchant>>.Success(new List<Merchant>());
        }

        /// <summary>
        /// Get Merchant of user have role Yacht Merchant ( YM )
        /// </summary>
        /// <returns></returns>
        public BaseResponse<Merchant> GetMerchantOfUserRoleYachtMerchant(string userId)
        {
            if (!String.IsNullOrEmpty(userId))
            {
                var query = (from m in _context.YachtMerchantUsers.AsNoTracking().Where(m => m.Deleted == false && m.UserFid.ToString().ToLower() == userId.ToLower())

                             join r in _context.YachtMerchants.AsNoTracking().Where(m => m.Deleted == false) on m.MerchantFid equals r.Id
                             select new Merchant()
                             {
                                 Value = m.MerchantFid.ToString(),
                                 Text = r.MerchantName
                             });
                if (query != null)
                    return BaseResponse<Merchant>.Success(query.FirstOrDefault());
                else
                    return BaseResponse<Merchant>.Success(new Merchant());
            }
            else
                return BaseResponse<Merchant>.Success(new Merchant());
        }

        public BaseResponse<List<DTODropdownItem>> GetListYachtOfMerchant(int merchantId)
        {
            var query = (from r in _context.Yachts.AsNoTracking().Where(r => r.Deleted == false && r.MerchantFid == merchantId)
                         select new DTODropdownItem()
                         {
                             Value = r.Id.ToString(),
                             Text = r.Name
                         });
            if (query.Count() > 0)
                return BaseResponse<List<DTODropdownItem>>.Success(query.ToList());
            else
                return BaseResponse<List<DTODropdownItem>>.Success(new List<DTODropdownItem>());
        }

        public BaseResponse<List<DTODropdownItem>> GetListYachtActiveForOperationOfMerchant(int merchantId)
        {
            var query = (from r in _context.Yachts.AsNoTracking().Where(r => r.Deleted == false && r.MerchantFid == merchantId && r.ActiveForOperation == true)
                         select new DTODropdownItem()
                         {
                             Value = r.Id.ToString(),
                             Text = r.Name
                         });
            if (query.Count() > 0)
                return BaseResponse<List<DTODropdownItem>>.Success(query.ToList());
            else
                return BaseResponse<List<DTODropdownItem>>.Success(new List<DTODropdownItem>());
        }

        public async Task<BaseResponse<int>> GetMerchantIdByYachtId(int yachtId)
        {
            var entity = await _context.Yachts.FindAsync(yachtId);
            if (entity != null)
                return BaseResponse<int>.Success(entity.MerchantFid);
            return BaseResponse<int>.NotFound();
        }

        public BaseResponse<bool> UpdateLandingPage(YachtMerchantViewModel model)
        {
            try
            {
                YachtMerchants obj = _context.YachtMerchants.AsNoTracking().FirstOrDefault(x => x.Id == model.Id);
                if (obj != null)
                {
                    obj.LandingPageOptionFid = model.LandingPageOptionFid;
                    _context.YachtMerchants.Update(obj);
                   _context.SaveChanges();
                    return BaseResponse<bool>.Success(true);
                    
                }
                return BaseResponse<bool>.NotFound(false);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}