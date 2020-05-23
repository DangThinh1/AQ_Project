using AQBooking.Admin.Core.Enums;
using AQBooking.Admin.Core.Models;
using AQBooking.Admin.Core.Models.YachtMerchant;
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
    public class YachtMerchantService : ServiceBase, IYachtMerchantService
    {
        #region Fields
        #endregion Fields

        #region Ctor

        public YachtMerchantService(
             AQYachtContext dbContext,
            IWorkContext workContext,
            IMapper mapper) : base(dbContext, workContext, mapper)
        {
        }

        #endregion Ctor

        #region Methods

        public IPagedList<YachtMerchantViewModel> SearchYachtMerchant(YachtMerchantSearchModel model)
        {
            var sortString = !string.IsNullOrEmpty(model.SortString) ? model.SortString : "CreatedDate DESC";
            var userId = _workContext.UserGuid;
            var userRoleId = _workContext.UserRoleId;
            IQueryable<YachtMerchantViewModel> query;
            if (userRoleId != (int)UserRoleEnum.YachtMerchantManager)
            {
                query = (from rm in _dbYachtContext.YachtMerchants
                         where rm.Deleted == false
                        && (string.IsNullOrEmpty(model.SearchString) || rm.MerchantName.Contains(model.SearchString) || rm.EmailAddress1.Contains(model.SearchString) || rm.EmailAddress2.Contains(model.SearchString) || rm.ContactNumber1.Contains(model.SearchString) || rm.ContactNumber2.Contains(model.SearchString))
                        && (string.IsNullOrEmpty(model.Country) || rm.Country.Contains(model.Country))
                        && (string.IsNullOrEmpty(model.City) || rm.City.Contains(model.City))
                        && (string.IsNullOrEmpty(model.State) || rm.State.Contains(model.State))
                         select _mapper.Map<YachtMerchantViewModel>(rm)).OrderBy(sortString).AsQueryable();
            }
            else
            {
                query = (from rm in _dbYachtContext.YachtMerchants
                         join yam in _dbYachtContext.YachtMerchantAqmgts on rm.Id equals yam.MerchantFid
                         where rm.Deleted == false && yam.Deleted == false && yam.AqadminUserFid.Equals(userId)
                         && (string.IsNullOrEmpty(model.SearchString) || rm.MerchantName.Contains(model.SearchString) || rm.EmailAddress1.Contains(model.SearchString) || rm.EmailAddress2.Contains(model.SearchString) || rm.ContactNumber1.Contains(model.SearchString) || rm.ContactNumber2.Contains(model.SearchString))
                         && (string.IsNullOrEmpty(model.Country) || rm.Country.Contains(model.Country))
                         && (string.IsNullOrEmpty(model.City) || rm.City.Contains(model.City))
                         && (string.IsNullOrEmpty(model.State) || rm.State.Contains(model.State))
                         select _mapper.Map<YachtMerchantViewModel>(rm)).OrderBy(sortString).AsQueryable();
            }

            return new PagedList<YachtMerchantViewModel>(query, model.PageIndex, model.PageSize);
        }

        public bool CreateYachtMerchant(YachtMerchantCreateModel model)
        {
            using (var trans = _dbYachtContext.Database.BeginTransaction())
            {
                try
                {
                    var userGuid = _workContext.UserGuid;
                    var userRoleId = _workContext.UserRoleId;
                    var entity = _mapper.Map<YachtMerchants>(model);

                    entity.UniqueId = UniqueIDHelper.GenerateRandomString(12, false);
                    entity.CreatedBy = userGuid;
                    entity.CreatedDate = DateTime.Now;
                    entity.LastModifiedBy = userGuid;
                    entity.LastModifiedDate = DateTime.Now;
                    _dbYachtContext.YachtMerchants.Add(entity);
                    _dbYachtContext.SaveChanges();

                    if (userRoleId == (int)UserRoleEnum.YachtMerchantManager)
                    {
                        var merchantAccountMgt = new YachtMerchantAqmgts
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
                        _dbYachtContext.YachtMerchantAqmgts.AddAsync(merchantAccountMgt);
                        _dbYachtContext.SaveChanges();
                    }
                    
                    trans.Commit();
                    trans.Dispose();
                    return true;
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    trans.Dispose();
                    throw ex;
                }
            }
        }

        public YachtMerchantViewModel FindYachtMerchantByIdAsync(int id)
        {
            try
            {
                var entity = _dbYachtContext.YachtMerchants.Where(x => x.Id == id && x.Deleted == false).SingleOrDefault();
                if (entity == null)
                    return new YachtMerchantViewModel();
                var result = _mapper.Map<YachtMerchantViewModel>(entity);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateYachtMerchantAsync(YachtMerchantCreateModel model)
        {
            try
            {
                var entity = _dbYachtContext.YachtMerchants.Where(x => x.Id == model.Id && x.Deleted == false).SingleOrDefault();
                entity = _mapper.Map<YachtMerchantCreateModel, YachtMerchants>(model, entity);
                entity.LastModifiedBy = _workContext.UserGuid;
                entity.LastModifiedDate = DateTime.Now;
                _dbYachtContext.YachtMerchants.Update(entity);
                _dbYachtContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteYachtMerchant(int id)
        {
            try
            {
                var entity = _dbYachtContext.YachtMerchants.Where(x => x.Id == id && x.Deleted == false).SingleOrDefault();
                if (entity == null)
                    throw new Exception("Yacht Merchant not found!");
                entity.Deleted = true;
                entity.LastModifiedBy = _workContext.UserGuid;
                entity.LastModifiedDate = DateTime.Now;
                _dbYachtContext.YachtMerchants.Update(entity);
                _dbYachtContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool RemoveYachtMerchant(int id)
        {
            try
            {
                var entity = _dbYachtContext.YachtMerchants.Where(x => x.Id == id && x.Deleted == false).SingleOrDefault();
                if (entity == null)
                    throw new Exception("Yacht Merchant not found!");
                _dbYachtContext.YachtMerchants.Remove(entity);
                _dbYachtContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Get list yacht merchant don't have account
        public List<SelectListModel> GetYachtMerchantWithoutAccountSelectList()
        {
            var userId = _workContext.UserGuid;
            var userRoleId = _workContext.UserRoleId;
            List<SelectListModel> listMerchantResult = new List<SelectListModel>();
            var listMerchantHasUser = _dbYachtContext.YachtMerchantUsers.Where(x => x.Deleted == false).ToList();
            if (userRoleId != (int)UserRoleEnum.YachtMerchantManager)
            {
                listMerchantResult = _dbYachtContext.YachtMerchants.Where(x => !listMerchantHasUser.Any(y => y.MerchantFid == x.Id)).Select(r => new SelectListModel(r.MerchantName, r.Id.ToString())).ToList();
            }
            else
            {
                var listYmHasMgt = (from ym in _dbYachtContext.YachtMerchants
                                    join ymamgt in _dbYachtContext.YachtMerchantAqmgts on ym.Id equals ymamgt.MerchantFid
                                    where ym.Deleted == false && ymamgt.Deleted == false
                                    && ymamgt.AqadminUserFid == userId
                                    select ym).ToList();
                listMerchantResult = listYmHasMgt.Where(x => !listMerchantHasUser.Any(y => y.MerchantFid == x.Id)).Select(r => new SelectListModel(r.MerchantName, r.Id.ToString())).ToList();
            }
            return listMerchantResult;
        }

        //Get list yacht merchant don't have manager
        public List<SelectListModel> GetYachtMerchantWithoutManagerSelectList()
        {
            var userId = _workContext.UserGuid;
            var userRoleId = _workContext.UserRoleId;
            List<SelectListModel> listMerchantResult = new List<SelectListModel>();
            var listMerchantHasManager = _dbYachtContext.YachtMerchantAqmgts.Where(x => x.Deleted == false).ToList();
            if (listMerchantHasManager.Count > 0)
            {
                if (userRoleId != (int)UserRoleEnum.YachtMerchantManager)
                {
                    listMerchantResult = _dbYachtContext.YachtMerchants.Where(x => !listMerchantHasManager.Any(y => y.MerchantFid == x.Id) && x.Deleted == false).Select(r => new SelectListModel(r.MerchantName, r.Id.ToString())).ToList();
                }
                else
                {
                    listMerchantResult = _dbYachtContext.YachtMerchants.Where(x => x.Deleted == false && x.CreatedBy.Equals(userId) && !listMerchantHasManager.Any(y => y.MerchantFid == x.Id)).Select(r => new SelectListModel(r.MerchantName, r.Id.ToString())).ToList();
                }
            }

            return listMerchantResult;
        }

        //Get all yacht merchant
        public List<SelectListModel> GetAllYachtMerchantSelectList()
        {
            var userId = _workContext.UserGuid;
            var userRoleId = _workContext.UserRoleId;
            List<SelectListModel> listMerchantResult;

            if (userRoleId != (int)UserRoleEnum.YachtMerchantManager)
            {
                listMerchantResult = (from ym in _dbYachtContext.YachtMerchants
                                      join mgt in _dbYachtContext.YachtMerchantAqmgts on ym.Id equals mgt.MerchantFid
                                      where ym.Deleted == false && mgt.Deleted == false
                                      select new SelectListModel(ym.MerchantName, ym.Id.ToString())).ToList();
            }
            else
            {
                listMerchantResult = (from rm in _dbYachtContext.YachtMerchants
                                      join mgt in _dbYachtContext.YachtMerchantAqmgts on rm.Id equals mgt.MerchantFid
                                      where rm.Deleted == false && mgt.Deleted == false && mgt.AqadminUserFid == userId
                                      select new SelectListModel(rm.MerchantName, rm.Id.ToString())).ToList();
            }

            return listMerchantResult;
        }

        //Get list yam has merchant
        public List<string> GetListYamHasMerchant()
        {
            try
            {
                var listUser = _dbYachtContext.YachtMerchantAqmgts.Where(x => x.Deleted == false).Select(x => x.AqadminUserFid.ToString()).ToList();
                return listUser.Count > 0 ? listUser : new List<string>();
            }
            catch
            {
                throw;
            }
        }

        //Get list ym has merchant
        public List<string> GetListYmHasMerchant()
        {
            try
            {
                var listUser = _dbYachtContext.YachtMerchantUsers.Where(x => x.Deleted == false).Select(x => x.UserFid.ToString()).ToList();
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