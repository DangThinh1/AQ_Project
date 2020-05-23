using AQBooking.Admin.Core.Enums;
using AQBooking.Admin.Core.Models.HotelMerchantMgt;
using AQBooking.Admin.Core.Paging;
using AQBooking.Admin.Infrastructure.Databases.HotelEntities;
using AQBooking.Admin.Infrastructure.Helpers;
using AQBooking.Admin.Infrastructure.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace AQBooking.Admin.Infrastructure.Services
{
    public class HotelMerchantAqmgtservice : ServiceBase, IHotelMerchantMgtService
    {
        #region Fields
        #endregion

        #region Ctor
        public HotelMerchantAqmgtservice(
            AQHotelContext dbHotelContext,
            IWorkContext workContext,
            IMapper mapper) : base(dbHotelContext, workContext, mapper)
        { }
        #endregion

        #region Methods
        public IPagedList<HotelMerchantMgtViewModel> SearchHotelMerchantMgt(HotelMerchantMgtSearchModel model)
        {
            var sortString = !string.IsNullOrEmpty(model.SortString) ? model.SortString : $"{nameof(HotelMerchantAqmgts.CreatedDate)} Desc";
            var lstHotelMerchant = new PagedList<HotelMerchantMgtViewModel>();
            var userGuid = _workContext.UserGuid;
            var userRoleId = _workContext.UserRoleId;
            
            var query = (from hmmgt in _dbYachtContext.YachtMerchantAqmgts.AsNoTracking()
                         join hm in _dbYachtContext.YachtMerchants.AsNoTracking() on hmmgt.MerchantFid equals hm.Id
                         where hmmgt.Deleted == false && hm.Deleted == false
                         && (userRoleId == (int)UserRoleEnum.YachtMerchantManager ? hmmgt.AqadminUserFid == userGuid : true)
                         && (string.IsNullOrEmpty(model.AqadminUserFid) || hmmgt.AqadminUserFid.ToString().Contains(model.AqadminUserFid))
                         && (model.MerchantFid == 0 || hmmgt.MerchantFid == model.MerchantFid)
                         && (string.IsNullOrEmpty(model.EffectiveStartDate) || Convert.ToDateTime(model.EffectiveStartDate) <= hmmgt.EffectiveStartDate)
                         && (string.IsNullOrEmpty(model.EffectiveEndDate) || Convert.ToDateTime(model.EffectiveEndDate) >= hmmgt.EffectiveEndDate)
                         select _mapper.Map<HotelMerchantMgtViewModel>(hmmgt)).OrderBy(sortString).AsQueryable();

            if (query.Count() > 0)
                lstHotelMerchant = new PagedList<HotelMerchantMgtViewModel>(query, model.PageIndex, model.PageSize);

            return lstHotelMerchant;
        }

        public HotelMerchantMgtViewModel GetHotelMerchantMgtById(int id)
        {
            var hotelMerchant = new HotelMerchantMgtViewModel();
            var query = _dbHotelContext.HotelMerchantAqmgts.AsNoTracking().Where(x => x.Deleted == false && x.Id == id).Select(x => _mapper.Map<HotelMerchantMgtViewModel>(x)).AsQueryable();
            if (query.Count() > 0)
                hotelMerchant = query.FirstOrDefault();

            return hotelMerchant;
        }

        public bool CreateHotelMerchantMgt(HotelMerchantMgtCreateUpdateModel model)
        {
            var result = false;
            var entity = new HotelMerchantAqmgts();
            entity = _mapper.Map<HotelMerchantMgtCreateUpdateModel, HotelMerchantAqmgts>(model, entity);
            entity.CreatedBy = _workContext.UserGuid;
            entity.CreatedDate = DateTime.Now;
            entity.LastModifiedBy = _workContext.UserGuid;
            entity.LastModifiedDate = DateTime.Now;
            var status = _dbHotelContext.HotelMerchantAqmgts.Add(entity);
            _dbHotelContext.SaveChanges();

            if (status.State == EntityState.Unchanged)
                result = true;

            return result;
        }

        public bool UpdateHotelMerchantMgt(HotelMerchantMgtCreateUpdateModel model)
        {
            var result = false;
            var entity = new HotelMerchantAqmgts();
            var query = _dbHotelContext.HotelMerchantAqmgts.Where(x => x.Deleted == false && x.Id == model.Id);
            if (query.Count() > 0)
                entity = query.FirstOrDefault();
            else
                return result;
            entity = _mapper.Map<HotelMerchantMgtCreateUpdateModel, HotelMerchantAqmgts>(model, entity);
            entity.LastModifiedBy = _workContext.UserGuid;
            entity.LastModifiedDate = DateTime.Now;
            var status = _dbHotelContext.HotelMerchantAqmgts.Update(entity);
            _dbHotelContext.SaveChanges();

            if (status.State == EntityState.Unchanged)
                result = true;

            return result;
        }

        public bool DeleteHotelMerchantMgt(int id)
        {
            var result = false;
            var entity = new HotelMerchantAqmgts();
            var query = _dbHotelContext.HotelMerchantAqmgts.Where(x => x.Deleted == false && x.Id == id);
            if (query.Count() > 0)
                entity = query.FirstOrDefault();
            else
                return result;
            entity.Deleted = true;
            entity.LastModifiedBy = _workContext.UserGuid;
            entity.LastModifiedDate = DateTime.Now;
            var status = _dbHotelContext.HotelMerchantAqmgts.Update(entity);
            _dbHotelContext.SaveChanges();

            if (status.State == EntityState.Unchanged)
                result = true;

            return result;
        }
        #endregion
    }
}
