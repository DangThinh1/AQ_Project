using AccommodationMerchant.Core.Helpers;
using AccommodationMerchant.Core.Models.HotelInformationDetails;
using AccommodationMerchant.Infrastructure.Databases;
using AccommodationMerchant.Infrastructure.Databases.Entities;
using AccommodationMerchant.Infrastructure.Services.Interfaces;
using APIHelpers.Response;
using AQBooking.Core.Helpers;
using AutoMapper;
using Identity.Core.Helpers;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace AccommodationMerchant.Infrastructure.Services.Implements
{
    public class HotelInformationDetailService : ServiceBase, IHotelInformationDetailService
    {
        public HotelInformationDetailService(AccommodationContext db, IMapper mapper) : base(db, mapper)
        {
        }

        public BaseResponse<HotelInformationDetailViewModel> Find(long id)
        {
            try
            {
                var entity = _db.HotelInformationDetails.FirstOrDefault(k => k.Id == id && !k.Deleted);
                if (entity != null)
                {
                    var model = _mapper.Map<HotelInformationDetails, HotelInformationDetailViewModel>(entity);
                    return BaseResponse<HotelInformationDetailViewModel>.Success(model);
                }
                return BaseResponse<HotelInformationDetailViewModel>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<HotelInformationDetailViewModel>.InternalServerError(ex);
            }
        }

        public BaseResponse<PagedList<HotelInformationDetailViewModel>> Search(HotelInformationDetailSearchModel model)
        {
            try
            {
                if (model != null)
                {
                    //Filter values
                    var informationFid = Decrypt.DecryptToInt32(model.InformationFid);

                    //Paging values
                    var pageIndex = model.PageIndex > 0 ? model.PageIndex : 1;
                    var pageSize = model.PageSize > 0 ? model.PageSize : 10;
                    var sortColumn = !string.IsNullOrEmpty(model.SortColumn) ? model.SortColumn : "Id";
                    var sortType = !string.IsNullOrEmpty(model.SortType) ? model.SortType : "ASC";
                    var sortString = $"{sortColumn} {sortType}";

                    //Query data
                    var query = _db.HotelInformationDetails
                    .Where(k => !k.Deleted)
                    .Where(k => k.InformationFid == informationFid)
                    .Select(k => _mapper.Map<HotelInformationDetails, HotelInformationDetailViewModel>(k))
                    .OrderBy(sortString);

                    var result = new PagedList<HotelInformationDetailViewModel>(query, pageIndex, pageSize);
                    return BaseResponse<PagedList<HotelInformationDetailViewModel>>.Success(result);
                }
                return BaseResponse<PagedList<HotelInformationDetailViewModel>>.BadRequest();
            }
            catch (Exception ex)
            {
                return BaseResponse<PagedList<HotelInformationDetailViewModel>>.InternalServerError(ex);
            }
        }

        public BaseResponse<bool> Create(HotelInformationDetailCreateModel model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<bool>.BadRequest();

                var entity = _mapper.Map<HotelInformationDetailCreateModel, HotelInformationDetails>(model);
                var now = DateTime.Now.Date;
                var userId = UserContextHelper.UserId;
                entity.UniqueId = UniqueIDHelper.GenarateRandomString(12);
                entity.Deleted = false;
                entity.IsActivated = false;
                entity.LastModifiedBy = userId;
                entity.LastModifiedDate = now;
                _db.HotelInformationDetails.Add(entity);
                _db.SaveChanges();

                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(ex);
            }
        }

        public BaseResponse<bool> Update(HotelInformationDetailUpdateModel model)
        {
            try
            {
                if (model == null)
                    return BaseResponse<bool>.BadRequest();

                var entity = _db.HotelInformationDetails.FirstOrDefault(k => k.Id == model.Id && !k.Deleted);
                var entityInfo = _db.HotelInformations.FirstOrDefault(x => x.Id == model.InformationFid);
                if (entity != null)
                {
                    if (model.LanguageFid == 1)
                    {
                        entityInfo.DefaultTitle = model.Title;
                        entityInfo.ActivatedDate = model.ActivatedDate.Value.Date;
                    }

                    if (model.ActivatedDate.HasValue)
                    {
                        entity.ActivatedDate = model.ActivatedDate.Value.Date;
                    }
                    if (model.FileStreamFid > 0)
                        entity.FileStreamFid = model.FileStreamFid;
                    if (model.FileTypeFid > 0)
                        entity.FileTypeFid = model.FileTypeFid;
                    entity.Title = model.Title;
                    entity.ShortDescriptions = model.ShortDescriptions;
                    entity.FullDescriptions = model.FullDescriptions;
                    entity.LastModifiedDate = DateTime.Now.Date;
                    entity.LastModifiedBy = UserContextHelper.UserId;
                    _db.HotelInformationDetails.Update(entity);
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

        public BaseResponse<bool> IsActivated(long id, bool value)
        {
            try
            {
                var entity = _db.HotelInformationDetails.FirstOrDefault(k => k.Id == id && !k.Deleted);
                if (entity != null)
                {
                    entity.IsActivated = value;
                    entity.ActivatedBy = UserContextHelper.UserId;
                    entity.LastModifiedBy = UserContextHelper.UserId;
                    entity.LastModifiedDate = DateTime.Now.Date;
                    _db.HotelInformationDetails.Update(entity);
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

        public BaseResponse<bool> Delete(long id)
        {
            try
            {
                var entity = _db.HotelInformationDetails.FirstOrDefault(k => k.Id == id && !k.Deleted);
                if (entity != null)
                {
                    entity.Deleted = true;
                    entity.LastModifiedDate = DateTime.Now.Date;
                    entity.LastModifiedBy = UserContextHelper.UserId;
                    _db.HotelInformationDetails.Update(entity);
                    _db.SaveChanges();
                }
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(ex);
            }
        }
    }
}