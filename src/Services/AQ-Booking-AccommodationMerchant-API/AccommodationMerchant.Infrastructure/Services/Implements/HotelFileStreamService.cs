using AccommodationMerchant.Core.Models.HotelFileStreamModel;
using AccommodationMerchant.Infrastructure.Databases;
using AQBooking.Core.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Text;
using AccommodationMerchant.Infrastructure.Databases.Entities;
using AutoMapper;
using Identity.Core.Helpers;
using AccommodationMerchant.Infrastructure.Services.Interfaces;


namespace AccommodationMerchant.Infrastructure.Services.Implements
{
    public class HotelFileStreamService : IHotelFileStreamService
    {
        #region Fields
        private readonly AccommodationContext _accommodationContext;
        private readonly IMapper _mapper;
        #endregion

        #region Ctor
        public HotelFileStreamService(AccommodationContext accommodationContext,
            IMapper mapper)
        {
            this._accommodationContext = accommodationContext;
            this._mapper = mapper;
        }
        #endregion

        #region Methods
        public PagedList<HotelFileStreamViewModel> SearchHotelFileStream(HotelFileStreamSearchModel model)
        {
            var sortString = !string.IsNullOrEmpty(model.SortString) ? model.SortString : $"{nameof(HotelFileStreams.LastModifiedDate)}";
            var hotelFsPagedList = new PagedList<HotelFileStreamViewModel>();
            var query = _accommodationContext.HotelFileStreams.AsNoTracking().Where(x => x.Deleted == false
            && (model.FileTypeFid == 0 || x.FileTypeFid == model.FileTypeFid)
            && (model.FileCategoryFid == 0 || x.FileCategoryFid == model.FileCategoryFid))
            .Select(x => _mapper.Map<HotelFileStreamViewModel>(x));
            if (query.Count() > 0)
                hotelFsPagedList = new PagedList<HotelFileStreamViewModel>(query, model.PageIndex, model.PageSize);

            return hotelFsPagedList;
        }

        public HotelFileStreamViewModel GetHotelFileStreamById(int id)
        {
            var hotelFs = new HotelFileStreamViewModel();
            var query = _accommodationContext.HotelFileStreams.AsNoTracking().Where(x => x.Deleted == false).Select(x => _mapper.Map<HotelFileStreamViewModel>(x));
            if (query.Count() > 0)
                hotelFs = query.FirstOrDefault();

            return hotelFs;
        }

        public bool CreateHotelFileStream(HotelFileStreamCreateUpdateModel parameters)
        {
            var result = false;
            var entity = new HotelFileStreams();
            entity = _mapper.Map<HotelFileStreams>(parameters);
            entity.Deleted = false;
            entity.LastModifiedDate = DateTime.Now;
            entity.LastModifiedBy = UserContextHelper.UserId;
            var add = _accommodationContext.Add(entity);
            _accommodationContext.SaveChanges();

            if (add.State == EntityState.Unchanged)
                result = true;

            return result;
        }

        public bool UpdateHotelFileStream(HotelFileStreamCreateUpdateModel parameters)
        {
            var result = false;
            var entity = new HotelFileStreams();
            var query = _accommodationContext.HotelFileStreams.Where(x => x.Deleted == false && x.Id == parameters.Id);
            if (query.Count() == 0)
                return result;

            entity = query.FirstOrDefault();
            entity = _mapper.Map<HotelFileStreamCreateUpdateModel, HotelFileStreams>(parameters, entity);
            entity.LastModifiedBy = UserContextHelper.UserId;
            entity.LastModifiedDate = DateTime.Now;
            var update = _accommodationContext.Update(entity);
            _accommodationContext.SaveChanges();

            if (update.State == EntityState.Unchanged)
                result = true;

            return result;
        }

        public bool DeleteHotelFileStream(int id)
        {
            var result = false;
            var entity = new HotelFileStreams();
            var query = _accommodationContext.HotelFileStreams.Where(x => x.Deleted == false && x.Id == id);
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
