using AQBooking.Admin.Core.Enums;
using AQBooking.Admin.Core.Models.YachtMerchantAccount;
using AQBooking.Admin.Core.Paging;
using AQBooking.Admin.Infrastructure.Databases.YachtEntities;
using AQBooking.Admin.Infrastructure.Helpers;
using AQBooking.Admin.Infrastructure.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

namespace AQBooking.Admin.Infrastructure.Services
{
    public class YachtMerchantAccService : ServiceBase, IYachtMerchantAccService
    {
        #region Fields
        

        #endregion Fields

        #region Ctor

        public YachtMerchantAccService(AQYachtContext dbContext,
            IWorkContext workContext,
           IMapper mapper) : base(dbContext, workContext, mapper)
        {
        }

        #endregion Ctor

        #region Methods

        public IPagedList<YachtMerchantAccViewModel> SearchYachtMerchantAcc(YachtMerchantAccSearchModel model)
        {
            try
            {
                var sortString = !string.IsNullOrEmpty(model.SortString) ? model.SortString : "CreatedDate DESC";
                var userId = _workContext.UserGuid;
                var userRoleId = _workContext.UserRoleId;
                IQueryable<YachtMerchantAccViewModel> query;
                if (userRoleId == (int)UserRoleEnum.YachtMerchantManager)
                {
                    query = (from ymu in _dbYachtContext.YachtMerchantUsers
                             join ymgt in _dbYachtContext.YachtMerchantAqmgts on ymu.MerchantFid equals ymgt.MerchantFid
                             where ymu.Deleted == false && ymgt.Deleted == false && ymgt.AqadminUserFid.Equals(userId)
                             && (model.MerchantFid == 0 || ymu.MerchantFid == model.MerchantFid)
                             && (string.IsNullOrEmpty(model.UserFid) || ymu.UserFid.ToString().Contains(model.UserFid))
                             && (string.IsNullOrEmpty(model.EffectiveStartDate) || Convert.ToDateTime(model.EffectiveStartDate) <= ymu.EffectiveStartDate)
                             && (string.IsNullOrEmpty(model.EffectiveEndDate) || Convert.ToDateTime(model.EffectiveEndDate) >= ymu.EffectiveEndDate)
                             select _mapper.Map<YachtMerchantAccViewModel>(ymu)).OrderBy(sortString).AsQueryable();
                }
                else
                {
                    query = (from ymu in _dbYachtContext.YachtMerchantUsers
                             join ym in _dbYachtContext.YachtMerchants on ymu.MerchantFid equals ym.Id
                             where ymu.Deleted == false && ym.Deleted == false
                             && (model.MerchantFid == 0 || ymu.MerchantFid == model.MerchantFid)
                             && (string.IsNullOrEmpty(model.UserFid) || ymu.UserFid.ToString().Contains(model.UserFid))
                             && (string.IsNullOrEmpty(model.EffectiveStartDate) || Convert.ToDateTime(model.EffectiveStartDate) <= ymu.EffectiveStartDate)
                             && (string.IsNullOrEmpty(model.EffectiveEndDate) || Convert.ToDateTime(model.EffectiveEndDate) >= ymu.EffectiveEndDate)
                             select _mapper.Map<YachtMerchantAccViewModel>(ymu)).OrderBy(sortString).AsQueryable();
                }
                return new PagedList<YachtMerchantAccViewModel>(query, model.PageIndex, model.PageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CreateYachtMerchantAcc(YachtMerchantAccCreateModel model)
        {
            try
            {
                var userGuid = _workContext.UserGuid;
                var entity = _mapper.Map<YachtMerchantUsers>(model);
                entity.MerchantName = model.MerchantName;
                entity.UserName = model.UserName;
                entity.CreatedBy = userGuid;
                entity.CreatedDate = DateTime.Now;
                entity.LastModifiedBy = userGuid;
                entity.LastModifiedDate = DateTime.Now;
                _dbYachtContext.YachtMerchantUsers.AddAsync(entity);
                _dbYachtContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public YachtMerchantAccViewModel FindYachtMerchantAccById(int id)
        {
            try
            {
                var entity = _dbYachtContext.YachtMerchantUsers.Where(x => x.Id == id && x.Deleted == false).SingleOrDefault();
                if (entity == null)
                    return new YachtMerchantAccViewModel();
                var result = _mapper.Map<YachtMerchantAccViewModel>(entity);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateYachtMerchantAcc(YachtMerchantAccCreateModel model)
        {
            try
            {
                var entity = _dbYachtContext.YachtMerchantUsers.Where(x => x.Id == model.Id && x.Deleted == false).SingleOrDefault();
                entity = _mapper.Map<YachtMerchantAccCreateModel, YachtMerchantUsers>(model, entity);

                entity.LastModifiedBy = _workContext.UserGuid;
                entity.LastModifiedDate = DateTime.Now;
                _dbYachtContext.YachtMerchantUsers.Update(entity);
                _dbYachtContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteYachtMerchantAcc(int id)
        {
            try
            {
                var entity = _dbYachtContext.YachtMerchantUsers.Where(x => x.Id == id && x.Deleted == false).SingleOrDefault();
                if (entity == null)
                    throw new Exception("Yacht Merchant User not found!");
                entity.Deleted = true;
                entity.LastModifiedBy = _workContext.UserGuid;
                entity.LastModifiedDate = DateTime.Now;
                _dbYachtContext.YachtMerchantUsers.Update(entity);
                _dbYachtContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool RemoveYachtMerchantAcc(int id)
        {
            try
            {
                var entity = _dbYachtContext.YachtMerchantUsers.Where(x => x.Id == id && x.Deleted == false).SingleOrDefault();
                if (entity == null)
                    throw new Exception("Yacht Merchant User not found!");
                _dbYachtContext.YachtMerchantUsers.Remove(entity);
                _dbYachtContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public YachtMerchantAccViewModel GetYachtMerchantAccByMerchatnId(int merchantId)
        {
            try
            {
                var result = _dbYachtContext.YachtMerchantUsers
                    .Where(x => x.MerchantFid == merchantId && x.Deleted == false && x.EffectiveStartDate <= DateTime.Now && x.EffectiveEndDate >= DateTime.Now)
                    .Select(x => new YachtMerchantAccViewModel
                    {
                        Id = x.Id,
                        UserName = x.UserName,
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