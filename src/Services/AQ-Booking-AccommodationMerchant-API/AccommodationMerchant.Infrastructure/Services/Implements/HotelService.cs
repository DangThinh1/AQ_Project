using System;
using AutoMapper;
using System.Linq;
using Omu.ValueInjecter;
using APIHelpers.Response;
using Identity.Core.Helpers;
using AQBooking.Core.Helpers;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using AccommodationMerchant.Core.Helpers;
using AccommodationMerchant.Core.Models.Hotels;
using AccommodationMerchant.Infrastructure.Databases;
using AccommodationMerchant.Infrastructure.Databases.Entities;
using AccommodationMerchant.Infrastructure.Services.Interfaces;
using AccommodationMerchant.Core.Models.HotelAttributeValues;
using AQConfigurations.Core.Services.Interfaces;
using System.Threading.Tasks;
using ExtendedUtility;

namespace AccommodationMerchant.Infrastructure.Services.Implements
{
    public class HotelService : ServiceBase, IHotelService
    {
        private readonly ICommonValueRequestService _commonValueRequestService;
        private readonly IHotelAttributeValueService _hotelAttributeValueService;
        public HotelService(AccommodationContext db, IMapper mapper,
            IHotelAttributeValueService hotelAttributeValueService,
            ICommonValueRequestService commonValueRequestService) : base(db, mapper)
        {
            _commonValueRequestService = commonValueRequestService;
            _hotelAttributeValueService = hotelAttributeValueService;
        }

        public async Task<BaseResponse<HotelBasicProfileModel>> GetHotelBasicProfile(int hotelId)
        {
            if (hotelId > 0)
            {
                var result = await _db.Hotels.AsNoTracking().FirstOrDefaultAsync(k => k.Id == hotelId && k.Deleted == false);
                if (result != null)
                {
                    return BaseResponse<HotelBasicProfileModel>.Success(new HotelBasicProfileModel()
                    {
                        HotelFid = result.Id,
                        HotelUniqueId = result.UniqueId,
                        MerchantId = result.MerchantFid,
                        HotelName = result.HotelName,
                        BrandName= result.BrandName
                    });
                }
                else
                    return BaseResponse<HotelBasicProfileModel>.Success(new HotelBasicProfileModel() { HotelFid = 0, MerchantId = 0, HotelUniqueId = string.Empty, HotelName = string.Empty, BrandName=string.Empty });
            }
            else
                return BaseResponse<HotelBasicProfileModel>.Success(new HotelBasicProfileModel() { HotelFid = 0, MerchantId = 0, HotelUniqueId = string.Empty, HotelName = string.Empty, BrandName = string.Empty });
        }

        public BaseResponse<PagedList<HotelViewModel>> Search(HotelSearchModel model)
        {
            try
            {
                if (model != null)
                {
                    //Filter values
                    var merchantFid = Decrypt.DecryptToInt32(model.MerchantFid);

                    //Paging values
                    var pageIndex = model.PageIndex > 0 ? model.PageIndex : 1;
                    var pageSize = model.PageSize > 0 ? model.PageSize : 10;
                    var sortColumn = (!string.IsNullOrEmpty(model.SortColumn)) ? model.SortColumn : "HotelName";
                    var sortType = !string.IsNullOrEmpty(model.SortType) ? model.SortType : "ASC";
                    var sortString = $"{sortColumn} {sortType}";

                    //Query data
                    var query = _db.Hotels.Include(k=>k.Inventories)
                        .AsNoTracking()
                        .Where(k => !k.Deleted)
                        .Where(k => merchantFid != 0 && k.MerchantFid == merchantFid)
                        .Where(k => string.IsNullOrEmpty(model.HotelName) || k.HotelName.Contains(model.HotelName))
                        .Where(k => !model.ActiveForOperation.HasValue || k.ActiveForOperation == model.ActiveForOperation)
                        .Select(k => _mapper.Map<Hotels, HotelViewModel>(k))
                        .OrderBy(sortString);

                    var result = new PagedList<HotelViewModel>(query, pageIndex, pageSize);
                    return BaseResponse<PagedList<HotelViewModel>>.Success(result);
                }
                return BaseResponse<PagedList<HotelViewModel>>.BadRequest();
            }
            catch(Exception ex)
            {
                return BaseResponse<PagedList<HotelViewModel>>.InternalServerError(ex);
            }
        }

