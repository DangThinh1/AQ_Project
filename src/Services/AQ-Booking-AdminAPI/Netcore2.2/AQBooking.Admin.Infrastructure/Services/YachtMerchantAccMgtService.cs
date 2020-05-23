using AQBooking.Admin.Core.Enums;
using AQBooking.Admin.Core.Models.YachtMerchantAccountMgt;
using AQBooking.Admin.Core.Paging;
using AQBooking.Admin.Infrastructure.Databases.YachtEntities;
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
    public class YachtMerchantAccMgtService : ServiceBase, IYachtMerchantAccMgtService
    {
        #region Fields
        #endregion Fields

        #region Ctor

        public YachtMerchantAccMgtService(
            AQYachtContext aqYachtContext,
            IWorkContext workContext,
            IMapper mapper) : base(aqYachtContext, workContext, mapper)
        {
        }

        #endregion Ctor

        #region Methods

        public IPagedList<YachtMerchantAccMgtViewModel> SearchYachtMerchantAccMgt(YachtMerchantAccMgtSearchModel model)
        {
            try
            {
                var userGuid = _workContext.UserGuid;
                var userRoleId = _workContext.UserRoleId;
                var sortString = !string.IsNullOrEmpty(model.SortString) ? model.SortString : "CreatedDate DESC";
                var query = (from ymamgt in _dbYachtContext.YachtMerchantAqmgts.AsNoTracking()
                         join ym in _dbYachtContext.YachtMerchants.AsNoTracking() on ymamgt.MerchantFid equals ym.Id
                         where ymamgt.Deleted == false && ym.Deleted == false
                         && (userRoleId == (int)UserRoleEnum.YachtMerchantManager ? ymamgt.AqadminUserFid == userGuid : true)
                         && (string.IsNullOrEmpty(model.UserFid) || ymamgt.AqadminUserFid.ToString().Contains(model.UserFid))
                         && (model.MerchantFid == 0 || ymamgt.MerchantFid == model.MerchantFid)
                         && (string.IsNullOrEmpty(model.EffectiveStartDate) || Convert.ToDateTime(model.EffectiveStartDate) <= ymamgt.EffectiveStartDate)
                         && (string.IsNullOrEmpty(model.EffectiveEndDate) || Convert.ToDateTime(model.EffectiveEndDate) >= ymamgt.EffectiveEndDate)
                         select _mapper.Map<YachtMerchantAccMgtViewModel>(ymamgt)).OrderBy(sortString).AsQueryable();

                return new PagedList<YachtMerchantAccMgtViewModel>(query, model.PageIndex, model.PageSize);
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> CreateYachtMerchantAccMgt(YachtMerchantAccMgtCreateModel model)
        {
            try
            {
                var userGuid = _workContext.UserGuid;
                var entity = _mapper.Map<YachtMerchantAqmgts>(model);

                entity.CreatedBy = userGuid;
                entity.CreatedDate = DateTime.Now;
                entity.LastModifiedBy = userGuid;
                entity.LastModifiedDate = DateTime.Now;
                await _dbYachtContext.YachtMerchantAqmgts.AddAsync(entity);
                var flag = await _dbYachtContext.SaveChangesAsync();
                if (flag != 0)
                    return true;
                return false;
            }
            catch
            {
                throw;
            }
        }

        public YachtMerchantAccMgtViewModel FindYachtMerchantAccMgtById(int id)
        {
            try
            {
                var entity = _dbYachtContext.YachtMerchantAqmgts.AsNoTracking().Where(x => x.Id == id && x.Deleted == false).SingleOrDefault();
                if (entity == null)
                    return new YachtMerchantAccMgtViewModel();
                var result = _mapper.Map<YachtMerchantAccMgtViewModel>(entity);
                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> UpdateYachtMerchantAccMgt(YachtMerchantAccMgtCreateModel model)
        {
            try
            {
                var entity = _dbYachtContext.YachtMerchantAqmgts.AsNoTracking().Where(x => x.Id == model.Id && x.Deleted == false).SingleOrDefault();
                entity = _mapper.Map<YachtMerchantAccMgtCreateModel, YachtMerchantAqmgts>(model, entity);

                entity.LastModifiedBy = _workContext.UserGuid;
                entity.LastModifiedDate = DateTime.Now;
                _dbYachtContext.YachtMerchantAqmgts.Update(entity);
                var flag = await _dbYachtContext.SaveChangesAsync();
                if (flag != 0)
                    return true;
                return false;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> DeleteYachtMerchantAccMgt(int id)
        {
            try
            {
                var entity = _dbYachtContext.YachtMerchantAqmgts.AsNoTracking().Where(x => x.Id == id && x.Deleted == false).SingleOrDefault();
                if (entity == null)
                    return false;
                entity.Deleted = true;
                entity.LastModifiedBy = _workContext.UserGuid;
                entity.LastModifiedDate = DateTime.Now;
                _dbYachtContext.YachtMerchantAqmgts.Update(entity);
                var flag = await _dbYachtContext.SaveChangesAsync();
                if (flag != 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> RemoveYachtMerchantAccMgt(int id)
        {
            try
            {
                var entity = _dbYachtContext.YachtMerchantAqmgts.AsNoTracking().Where(x => x.Id == id && x.Deleted == false).SingleOrDefault();
                if (entity == null)
                    return false;
                _dbYachtContext.YachtMerchantAqmgts.Remove(entity);
                var flag = await _dbYachtContext.SaveChangesAsync();
                if (flag != 0)
                    return true;
                return false;
            }
            catch
            {
                throw;
            }
        }

        public YachtMerchantAccMgtViewModel GetYachtMerchantAccMgtByMerchantId(int merchantId)
        {
            try
            {
                var result = _dbYachtContext.YachtMerchantAqmgts.AsNoTracking()
                    .Where(x => x.MerchantFid == merchantId && x.Deleted == false && x.EffectiveStartDate <= DateTime.Now && x.EffectiveEndDate >= DateTime.Now)
                    .Select(x => new YachtMerchantAccMgtViewModel
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