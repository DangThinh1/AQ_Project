using System;
using System.Linq;

using System.Collections.Generic;
using AQBooking.Admin.Core.Paging;
using AQBooking.Admin.Infrastructure.Databases.EvisaEntities;
using AQBooking.Admin.Infrastructure.Helpers;
using AQBooking.Admin.Infrastructure.Interfaces;
using AQBooking.Admin.Core.Enums;
using AQBooking.Admin.Core.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AQBooking.Admin.Infrastructure.Databases.HotelEntities;
using AQBooking.Admin.Core.Models.HotelMerchant;

namespace AQBooking.Admin.Infrastructure.Services
{
    public class HotelMerchantService : ServiceBase, IHotelMerchantService
    {
        #region Fields
        #endregion

        #region Ctor
        public HotelMerchantService(
            AQHotelContext dbAQHotelContext,
            IWorkContext workContext,
            IMapper mapper) : base(dbAQHotelContext, workContext, mapper)
        { }
        #endregion

        #region Methods
        public IPagedList<HotelMerchantViewModel> SearchHotelMerchant(HotelMerchantSearchModel model)
        {
            var searchString = !string.IsNullOrEmpty(model.SortString) ? model.SortString : $"{nameof(HotelMerchants.CreatedDate)} Desc";
            var lstHotelMerchant = new PagedList<HotelMerchantViewModel>();
            var query = _dbHotelContext.HotelMerchants.AsNoTracking().Where(x => x.Deleted == false
            && string.IsNullOrEmpty(model.Country) || x.Country.Contains(model.Country)
            && string.IsNullOrEmpty(model.City) || x.City.Contains(model.City)
            && string.IsNullOrEmpty(model.State) || x.State.Contains(model.State)).Select(x => _mapper.Map<HotelMerchantViewModel>(x)).OrderBy(searchString);

            if (query.Count() > 0)
                lstHotelMerchant = new PagedList<HotelMerchantViewModel>(query, model.PageIndex, model.PageSize);

            return lstHotelMerchant;
        }

        public HotelMerchantViewModel GetHotelMerchantById(int id)
        {
            var HotelMerchant = new HotelMerchantViewModel();
            var query = _dbHotelContext.HotelMerchants.AsNoTracking().Where(x => x.Deleted == false && x.Id == id).Select(x => _mapper.Map<HotelMerchantViewModel>(x)).AsQueryable();
            if (query.Count() > 0)
                HotelMerchant = query.FirstOrDefault();

            return HotelMerchant;
        }

        public bool CreateHotelMerchant(HotelMerchantCreateUpdateModel model)
        {
            var result = false;
            var entity = new HotelMerchants();
            entity = _mapper.Map<HotelMerchantCreateUpdateModel, HotelMerchants>(model, entity);
            entity.UniqueId = UniqueIDHelper.GenerateRandomString(12);
            entity.CreatedBy = _workContext.UserGuid;
            entity.CreatedDate = DateTime.Now;
            entity.LastModifiedBy = _workContext.UserGuid;
            entity.LastModifiedDate = DateTime.Now;
            var status = _dbHotelContext.HotelMerchants.Add(entity);
            _dbHotelContext.SaveChanges();

            if (status.State == EntityState.Unchanged)
                result = true;

            return result;
        }

        public bool UpdateHotelMerchant(HotelMerchantCreateUpdateModel model)
        {
            var result = false;
            var entity = new HotelMerchants();
            var query = _dbHotelContext.HotelMerchants.Where(x => x.Deleted == false && x.Id == model.Id);
            if (query.Count() > 0)
                entity = query.FirstOrDefault();
            else
                return result;
            entity = _mapper.Map<HotelMerchantCreateUpdateModel, HotelMerchants>(model, entity);
            entity.LastModifiedBy = _workContext.UserGuid;
            entity.LastModifiedDate = DateTime.Now;
            var status = _dbHotelContext.HotelMerchants.Update(entity);
            _dbHotelContext.SaveChanges();

            if (status.State == EntityState.Unchanged)
                result = true;

            return result;
        }

        public bool DeleteHotelMerchant(int id)
        {
            var result = false;
            var entity = new HotelMerchants();
            var query = _dbHotelContext.HotelMerchants.Where(x => x.Deleted == false && x.Id == id);
            if (query.Count() > 0)
                entity = query.FirstOrDefault();
            else
                return result;
            entity.Deleted = true;
            entity.LastModifiedBy = _workContext.UserGuid;
            entity.LastModifiedDate = DateTime.Now;
            var status = _dbHotelContext.HotelMerchants.Update(entity);
            _dbHotelContext.SaveChanges();

            if (status.State == EntityState.Unchanged)
                result = true;

            return result;
        }

        /// <summary>
        /// Get list evisa merchant selectlist don't have user account
        /// </summary>
        /// <returns></returns>
        public List<SelectListModel> GetHotelMerchantNoUserSll()
        {
            var userId = _workContext.UserGuid;
            var userRoleId = _workContext.UserRoleId;
            var listMerchantSll = new List<SelectListModel>();
            var listMerchantHasUser = _dbHotelContext.HotelMerchantUsers.Where(x => x.Deleted == false).ToList();
            if (userRoleId != (int)UserRoleEnum.AccommodationMerchantManager)
            {
                listMerchantSll = _dbHotelContext.HotelMerchants.Where(x => !listMerchantHasUser.Any(y => y.MerchantFid == x.Id)).Select(r => new SelectListModel(r.MerchantName, r.Id.ToString())).ToList();
            }
            else
            {
                //Pending because table visamerchantmanager has not created    
            }

            return listMerchantSll;
        }

        /// <summary>
        /// Get all evisa merchant selectlist
        /// </summary>
        /// <returns></returns>
        public List<SelectListModel> GetAllHotelMerchantSll()
        {
            var lstMerchant = new List<SelectListModel>();
            var query = _dbHotelContext.HotelMerchants.Where(x => x.Deleted == false).Select(x => new SelectListModel(x.MerchantName, x.Id.ToString()));
            if (query.Count() > 0)
                lstMerchant = query.ToList();
            return lstMerchant;
        }
        #endregion
    }
}