        public BaseResponse<HotelViewModel> Find(int id)
        {
            try
            {
                var hotel = _db.Hotels
                    .Include(k => k.Inventories)
                    .FirstOrDefault(k=> k.Id == id && !k.Deleted);
                if (hotel == null)
                    return BaseResponse<HotelViewModel>.BadRequest();
                var viewModel = _mapper.Map<Hotels, HotelViewModel>(hotel);
                return BaseResponse<HotelViewModel>.Success(viewModel);
            }
            catch (Exception ex)
            {
                return BaseResponse<HotelViewModel>.InternalServerError(ex);
            }
        }

        public BaseResponse<int> Create(HotelCreateModel model)
        {
            try
            {
                //Return badRequest if model null
                if (model == null)
                    return BaseResponse<int>.BadRequest();

                //Convert model to hotel
                var entity = _mapper.Map<HotelCreateModel, Hotels>(model);
                var now = DateTime.Now.Date;
                var userId = UserContextHelper.UserId;

                //Generate private properties for hotel
                entity.UniqueId = UniqueIDHelper.GenarateRandomString(12);
                entity.Deleted = false;
                entity.ActiveForOperation = false;
                entity.CreatedDate = now;
                entity.LastModifiedDate = now;
                entity.CreatedBy = userId;
                entity.LastModifiedBy = userId;

                //Load Res Keys
                entity.StatusResKey = _commonValueRequestService.Find(entity.StatusFid)?.ResponseData?.ResourceKey;
                entity.HotelTypeResKey = _commonValueRequestService.Find(entity.HotelTypeFid)?.ResponseData?.ResourceKey;
                entity.HotelCategoryResKey = _commonValueRequestService.Find(entity.HotelCategoryFid)?.ResponseData?.ResourceKey;

                //Insert to database and return success
                _db.Hotels.Add(entity);
                _db.SaveChanges();
                return BaseResponse<int>.Success(entity.Id);
            }
            catch (Exception ex)
            {
                return BaseResponse<int>.InternalServerError(ex);
            }
        }

        public BaseResponse<bool> Update(HotelUpdateModel model)
        {
            try
            {
                //Return badRequest if model null
                if (model == null)
                    return BaseResponse<bool>.BadRequest();
                var hotel = _db.Hotels.FirstOrDefault(k => k.Id == model.Id && !k.Deleted);
                if(hotel == null)
                    return BaseResponse<bool>.BadRequest();
                
                //Update hotel
                hotel.InjectFrom(model);
                hotel.LastModifiedDate = DateTime.Now.Date;
                hotel.LastModifiedBy = UserContextHelper.UserId;
                _db.Hotels.Update(hotel);
                _db.SaveChanges();
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(ex);
            }
        }

        public BaseResponse<bool> Delete(int id)
        {
            try
            {
                var hotel = _db.Hotels.FirstOrDefault(k => k.Id == id && !k.Deleted);
                if (hotel != null)
                {
                    hotel.Deleted = true;
                    hotel.LastModifiedDate = DateTime.Now.Date;
                    hotel.LastModifiedBy = UserContextHelper.UserId;
                    _db.Hotels.Update(hotel);
                    _db.SaveChanges();
                }
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(ex);
            }
        }

        public BaseResponse<bool> ActiveForOperation(int id, bool value)
        {
            try
            {
                var hotel = _db.Hotels.FirstOrDefault(k => k.Id == id && !k.Deleted);
                if (hotel == null)
                    return BaseResponse<bool>.BadRequest();
                hotel.ActiveForOperation = value;
                hotel.LastModifiedDate = DateTime.Now.Date;
                hotel.LastModifiedBy = UserContextHelper.UserId;
                _db.Hotels.Update(hotel);
                _db.SaveChanges();
                return BaseResponse<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(ex);
            }
        }

        public BaseResponse<bool> Setup(HotelCreateModel model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var createHotelResult = Create(model);
                    if(createHotelResult.IsSuccessStatusCode)
                    {
                        var hotelId = createHotelResult.ResponseData;
                        var attributeCreateString = model.HotelAttributesCreateString;
                        var listAtts = attributeCreateString.Split(",");
                        var listAttributetoCreates = listAtts.Select(k => new HotelAttributeValueCreateModels() {
                            HotelFid = hotelId,
                            AttributeFid = k.Split("_")[0].ToInt32(),
                            AttributeValue = k.Split("_")[1]
                        }).ToList();

                        var createAttributeResult = _hotelAttributeValueService.CreateRangeAsync(listAttributetoCreates).Result;


                        transaction.Commit();
                        return BaseResponse<bool>.Success(true);
                    }

                    transaction.Rollback();
                    return BaseResponse<bool>.BadRequest();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return BaseResponse<bool>.InternalServerError(ex);
                }
            }
        }
    }
}