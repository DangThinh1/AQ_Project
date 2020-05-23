using AQBooking.Admin.Core.Enums;
using AQBooking.Admin.Core.Models.RestaurantMerchantAccount;
using AQBooking.Admin.Core.Paging;
using AQBooking.Admin.Infrastructure.Databases.DiningEntities;
using AQBooking.Admin.Infrastructure.Helpers;
using AQBooking.Admin.Infrastructure.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

namespace AQBooking.Admin.Infrastructure.Services
{
    public class RestaurantMerchantAccService : ServiceBase, IRestaurantMerchantAccService
    {
        #region Fields
        #endregion Fields

        #region Ctor

        public RestaurantMerchantAccService(
            AQDiningContext dbContext,
            IWorkContext workContext,
            IMapper mapper) : base(dbContext, workContext, mapper)
        {
        }

        #endregion Ctor

        #region Methods

        public IPagedList<RestaurantMerchantAccViewModel> SearchRestaurantMerchantAcc(RestaurantMerchantAccSearchModel model)
        {
            var sortString = !string.IsNullOrEmpty(model.SortString) ? model.SortString : "CreatedDate DESC";
            var userId = _workContext.UserGuid;
            var userRoleId = _workContext.UserRoleId;
            IQueryable<RestaurantMerchantAccViewModel> query;
            if (userRoleId == (int)UserRoleEnum.DiningMerchantManager)
            {
                query = (from rmu in _dbDiningContext.RestaurantMerchantUsers
                         join rmgt in _dbDiningContext.RestaurantMerchantAqmgts on rmu.MerchantFid equals rmgt.MerchantFid
                         where rmu.Deleted == false && rmgt.Deleted==false && rmgt.AqadminUserFid.Equals(userId)
                         && (model.MerchantFid == 0 || rmu.MerchantFid == model.MerchantFid)
                         && (string.IsNullOrEmpty(model.UserFid) || rmu.UserFid.ToString().Contains(model.UserFid))
                         && (string.IsNullOrEmpty(model.EffectiveStartDate) || Convert.ToDateTime(model.EffectiveStartDate) <= rmu.EffectiveStartDate)
                         && (string.IsNullOrEmpty(model.EffectiveEndDate) || Convert.ToDateTime(model.EffectiveEndDate) >= rmu.EffectiveEndDate)
                         select _mapper.Map<RestaurantMerchantAccViewModel>(rmu)).OrderBy(sortString).AsQueryable();
            }
            else
            {
                query = (from rmu in _dbDiningContext.RestaurantMerchantUsers
                         join rm in _dbDiningContext.RestaurantMerchants on rmu.MerchantFid equals rm.Id
                         where rmu.Deleted == false && rm.Deleted==false
                         && (model.MerchantFid == 0 || rmu.MerchantFid == model.MerchantFid)
                         && (string.IsNullOrEmpty(model.UserFid) || rmu.UserFid.ToString().Contains(model.UserFid))
                         && (string.IsNullOrEmpty(model.EffectiveStartDate) || Convert.ToDateTime(model.EffectiveStartDate) <= rmu.EffectiveStartDate)
                         && (string.IsNullOrEmpty(model.EffectiveEndDate) || Convert.ToDateTime(model.EffectiveEndDate) >= rmu.EffectiveEndDate)
                         select _mapper.Map<RestaurantMerchantAccViewModel>(rmu)).OrderBy(sortString).AsQueryable();
            }

            return new PagedList<RestaurantMerchantAccViewModel>(query, model.PageIndex, model.PageSize);
        }

        public async Task<bool> CreateRestaurantMerchantAcc(RestaurantMerchantAccCreateModel model)
        {
            try
            {
                var userGuid = _workContext.UserGuid;
                var entity = _mapper.Map<RestaurantMerchantUsers>(model);
                entity.MerchantName = model.MerchantName;
                entity.UserName = model.UserName;
                entity.CreatedBy = userGuid;
                entity.CreatedDate = DateTime.Now;
                entity.LastModifiedBy = userGuid;
                entity.LastModifiedDate = DateTime.Now;
                await _dbDiningContext.RestaurantMerchantUsers.AddAsync(entity);
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

        public RestaurantMerchantAccViewModel FindRestaurantMerchantAccById(int id)
        {
            try
            {
                var entity = _dbDiningContext.RestaurantMerchantUsers.AsNoTracking().Where(x => x.Id == id && x.Deleted == false).SingleOrDefault();
                if (entity == null)
                    return new RestaurantMerchantAccViewModel();
                var result = _mapper.Map<RestaurantMerchantAccViewModel>(entity);
                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> UpdateRestaurantMerchantAcc(RestaurantMerchantAccCreateModel model)
        {
            try
            {
                var entity = _dbDiningContext.RestaurantMerchantUsers.AsNoTracking().Where(x => x.Id == model.Id && x.Deleted == false).SingleOrDefault();
                entity = _mapper.Map<RestaurantMerchantAccCreateModel, RestaurantMerchantUsers>(model, entity);

                entity.LastModifiedBy = _workContext.UserGuid;
                entity.LastModifiedDate = DateTime.Now;
                _dbDiningContext.RestaurantMerchantUsers.Update(entity);
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

        public async Task<bool> DeleteRestaurantMerchantAcc(int id)
        {
            try
            {
                var entity = await Task.Run(() => _dbDiningContext.RestaurantMerchantUsers.AsNoTracking().Where(x => x.Id == id && x.Deleted == false).SingleOrDefault());
                if (entity == null)
                    return false;
                entity.Deleted = true;
                entity.LastModifiedBy = _workContext.UserGuid;
                entity.LastModifiedDate = DateTime.Now;
                _dbDiningContext.RestaurantMerchantUsers.Update(entity);
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

        public async Task<bool> RemoveRestaurantMerchantAcc(int id)
        {
            try
            {
                var entity = _dbDiningContext.RestaurantMerchantUsers.AsNoTracking().Where(x => x.Id == id && x.Deleted == false).SingleOrDefault();
                if (entity == null)
                    throw new Exception("Restaurant Merchant User not found!");
                _dbDiningContext.RestaurantMerchantUsers.Remove(entity);
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

        public RestaurantMerchantAccViewModel GetRestaurantMerchantAccByMerchatnId(int merchantId)
        {
            try
            {
                var result = _dbDiningContext.RestaurantMerchantUsers.AsNoTracking()
                    .Where(x => x.MerchantFid == merchantId && x.Deleted == false && x.EffectiveStartDate <= DateTime.Now && x.EffectiveEndDate >= DateTime.Now)
                    .Select(x => new RestaurantMerchantAccViewModel
                    {
                        Id = x.Id,
                        MerchantName = x.MerchantName,
                        UserFid = x.UserFid.ToString(),
                        EffectiveStartDate = x.EffectiveStartDate,
                        EffectiveEndDate = x.EffectiveEndDate,
                        CreatedBy = x.CreatedBy,
                        CreatedDate = x.CreatedDate
                    }).SingleOrDefault();
                return result;
            }
            catch
            {
                throw;
            }
        }

        #endregion Methods
    }
}