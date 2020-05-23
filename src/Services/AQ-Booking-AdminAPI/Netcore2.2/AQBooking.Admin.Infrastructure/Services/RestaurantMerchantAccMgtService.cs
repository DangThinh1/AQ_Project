using AQBooking.Admin.Core.Enums;
using AQBooking.Admin.Core.Models.RestaurantMerchantAccountMgt;
using AQBooking.Admin.Core.Paging;
using AQBooking.Admin.Infrastructure.Databases.DiningEntities;
using AQBooking.Admin.Infrastructure.Helpers;
using AQBooking.Admin.Infrastructure.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace AQBooking.Admin.Infrastructure.Services
{
    public class RestaurantMerchantAccMgtService : ServiceBase, IRestaurantMerchantAccMgtService
    {
        #region Fields
        #endregion Fields

        #region Ctor

        public RestaurantMerchantAccMgtService(
            AQDiningContext dbContext,
            IWorkContext workContext,
            IMapper mapper) : base(dbContext, workContext, mapper)
        {
        }

        #endregion Ctor

        #region Methods

        public IPagedList<RestaurantMerchantAccMgtViewModel> SearchRestaurantMerchantAccMgt(RestaurantMerchantAccMgtSearchModel model)
        {
            try
            {
                var userGuid = _workContext.UserGuid;
                var userRoleId = _workContext.UserRoleId;
                var sortString = !string.IsNullOrEmpty(model.SortString) ? model.SortString : "CreatedDate DESC";

                var query = (from rmamgt in _dbDiningContext.RestaurantMerchantAqmgts
                                 join rm in _dbDiningContext.RestaurantMerchants on rmamgt.MerchantFid equals rm.Id
                                 where rmamgt.Deleted == false && rm.Deleted == false
                                 && (userRoleId == (int)UserRoleEnum.DiningMerchantManager ? rmamgt.AqadminUserFid == userGuid : true)
                                 && (string.IsNullOrEmpty(model.UserFid) || rmamgt.AqadminUserFid.ToString().Contains(model.UserFid))
                                 && (model.MerchantFid == 0 || rmamgt.MerchantFid == model.MerchantFid)
                                 && (string.IsNullOrEmpty(model.EffectiveStartDate) || Convert.ToDateTime(model.EffectiveStartDate) <= rmamgt.EffectiveStartDate)
                                 && (string.IsNullOrEmpty(model.EffectiveEndDate) || Convert.ToDateTime(model.EffectiveEndDate) >= rmamgt.EffectiveEndDate)
                                 select _mapper.Map<RestaurantMerchantAccMgtViewModel>(rmamgt)).OrderBy(sortString).AsQueryable();

                return new PagedList<RestaurantMerchantAccMgtViewModel>(query, model.PageIndex, model.PageSize);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> CreateRestaurantMerchantAccMgt(RestaurantMerchantAccMgtCreateModel model)
        {
            try
            {
                var userGuid = _workContext.UserGuid;
                var entity = _mapper.Map<RestaurantMerchantAqmgts>(model);

                entity.CreatedBy = userGuid;
                entity.CreatedDate = DateTime.UtcNow;
                entity.LastModifiedBy = userGuid;
                entity.LastModifiedDate = DateTime.Now;
                await _dbDiningContext.RestaurantMerchantAqmgts.AddAsync(entity);
                var flag = await _dbDiningContext.SaveChangesAsync();
                if (flag != 0)
                    return true;
                return false;
            }
            catch
            {
                throw;
            }
        }

        public RestaurantMerchantAccMgtViewModel FindRestaurantMerchantAccMgtById(int id)
        {
            try
            {
                var entity = _dbDiningContext.RestaurantMerchantAqmgts.AsNoTracking().Where(x => x.Id == id && x.Deleted == false).SingleOrDefault();
                if (entity == null)
                    return new RestaurantMerchantAccMgtViewModel();
                var result = _mapper.Map<RestaurantMerchantAccMgtViewModel>(entity);
                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> UpdateRestaurantMerchantAccMgt(RestaurantMerchantAccMgtCreateModel model)
        {
            try
            {
                var entity = _dbDiningContext.RestaurantMerchantAqmgts.AsNoTracking().Where(x => x.Id == model.Id && x.Deleted == false).SingleOrDefault();
                entity = _mapper.Map<RestaurantMerchantAccMgtCreateModel, RestaurantMerchantAqmgts>(model, entity);
                if (entity == null)
                    throw new Exception("Restaurant Merchant Account Mgt not found!");
                entity.LastModifiedBy = _workContext.UserGuid;
                entity.LastModifiedDate = DateTime.Now;
                _dbDiningContext.RestaurantMerchantAqmgts.Update(entity);
                var flag = await _dbDiningContext.SaveChangesAsync();
                if (flag != 0)
                    return true;
                return false;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> DeleteRestaurantMerchantAccMgt(int id)
        {
            try
            {
                var entity = _dbDiningContext.RestaurantMerchantAqmgts.AsNoTracking().Where(x => x.Id == id && x.Deleted == false).SingleOrDefault();
                if (entity == null)
                    return false;
                entity.Deleted = true;
                entity.LastModifiedBy = _workContext.UserGuid;
                entity.LastModifiedDate = DateTime.Now;
                _dbDiningContext.RestaurantMerchantAqmgts.Update(entity);
                var flag = await _dbDiningContext.SaveChangesAsync();
                if (flag != 0)
                    return true;
                return false;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> RemoveRestaurantMerchantAccMgt(int id)
        {
            try
            {
                var entity = _dbDiningContext.RestaurantMerchantAqmgts.AsNoTracking().Where(x => x.Id == id && x.Deleted == false).SingleOrDefault();
                if (entity == null)
                    return false;

                _dbDiningContext.RestaurantMerchantAqmgts.Remove(entity);
                var flag = await _dbDiningContext.SaveChangesAsync();
                if (flag != 0)
                    return true;
                return false;
            }
            catch
            {
                throw;
            }
        }

        public RestaurantMerchantAccMgtViewModel GetRestaurantMerchantAccMgtByMerchantId(int merchantId)
        {
            try
            {
                var result = _dbDiningContext.RestaurantMerchantAqmgts.AsNoTracking()
                    .Where(x => x.MerchantFid == merchantId && x.Deleted == false && x.EffectiveStartDate <= DateTime.Now && x.EffectiveEndDate >= DateTime.Now)
                    .Select(x => new RestaurantMerchantAccMgtViewModel
                    {
                        Id = x.Id,
                        AqadminUserFid = x.AqadminUserFid.ToString(),
                        EffectiveStartDate = x.EffectiveStartDate,
                        EffectiveEndDate = x.EffectiveEndDate,
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate
                    }).SingleOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Methods
    }
}