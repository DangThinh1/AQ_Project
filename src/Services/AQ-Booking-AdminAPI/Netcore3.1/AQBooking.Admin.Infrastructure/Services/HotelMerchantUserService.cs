using AQBooking.Admin.Core.Models.HotelMerchantUser;
using AQBooking.Admin.Core.Paging;
using AQBooking.Admin.Infrastructure.Databases.HotelEntities;
using AQBooking.Admin.Infrastructure.Helpers;
using AQBooking.Admin.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using AutoMapper;

namespace AQBooking.Admin.Infrastructure.Services
{
    public class HotelMerchantUserService : ServiceBase, IHotelMerchantUserService
    {
        #region Fields
        #endregion

        #region Ctor
        public HotelMerchantUserService(
            AQHotelContext dbHotelContext,
            IWorkContext workContext,
            IMapper mapper) : base(dbHotelContext, workContext, mapper)
        { }
        #endregion

        #region Methods
        public IPagedList<HotelMerchantUserViewModel> SearchHotelMerchantUser(HotelMerchantUserSearchModel model)
        {
            var searchString = !string.IsNullOrEmpty(model.SortString) ? model.SortString : $"{nameof(HotelMerchantUsers.CreatedDate)} Desc";
            var lstEvisaMerchant = new PagedList<HotelMerchantUserViewModel>();
            var query = _dbHotelContext.HotelMerchantUsers.AsNoTracking().Where(x => x.Deleted == false
            && (string.IsNullOrEmpty(model.UserFid) || x.UserFid.ToString() == model.UserFid)
            && (model.MerchantFid == 0 || x.MerchantFid == model.MerchantFid)
            && (model.EffectiveStartDate == null || x.EffectiveStartDate >= DateTime.Now)
            && (model.EffectiveEndDate == null || x.EffectiveEndDate <= DateTime.Now)).Select(x => _mapper.Map<HotelMerchantUserViewModel>(x)).OrderBy(searchString);

            if (query.Count() > 0)
                lstEvisaMerchant = new PagedList<HotelMerchantUserViewModel>(query, model.PageIndex, model.PageSize);

            return lstEvisaMerchant;
        }

        public HotelMerchantUserViewModel GetHotelMerchantUserById(int id)
        {
            var evisaMerchant = new HotelMerchantUserViewModel();
            var query = _dbHotelContext.HotelMerchantUsers.AsNoTracking().Where(x => x.Deleted == false && x.Id == id).Select(x => _mapper.Map<HotelMerchantUserViewModel>(x)).AsQueryable();
            if (query.Count() > 0)
                evisaMerchant = query.FirstOrDefault();

            return evisaMerchant;
        }

        public bool CreateHotelMerchantUser(HotelMerchantUserCreateUpdateModel model)
        {
            var result = false;
            var entity = new HotelMerchantUsers();
            entity = _mapper.Map<HotelMerchantUserCreateUpdateModel, HotelMerchantUsers>(model, entity);
            entity.CreatedBy = _workContext.UserGuid;
            entity.CreatedDate = DateTime.Now;
            entity.LastModifiedBy = _workContext.UserGuid;
            entity.LastModifiedDate = DateTime.Now;
            var status = _dbHotelContext.HotelMerchantUsers.Add(entity);
            _dbHotelContext.SaveChanges();

            if (status.State == EntityState.Unchanged)
                result = true;

            return result;
        }

        public bool UpdateHotelMerchantUser(HotelMerchantUserCreateUpdateModel model)
        {
            var result = false;
            var entity = new HotelMerchantUsers();
            var query = _dbHotelContext.HotelMerchantUsers.Where(x => x.Deleted == false && x.Id == model.Id);
            if (query.Count() > 0)
                entity = query.FirstOrDefault();
            else
                return result;
            entity = _mapper.Map<HotelMerchantUserCreateUpdateModel, HotelMerchantUsers>(model, entity);
            entity.LastModifiedBy = _workContext.UserGuid;
            entity.LastModifiedDate = DateTime.Now;
            var status = _dbHotelContext.HotelMerchantUsers.Update(entity);
            _dbHotelContext.SaveChanges();

            if (status.State == EntityState.Unchanged)
                result = true;

            return result;
        }

        public bool DeleteHotelMerchantUser(int id)
        {
            var result = false;
            var entity = new HotelMerchantUsers();
            var query = _dbHotelContext.HotelMerchantUsers.Where(x => x.Deleted == false && x.Id == id);
            if (query.Count() > 0)
                entity = query.FirstOrDefault();
            else
                return result;
            entity.Deleted = true;
            entity.LastModifiedBy = _workContext.UserGuid;
            entity.LastModifiedDate = DateTime.Now;
            var status = _dbHotelContext.HotelMerchantUsers.Update(entity);
            _dbHotelContext.SaveChanges();

            if (status.State == EntityState.Unchanged)
                result = true;

            return result;
        }
        #endregion
    }
}
