using AccommodationMerchant.Core.Models.HotelInventoryFileStreamModel;
using AccommodationMerchant.Infrastructure.Databases;
using AccommodationMerchant.Infrastructure.Databases.Entities;
using AccommodationMerchant.Infrastructure.Services.Interfaces;
using AQBooking.Core.Helpers;
using AutoMapper;
using Identity.Core.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccommodationMerchant.Infrastructure.Services.Implements
{
    public class HotelInventoryFileStreamService : IHotelInventoryFileStreamService
    {
        #region Fields
        private readonly AccommodationContext _accommodationContext;
        private readonly IMapper _mapper;
        #endregion

        #region Ctor
        public HotelInventoryFileStreamService(AccommodationContext accommodationContext,
            IMapper mapper)
        {
            this._accommodationContext = accommodationContext;
            this._mapper = mapper;
        }
        #endregion

        #region Methods
        public PagedList<HotelInventoryFileStreamViewModel> SearchHotelInventoryFileStream(HotelInventoryFileStreamSearchModel model)
        {
            var sortString = !string.IsNullOrEmpty(model.SortString) ? model.SortString : $"{nameof(HotelInventoryFileStreams.LastModifiedDate)}";
            var hotelFsPagedList = new PagedList<HotelInventoryFileStreamViewModel>();
            var query = _accommodationContext.HotelInventoryFileStreams.AsNoTracking().Where(x => x.Deleted == false
            && (model.FileTypeFid == 0 || x.FileTypeFid == model.FileTypeFid)
            && (model.FileCategoryFid == 0 || x.FileCategoryFid == model.FileCategoryFid))
            .Select(x => _mapper.Map<HotelInventoryFileStreamViewModel>(x));
            if (query.Count() > 0)
                hotelFsPagedList = new PagedList<HotelInventoryFileStreamViewModel>(query, model.PageIndex, model.PageSize);

            return hotelFsPagedList;
        }

        public HotelInventoryFileStreamViewModel GetHotelInventoryFileStreamById(int id)
        {
            var hotelFs = new HotelInventoryFileStreamViewModel();
            var query = _accommodationContext.HotelInventoryFileStreams.AsNoTracking().Where(x => x.Deleted == false).Select(x => _mapper.Map<HotelInventoryFileStreamViewModel>(x));
            if (query.Count() > 0)
                hotelFs = query.FirstOrDefault();

            return hotelFs;
        }

        public bool CreateHotelInventoryFileStream(HotelInventoryFileStreamCreateUpdateModel parameters)
        {
            var result = false;
            var entity = new HotelInventoryFileStreams();
            entity = _mapper.Map<HotelInventoryFileStreams>(parameters);
            entity.Deleted = false;
            entity.LastModifiedDate = DateTime.Now;
            entity.LastModifiedBy = UserContextHelper.UserId;
            var add = _accommodationContext.Add(entity);
            _accommodationContext.SaveChanges();

            if (add.State == EntityState.Unchanged)
                result = true;

            return result;
        }

        public bool UpdateHotelInventoryFileStream(HotelInventoryFileStreamCreateUpdateModel parameters)
        {
            var result = false;
            var entity = new HotelInventoryFileStreams();
            var query = _accommodationContext.HotelInventoryFileStreams.Where(x => x.Deleted == false && x.Id == parameters.Id);
            if (query.Count() == 0)
                return result;

            entity = query.FirstOrDefault();
            entity = _mapper.Map<HotelInventoryFileStreamCreateUpdateModel, HotelInventoryFileStreams>(parameters, entity);
            entity.LastModifiedBy = UserContextHelper.UserId;
            entity.LastModifiedDate = DateTime.Now;
            var update = _accommodationContext.Update(entity);
            _accommodationContext.SaveChanges();

            if (update.State == EntityState.Unchanged)
                result = true;

            return result;
        }

        public bool DeleteHotelInventoryFileStream(int id)
        {
            var result = false;
            var entity = new HotelInventoryFileStreams();
            var query = _accommodationContext.HotelInventoryFileStreams.Where(x => x.Deleted == false && x.Id == id);
            if (query.Count() == 0)
                return result;

            entity = query.FirstOrDefault();
            entity.Deleted = true;
            entity.LastModifiedBy = UserContextHelper.UserId;
            entity.LastModifiedDate = DateTime.Now;
            var update = _accommodationContext.Update(entity);
            _accommodationContext.SaveChanges();

            if (update.State == EntityState.Unchanged)
                result = true;

            return result;
        }
        #endregion
    }
}
