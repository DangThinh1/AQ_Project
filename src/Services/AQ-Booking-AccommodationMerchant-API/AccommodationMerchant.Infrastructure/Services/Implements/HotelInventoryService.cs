using System;
using AutoMapper;
using System.Linq;
using APIHelpers.Response;
using Identity.Core.Helpers;
using AQBooking.Core.Helpers;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using AQConfigurations.Core.Services.Interfaces;
using AccommodationMerchant.Infrastructure.Databases;
using AccommodationMerchant.Core.Models.HotelInventories;
using AccommodationMerchant.Infrastructure.Databases.Entities;
using AccommodationMerchant.Infrastructure.Services.Interfaces;
using AQEncrypts;

namespace AccommodationMerchant.Infrastructure.Services.Implements
{
    public class HotelInventoryService : ServiceBase, IHotelInventoryService
    {
        private readonly ICommonValueRequestService _commonValueRequestService;
        public HotelInventoryService(AccommodationContext db, IMapper mapper,
            ICommonValueRequestService commonValueRequestService) : base(db, mapper)
        {
            _commonValueRequestService = commonValueRequestService;
        }

        public BaseResponse<PagedList<HotelInventoryViewModel>> Search(HotelInventorySearchModel model)
        {
            try
            {
                var hotelId = model.HotelFid.DecryptToInt32();
                if (model != null && hotelId != 0)
                {
                    //Filter values
                    var roomName = !string.IsNullOrEmpty(model.RoomName) ? model.RoomName.ToUpper() : string.Empty;
                    var fromDate = model.FromDate;
                    var toDate = model.ToDate;

                    //Paging values
                    var pageIndex = model.PageIndex > 0 ? model.PageIndex : 1;
                    var pageSize = model.PageSize > 0 ? model.PageSize : 10;
                    var sortColumn = !string.IsNullOrEmpty(model.SortColumn) ? model.SortColumn : "RoomName";
                    var sortType = !string.IsNullOrEmpty(model.SortType) ? model.SortType : "ASC";
                    var sortString = $"{sortColumn} {sortType}";

                    //Query
                    var query = _db.HotelInventories
                        .AsNoTracking()
                        .Where(k => !k.Deleted)
                        .Where(k => hotelId != 0 && k.HotelFid == hotelId)
                        .Where(k => string.IsNullOrEmpty(roomName) || k.RoomName.ToUpper().Contains(roomName))
                        .Where(k => (fromDate == null || k.ActivatedDate >= fromDate) && (toDate == null || k.ActivatedDate <= toDate))
                        .Select(k=> _mapper.Map<HotelInventories, HotelInventoryViewModel>(k))
                        .OrderBy(sortString);

                    var result = new PagedList<HotelInventoryViewModel>(query, pageIndex, pageSize);
                    return BaseResponse<PagedList<HotelInventoryViewModel>>.Success(result);
                }
                return BaseResponse<PagedList<HotelInventoryViewModel>>.BadRequest();
            }
            catch(Exception ex)
            {
                return BaseResponse<PagedList<HotelInventoryViewModel>>.InternalServerError(ex);
            }
        }

        public BaseResponse<HotelInventoryViewModel> Find(long id)
        {
            try
            {
                var entity = _db.HotelInventories.FirstOrDefault(k => !k.Deleted && k.Id == id);
                if (entity != null)
                {
                    var model = _mapper.Map<HotelInventories, HotelInventoryViewModel>(entity);
                    return BaseResponse<HotelInventoryViewModel>.Success(model);
                }
                return BaseResponse<HotelInventoryViewModel>.BadRequest();
            }
            catch(Exception ex)
            {
                return BaseResponse<HotelInventoryViewModel>.InternalServerError(ex);
            }
        }

        public BaseResponse<bool> Create(HotelInventoryCreateModel model)
        {
            try
            {
                if (model != null)
                {
                    var roomTypeResKey = _commonValueRequestService.Find(model.RoomTypeFid).ResponseData?.ResourceKey ?? string.Empty;
                    var entity = _mapper.Map<HotelInventoryCreateModel, HotelInventories>(model);
                    entity.RoomTypeResKey = roomTypeResKey;
                    entity.Deleted = false;
                    entity.IsActivated = false;
                    entity.ActivatedDate = model.ActivatedDate;
                    entity.ActivatedBy = (Guid?)UserContextHelper.UserId;
                    entity.LastModifiedBy = UserContextHelper.UserId;
                    entity.LastModifiedDate = DateTime.Now;
                    _db.HotelInventories.Add(entity);
                    _db.SaveChanges();
                    return BaseResponse<bool>.Success(true);
                }
                return BaseResponse<bool>.BadRequest();
            }
            catch(Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(ex);
            }
        }

        public BaseResponse<bool> Update(HotelInventoryUpdateModel model)
        {
            try
            {
                if (model != null)
                {
                    var entity = _db.HotelInventories.FirstOrDefault(k => !k.Deleted && k.Id == model.Id);
                    if (entity == null)
                        return BaseResponse<bool>.BadRequest();

                    entity.LastModifiedBy = UserContextHelper.UserId;
                    entity.LastModifiedDate = DateTime.Now;
                    _db.HotelInventories.Update(entity);
                    _db.SaveChanges();
                    return BaseResponse<bool>.Success(true);
                }
                return BaseResponse<bool>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(ex);
            }
        }

        public BaseResponse<bool> Activate(long id)
        {
            try
            {
                var entity = _db.HotelInventories.FirstOrDefault(k => !k.Deleted && k.Id == id);
                if (entity == null)
                    return BaseResponse<bool>.BadRequest();
                entity.IsActivated = true;
                entity.ActivatedBy = UserContextHelper.UserId;
                entity.LastModifiedBy = UserContextHelper.UserId;
                entity.LastModifiedDate = DateTime.Now;
                _db.HotelInventories.Update(entity);
                _db.SaveChanges();
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(ex);
            }
        }

        public BaseResponse<bool> Deactivate(long id)
        {
            try
            {
                var entity = _db.HotelInventories.FirstOrDefault(k => !k.Deleted && k.Id == id);
                if (entity == null)
                    return BaseResponse<bool>.BadRequest();
                entity.IsActivated = false;
                entity.ActivatedBy = UserContextHelper.UserId;
                entity.LastModifiedBy = UserContextHelper.UserId;
                entity.LastModifiedDate = DateTime.Now;
                _db.HotelInventories.Update(entity);
                _db.SaveChanges();
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(ex);
            }
        }

        public BaseResponse<bool> Delete(long id)
        {
            try
            {
                var entity = _db.HotelInventories.FirstOrDefault(k => !k.Deleted && k.Id == id);
                if (entity == null)
                    return BaseResponse<bool>.BadRequest();
                entity.Deleted = true;
                entity.LastModifiedBy = UserContextHelper.UserId;
                entity.LastModifiedDate = DateTime.Now;
                _db.HotelInventories.Update(entity);
                _db.SaveChanges();
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(ex);
            }
        }
    }
}