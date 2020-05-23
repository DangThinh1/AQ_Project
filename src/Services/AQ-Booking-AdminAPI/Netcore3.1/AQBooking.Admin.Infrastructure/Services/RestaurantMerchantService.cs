using AQBooking.Admin.Core.Enums;
using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Models.RestaurantMerchant;
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
    public class RestaurantMerchantService : ServiceBase, IRestaurantMerchantService
    {
        #region Fields
        #endregion Fields

        #region Ctor

        public RestaurantMerchantService(
            AQDiningContext dbContext,
            IWorkContext workContext,
            IMapper mapper) : base(dbContext, workContext, mapper)
        {
        }

        #endregion Ctor

        #region Methods

        public IPagedList<RestaurantMerchantViewModel> SearchRestaurantMerchant(RestaurantMerchantSearchModel model)
        {
            var sortString = !string.IsNullOrEmpty(model.SortString) ? model.SortString : "CreatedDate DESC";
            var userId = _workContext.UserGuid;
            var userRoleId = _workContext.UserRoleId;
            IQueryable<RestaurantMerchantViewModel> query;
            if (userRoleId != (int)UserRoleEnum.DiningMerchantManager)
            {
                query = (from rm in _dbDiningContext.RestaurantMerchants
                         where rm.Deleted == false
                         && (string.IsNullOrEmpty(model.SearchString) || rm.MerchantName.Contains(model.SearchString) || rm.EmailAddress1.Contains(model.SearchString) || rm.EmailAddress2.Contains(model.SearchString) || rm.ContactNumber1.Contains(model.SearchString) || rm.ContactNumber2.Contains(model.SearchString))
                         && (string.IsNullOrEmpty(model.Country) || rm.Country.Contains(model.Country))
                         && (string.IsNullOrEmpty(model.City) || rm.City.Contains(model.City))
                         && (string.IsNullOrEmpty(model.State) || rm.State.Contains(model.State))
                         select _mapper.Map<RestaurantMerchantViewModel>(rm)).OrderBy(sortString).AsQueryable();
            }
            else
            {
                query = (from rm in _dbDiningContext.RestaurantMerchants
                         join dam in _dbDiningContext.RestaurantMerchantAqmgts on rm.Id equals dam.MerchantFid
                         where rm.Deleted == false && dam.Deleted == false
                         && dam.AqadminUserFid.Equals(userId)
                         && (string.IsNullOrEmpty(model.SearchString) || rm.MerchantName.Contains(model.SearchString) || rm.EmailAddress1.Contains(model.SearchString) || rm.EmailAddress2.Contains(model.SearchString) || rm.ContactNumber1.Contains(model.SearchString) || rm.ContactNumber2.Contains(model.SearchString))
                         && (string.IsNullOrEmpty(model.Country) || rm.Country.Contains(model.Country))
                         && (string.IsNullOrEmpty(model.City) || rm.City.Contains(model.City))
                         && (string.IsNullOrEmpty(model.State) || rm.State.Contains(model.State))
                         select _mapper.Map<RestaurantMerchantViewModel>(rm)).OrderBy(sortString).AsQueryable();
            }

            return new PagedList<RestaurantMerchantViewModel>(query, model.PageIndex, model.PageSize);
        }

        public async Task<bool> CreateRestaurantMerchant(RestaurantMerchantCreateModel model)
        {
            using (var tran = _dbDiningContext.Database.BeginTransaction())
            {
                try
                {
                    var userGuid = _workContext.UserGuid;
                    var userRoleId = _workContext.UserRoleId;
                    var entity = _mapper.Map<RestaurantMerchants>(model);

                    entity.UniqueId = UniqueIDHelper.GenerateRandomString(12, false);
                    entity.CreatedBy = userGuid;
                    entity.CreatedDate = DateTime.Now;
                    entity.LastModifiedBy = userGuid;
                    entity.LastModifiedDate = DateTime.Now;
                    await _dbDiningContext.RestaurantMerchants.AddAsync(entity);
                    await _dbDiningContext.SaveChangesAsync();

                    if (userRoleId == (int)UserRoleEnum.DiningMerchantManager)
                    {
                        var merchantAccountMgt = new RestaurantMerchantAqmgts
                        {
                            AqadminUserFid = userGuid,
                            MerchantFid = entity.Id,
                            MerchantName = entity.MerchantName,
                            CreatedBy = userGuid,
                            CreatedDate = DateTime.Now,
                            EffectiveStartDate = DateTime.Now,
                            EffectiveEndDate = DateTime.Now.AddYears(1),
                            Deleted = false,
                            LastModifiedBy = userGuid,
                            LastModifiedDate = DateTime.Now
                        };
                        await _dbDiningContext.RestaurantMerchantAqmgts.AddAsync(merchantAccountMgt);
                        await _dbDiningContext.SaveChangesAsync();
                    }

                    tran.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
        }

        public RestaurantMerchantViewModel FindRestaurantMerchantByIdAsync(int id)
        {
            try
            {
                var entity = _dbDiningContext.RestaurantMerchants.AsNoTracking().Where(x => x.Id == id && x.Deleted == false).SingleOrDefault();
                if (entity == null)
                    return new RestaurantMerchantViewModel();
                var result = _mapper.Map<RestaurantMerchantViewModel>(entity);
                return result;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> UpdateRestaurantMerchantAsync(RestaurantMerchantCreateModel model)
        {
            try
            {
                var entity = _dbDiningContext.RestaurantMerchants.AsNoTracking().Where(x => x.Id == model.Id && x.Deleted == false).SingleOrDefault();
                entity = _mapper.Map<RestaurantMerchantCreateModel, RestaurantMerchants>(model, entity);
                entity.LastModifiedBy = _workContext.UserGuid;
                entity.LastModifiedDate = DateTime.Now;
                _dbDiningContext.RestaurantMerchants.Update(entity);
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

        public async Task<bool> DeleteRestaurantMerchant(int id)
        {
            try
            {
                var entity = await Task.Run(() => _dbDiningContext.RestaurantMerchants.AsNoTracking().Where(x => x.Id == id && x.Deleted == false).SingleOrDefault());
                if (entity == null)
                    throw new Exception("Restaurant Merchant not found!");
                entity.Deleted = true;
                entity.LastModifiedBy = _workContext.UserGuid;
                entity.LastModifiedDate = DateTime.Now;
                _dbDiningContext.RestaurantMerchants.Update(entity);
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

        public async Task<bool> RemoveRestaurantMerchant(int id)
        {
            try
            {
                var entity = _dbDiningContext.RestaurantMerchants.AsNoTracking().Where(x => x.Id == id && x.Deleted == false).SingleOrDefault();
                if (entity == null)
                    return false;
                _dbDiningContext.RestaurantMerchants.Remove(entity);
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

        //Get list restaurant merchant don't have account
        public List<SelectListModel> GetRestaurantMerchantWithoutAccountSelectList()
        {
            try
            {
                var userId = _workContext.UserGuid;
                var userRoleId = _workContext.UserRoleId;
                List<SelectListModel> listMerchantResult = new List<SelectListModel>();
                var listMerchantHasUser = _dbDiningContext.RestaurantMerchantUsers.Where(x => x.Deleted == false).ToList();
                if (userRoleId != (int)UserRoleEnum.DiningMerchantManager)
                {
                    listMerchantResult = _dbDiningContext.RestaurantMerchants.AsNoTracking().Where(x => !listMerchantHasUser.Any(y => y.MerchantFid == x.Id)).Select(r => new SelectListModel(r.MerchantName, r.Id.ToString())).ToList();
                }
                else
                {
                    var listRmHasMgt = (from rm in _dbDiningContext.RestaurantMerchants
                                        join rmamgt in _dbDiningContext.RestaurantMerchantAqmgts on rm.Id equals rmamgt.MerchantFid
                                        where rm.Deleted == false && rmamgt.Deleted == false
                                        && rmamgt.AqadminUserFid == userId
                                        select rm).ToList();
                    listMerchantResult = listRmHasMgt.Where(x => !listMerchantHasUser.Any(y => y.MerchantFid == x.Id)).Select(r => new SelectListModel(r.MerchantName, r.Id.ToString())).ToList();
                }
                return listMerchantResult;
            }
            catch
            {
                throw;
            }
        }

        //Get list restaurant merchant don't have manager
        public List<SelectListModel> GetRestaurantMerchantWithoutManagerSelectList()
        {
            var userId = _workContext.UserGuid;
            var userRoleId = _workContext.UserRoleId;
            List<SelectListModel> listMerchantResult = new List<SelectListModel>();
            var listMerchantHasManager = _dbDiningContext.RestaurantMerchantAqmgts.Where(x => x.Deleted == false).ToList();
            if (listMerchantHasManager.Count > 0)
            {
                if (userRoleId != (int)UserRoleEnum.DiningMerchantManager)
                {
                    listMerchantResult = _dbDiningContext.RestaurantMerchants.Where(x => !listMerchantHasManager.Any(y => y.MerchantFid == x.Id) && x.Deleted == false).Select(r => new SelectListModel(r.MerchantName, r.Id.ToString())).ToList();
                }
                else
                {
                    listMerchantResult = _dbDiningContext.RestaurantMerchants.Where(x => x.Deleted == false && !listMerchantHasManager.Any(y => y.MerchantFid == x.Id) && x.CreatedBy.Equals(userId)).Select(r => new SelectListModel(r.MerchantName, r.Id.ToString())).ToList();
                }
            }

            return listMerchantResult;
        }

        //Get all restaurant merchant
        public List<SelectListModel> GetAllRestaurantMerchantSelectList()
        {
            var userId = _workContext.UserGuid;
            var userRoleId = _workContext.UserRoleId;
            List<SelectListModel> listMerchantResult;

            if (userRoleId != (int)UserRoleEnum.DiningMerchantManager)
            {
                listMerchantResult = (from rm in _dbDiningContext.RestaurantMerchants
                                      join mgt in _dbDiningContext.RestaurantMerchantAqmgts on rm.Id equals mgt.MerchantFid
                                      where rm.Deleted == false && mgt.Deleted == false
                                      select new SelectListModel(rm.MerchantName, rm.Id.ToString())).ToList();
            }
            else
            {
                listMerchantResult = (from rm in _dbDiningContext.RestaurantMerchants
                                      join mgt in _dbDiningContext.RestaurantMerchantAqmgts on rm.Id equals mgt.MerchantFid
                                      where rm.Deleted == false && mgt.Deleted == false && mgt.AqadminUserFid == userId
                                      select new SelectListModel(rm.MerchantName, rm.Id.ToString())).ToList();
            }
            return listMerchantResult;
        }

        //Get list dam has merchant
        public List<string> GetListDamHasMerchant()
        {
            try
            {
                var listUser = _dbDiningContext.RestaurantMerchantAqmgts.Where(x => x.Deleted == false).Select(x => x.AqadminUserFid.ToString()).ToList();
                return listUser.Count > 0 ? listUser : new List<string>();
            }
            catch
            {
                throw;
            }
        }

        //Get list dm has merchant
        public List<string> GetListDmHasMerchant()
        {
            try
            {
                var listUser = _dbDiningContext.RestaurantMerchantUsers.Where(x => x.Deleted == false).Select(x => x.UserFid.ToString()).ToList();
                return listUser.Count > 0 ? listUser : new List<string>();
            }
            catch
            {
                throw;
            }
        }

        #endregion Methods
    }
}