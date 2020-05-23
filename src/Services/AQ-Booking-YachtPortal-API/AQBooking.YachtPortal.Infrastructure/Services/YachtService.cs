using APIHelpers.Response;
using AQBooking.YachtPortal.Core.Enum;
using AQBooking.YachtPortal.Core.Helpers;
using AQBooking.YachtPortal.Core.Models.RedisCaches;
using AQBooking.YachtPortal.Core.Models.YachtCharterings;
using AQBooking.YachtPortal.Core.Models.YachtCounters;
using AQBooking.YachtPortal.Core.Models.YachtMerchantProductInventories;
using AQBooking.YachtPortal.Core.Models.YachtOptions;
using AQBooking.YachtPortal.Core.Models.YachtPricingPlanDetails;
using AQBooking.YachtPortal.Core.Models.Yachts;
using AQBooking.YachtPortal.Core.Models.Yachts.StoreProcedure;
using AQBooking.YachtPortal.Infrastructure.Entities;
using AQBooking.YachtPortal.Infrastructure.Helpers;
using AQBooking.YachtPortal.Infrastructure.Interfaces;
using AQEncrypts;
using AutoMapper;
using ExtendedUtility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace AQBooking.YachtPortal.Infrastructure.Services
{
    public class YachtService : IYachtService
    {
        private readonly AQYachtContext _AQYachtContext;
        private readonly IMapper _mapper;
        private readonly IYachtPricingPlanDetailService _yachtPricingPlanDetailService;
        private readonly IYachtMerchantProductInventoryService _yachtMerchantProductInventoryService;
        private DateTime _NOW_DATE { get => DateTime.Now; }
        private readonly int DEFAULT_LANGUAGE_CODE = 1;
        private readonly int FILESTREAM_IMAGE_ID = 4;
        private readonly IDistributedCache _distributedCache;

        public YachtService(
            AQYachtContext searchContext,
            IMapper mapper,
            IYachtPricingPlanDetailService yachtPricingPlanDetailService,
            IYachtMerchantProductInventoryService yachtMerchantProductInventoryService,
            IDistributedCache distributedCache)
        {
            _AQYachtContext = searchContext;
            _mapper = mapper;
            _yachtPricingPlanDetailService = yachtPricingPlanDetailService;
            _yachtMerchantProductInventoryService = yachtMerchantProductInventoryService;
            _distributedCache = distributedCache;
        }
        #region Private Method

        private Yachts FindYacht(int id)
        {
            return _AQYachtContext.Yachts.Find(id);
        }

        private YachtCounterViewModel LoadCounter(Yachts yatch)
        {
            _AQYachtContext.YachtCounters
                .Where(k => k.YachtId == yatch.Id)
                .Load();

            var model = new YachtCounterViewModel();
            model.InjectFrom(yatch.Counter);
            return model;
        }

        private void IncreaseYachtTotalViews(int yachtId, int topupValue = 1)
        {
            var yacht = _AQYachtContext.Yachts.FirstOrDefault(k => !k.Deleted && k.Id == yachtId);
            if (yacht != null)
            {
                CreateCounterIfNotExisted(yacht);
                LoadCounter(yacht);
                yacht.Counter.TotalViews += topupValue;
                _AQYachtContext.SaveChanges();
            }
        }

        private bool CreateCounterIfNotExisted(Yachts yacht)
        {
            try
            {
                var isExistedCounter = _AQYachtContext.YachtCounters.Any(k => k.YachtId == yacht.Id);
                if (isExistedCounter)
                    return true;
                var counter = new YachtCounters();
                counter.YachtId = yacht.Id;
                counter.YachtUniqueId = yacht.UniqueId;
                _AQYachtContext.YachtCounters.Add(counter);
                var save = _AQYachtContext.SaveChanges();
                return save > 0;
            }
            catch
            {
                return false;
            }
        }

        #endregion Private Method

        
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public YachtPricingPlanDetailsResult PricingPlanDetailsConvertFromJson(string strJson)
        {
            try
            {
                if (string.IsNullOrEmpty(strJson))
                {
                    return null;
                }
                else
                {
                    return JsonConvert.DeserializeObject<YachtPricingPlanDetailsResult>(strJson);
                }
            }
            catch
            {
                return null;
            }
        }
        public BaseResponse<List<YachtDetailModel>> GetListYacht()
        {
            try
            {
                var result = _AQYachtContext.Yachts.AsNoTracking().ToList();//.Select(x => _mapper.Map<YachtViewModel>(x)).ToList();

                var res = (from y in _AQYachtContext.Yachts.AsNoTracking()
                           select _mapper.Map<Yachts, YachtDetailModel>(y)).ToList();

                if (result != null && result.Count() > 0)
                    return BaseResponse<List<YachtDetailModel>>.Success(res);
                else
                    return BaseResponse<List<YachtDetailModel>>.NoContent();
            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtDetailModel>>.InternalServerError(null, ex.Message);
            }
        }
        public BaseResponse<List<YachtOptionExclusiveViewModel>> GetAllMerchantYacht(int merchantId)
        {
            try
            {
                var result = _AQYachtContext.Yachts.Where(k => k.MerchantFid == merchantId && k.ActiveForOperation && k.Deleted == false).Select(k => new YachtOptionExclusiveViewModel()
                {
                    Id = Terminator.Encrypt(k.Id.ToString()),
                    UniqueId = k.UniqueId,
                    MerchantFid = Terminator.Encrypt(k.MerchantFid.ToString()),
                    YachtFileStreamId = k.FileStreams.Where(c => c.YachtFid == k.Id && (c.FileTypeFid == 4 || c.FileTypeFid == 5) && c.Deleted == false && c.ActivatedDate <= DateTime.Now).OrderByDescending(d => d.ActivatedDate).FirstOrDefault().FileStreamFid,
                    LandingPageOptionFid = k.Merchant.LandingPageOptionFid,
                    MerchantFileStreamId = k.Merchant.FileStreams.Where(c => k.MerchantFid == k.MerchantFid && k.Deleted == false && c.ActivatedDate <= DateTime.Now).OrderByDescending(d => d.ActivatedDate).FirstOrDefault().FileStreamFid,
                    MerchantName = k.Merchant.MerchantName,
                    Name = k.Name,
                    City = k.City,
                    Country = k.Country,
                    YachtTypeResKey = k.YachtTypeResKey
                });


                if (result != null)
                    return BaseResponse<List<YachtOptionExclusiveViewModel>>.Success(result.ToList());
                else
                    return BaseResponse<List<YachtOptionExclusiveViewModel>>.NoContent();
            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtOptionExclusiveViewModel>>.InternalServerError(null, ex.Message);
            }
        }

        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public BaseResponse<PagedList<YachtViewModel>> GetYachtsByMerchantFId(SearchYachtWithMerchantIdModel searchModel)
        {
            try
            {
                var sortString = !string.IsNullOrEmpty(searchModel.SortString)
                ? searchModel.SortString
                : "Name DESC";

                searchModel.PageIndex = searchModel.PageIndex > 0 ? searchModel.PageIndex : 1;
                searchModel.PageSize = searchModel.PageSize > 0 ? searchModel.PageSize : 10;

                //var IdDecrypted = Terminator.Decrypt(searchModel.MerchantId);

                //int mechantId = Convert.ToInt32(IdDecrypted);
                var query = _AQYachtContext.Yachts
                           .Where(x => x.Deleted == false
                                   && x.ActiveForOperation == true
                                   && x.MerchantFid == searchModel.MerchantId.ToInt32())

                           .Select(k => new YachtViewModel
                           {
                               Country = k.Country,
                               ID = Terminator.Encrypt(k.Id.ToString()),
                               Name = k.Name,
                               LengthMeters = k.LengthMeters.GetValueOrDefault(),
                               Cabins = k.Cabins,
                               City = k.City,
                               MaxPassenger = k.MaxPassengers,
                               MaxSpeed = k.MaxSpeed.GetValueOrDefault(),
                               EngineGenerators = k.EngineGenerators,
                               CharterTypeFid = k.CharterTypeFid,
                               CharterTypeReskey = k.CharterCategoryResKey,
                               CharterCategoryFid = k.CharterCategoryFid,
                               PricingPlanDetailJson = PricingPlanDetailsConvertFromJson(_AQYachtContext.GetfnYachtPricingPlanDetailVal(k.Id, 8)),
                               YachtFileStreamId = k.FileStreams.Where(c => c.YachtFid == k.Id && (c.FileTypeFid == 4 || c.FileTypeFid == 5) && c.Deleted == false && c.ActivatedDate <= DateTime.Now).OrderByDescending(d => d.ActivatedDate).FirstOrDefault().FileStreamFid
                           });
                return BaseResponse<PagedList<YachtViewModel>>.Success(new PagedList<YachtViewModel>(query, searchModel.PageIndex, searchModel.PageSize));
            }
            catch (Exception ex)
            {
                return BaseResponse<PagedList<YachtViewModel>>.InternalServerError(new PagedList<YachtViewModel>(Enumerable.Empty<YachtViewModel>().AsQueryable(), searchModel.PageIndex, searchModel.PageSize), message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        /// <summary>
        /// SEARCH MODEL
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public BaseResponse<List<string>> GetListYachtName(YachtSearchModel searchModel)
        {
            try
            {
                if (searchModel == null)
                    return BaseResponse<List<string>>.BadRequest();

                var sortString = !string.IsNullOrEmpty(searchModel.SortString) ? searchModel.SortString : "Name ASC";
                searchModel.PageIndex = searchModel.PageIndex > 0 ? searchModel.PageIndex : 1;
                searchModel.PageSize = searchModel.PageSize > 0 ? searchModel.PageSize : 10;
                var query = _AQYachtContext.Yachts
                            .Where(x => x.Deleted == false && x.ActiveForOperation == true
                                  && (searchModel.City == null || searchModel.City == "" || x.City == searchModel.City)
                                            && (searchModel.YachtName == null || searchModel.YachtName == "" || x.Name.StartsWith(searchModel.YachtName))
                                            && (searchModel.CharterCategoryFID == 0 || (searchModel.CharterCategoryFID != 0 && x.CharterCategoryFid == searchModel.CharterCategoryFID))
                                            && (searchModel.CharterTypeFID == 0 || (searchModel.CharterTypeFID != 0 && x.CharterTypeFid == searchModel.CharterTypeFID))
                                            && (searchModel.YachtTypeFID == 0 || (searchModel.YachtTypeFID != 0 && x.YachtTypeFid == searchModel.YachtTypeFID))
                                            && (searchModel.HullTypeFID == 0 || (searchModel.HullTypeFID != 0 && x.HullTypeFid == searchModel.HullTypeFID))

                                              && (
                                                      (
                                                        searchModel.PortFID == 0 && x.Ports.Any(k => k.Deleted == false && k.EffectiveDate <= DateTime.Now && k.IsActivated == true)
                                                      )
                                                  || (searchModel.PortFID != 0 && x.Ports.Any(k => k.PortFid == searchModel.PortFID && k.Deleted == false && k.EffectiveDate <= DateTime.Now && k.IsActivated == true))
                                                )

                                              && (searchModel.Passengers == 0 || x.MaxPassengers >= searchModel.Passengers)
                                              && (searchModel.Cabins == 0 || x.Cabins >= searchModel.Cabins)
                                   )
                            .OrderBy(sortString)
                            .Select(k => k.Name)
                            .Skip(searchModel.PageSize * (searchModel.PageIndex - 1))
                            .Take(searchModel.PageSize)
                            .ToList();
                if (query != null)
                    return BaseResponse<List<string>>.Success(query);

                return BaseResponse<List<string>>.NoContent(new List<string>());
            }
            catch (Exception ex)
            {
                return BaseResponse<List<string>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public BaseResponse<PagedList<YachtPrivateCharterViewModel>> PrivateCharterSearch(YachtSearchModel searchModel)
        {
            //searchModel.PortFID = 2011;
            try
            {
                DateTime checkinDate = DateTime.Parse(searchModel.CheckIn);
                DateTime checkoutDate = DateTime.Parse(searchModel.CheckOut);
                List<int> lstStatusId = GlobalMethod.GetYachtSoldOutStatus();

                if (searchModel == null)
                    return BaseResponse<PagedList<YachtPrivateCharterViewModel>>.NotFound(new PagedList<YachtPrivateCharterViewModel>(Enumerable.Empty<YachtPrivateCharterViewModel>().AsQueryable(), searchModel.PageIndex, searchModel.PageSize), message: "searchModel variable is null", fullMsg: "");

                var sortString = !string.IsNullOrEmpty(searchModel.SortString) ? searchModel.SortString : "Name DESC";

                searchModel.PageIndex = searchModel.PageIndex > 0 ? searchModel.PageIndex : 1;
                searchModel.PageSize = searchModel.PageSize > 0 ? searchModel.PageSize : 9;
                searchModel.PageSize = searchModel.PageSize == 10 ? 18 : searchModel.PageSize;
                var result = (from x in _AQYachtContext.Yachts
                              where x.Deleted == false && x.ActiveForOperation == true
                                            && (searchModel.City == null || searchModel.City == "" || x.City == searchModel.City)
                                            && (searchModel.YachtName == null || searchModel.YachtName == "" || x.Name.StartsWith(searchModel.YachtName))
                                            && (searchModel.CharterCategoryFID == 0 || (searchModel.CharterCategoryFID != 0 && x.CharterCategoryFid == searchModel.CharterCategoryFID))
                                            && (searchModel.CharterTypeFID == 0 || (searchModel.CharterTypeFID != 0 && x.CharterTypeFid == searchModel.CharterTypeFID))
                                            && (searchModel.YachtTypeFID == 0 || (searchModel.YachtTypeFID != 0 && x.YachtTypeFid == searchModel.YachtTypeFID))
                                            && (searchModel.HullTypeFID == 0 || (searchModel.HullTypeFID != 0 && x.HullTypeFid == searchModel.HullTypeFID))

                                              && (
                                                      (
                                                        searchModel.PortFID == 0 && x.Ports.Any(k => k.Deleted == false && k.EffectiveDate.Date <= DateTime.Now.Date && k.IsActivated == true)
                                                      )
                                                  || (searchModel.PortFID != 0 && x.Ports.Any(k => k.PortFid == searchModel.PortFID && k.Deleted == false && k.EffectiveDate <= DateTime.Now && k.IsActivated == true))
                                                )

                                              && (searchModel.Passengers == 0 || x.MaxPassengers >= searchModel.Passengers)
                                              && (searchModel.Cabins == 0 || x.Cabins >= searchModel.Cabins)
                              //&& (searchModel.LengthMin == 0 || x.LengthMeters.GetValueOrDefault() >= searchModel.LengthMin)
                              //&& (searchModel.LengthMax == 0 || x.LengthMeters.GetValueOrDefault() <= searchModel.LengthMax)
                              select new YachtPrivateCharterViewModel
                              {
                                  ID = Terminator.Encrypt(x.Id.ToString()),
                                  UniqueID = x.UniqueId,
                                  Name = x.Name,
                                  LengthMeters = x.LengthMeters.GetValueOrDefault(),
                                  Cabins = x.Cabins,
                                  MaxPassenger = x.MaxPassengers,
                                  MaxSpeed = x.MaxSpeed.GetValueOrDefault(),
                                  CharterTypeFid = x.CharterTypeFid,
                                  CharterTypeReskey = x.CharterCategoryResKey,
                                  EngineGenerators = x.EngineGenerators,
                                  FileStreamFid = x.FileStreams.Where(k => k.FileTypeFid == (int)FileTypeEnum.RefImage && k.ActivatedDate <= DateTime.Now.Date).OrderByDescending(k => k.ActivatedDate).FirstOrDefault().FileStreamFid,

                                  CharteringFId = x.Charterings.Where(k => (lstStatusId == null || lstStatusId.Count > 0 || lstStatusId.Contains(k.StatusFid))
                                                                              && ((searchModel.CheckIn == "" && searchModel.CheckOut == "")
                                                                                               || (checkinDate >= k.CharterDateFrom && checkinDate <= k.CharterDateTo)
                                                                                               || (checkoutDate >= k.CharterDateFrom && checkoutDate <= k.CharterDateTo)
                                                                                               || (checkinDate <= k.CharterDateFrom && checkoutDate >= k.CharterDateTo)
                                                                                )

                                                                        ).FirstOrDefault().Id
                              }
                          );

                if (result != null)
                    return BaseResponse<PagedList<YachtPrivateCharterViewModel>>.Success(new PagedList<YachtPrivateCharterViewModel>(result, searchModel.PageIndex, searchModel.PageSize));
                else
                    return BaseResponse<PagedList<YachtPrivateCharterViewModel>>.NoContent();
            }
            catch (Exception ex)
            {
                return BaseResponse<PagedList<YachtPrivateCharterViewModel>>.InternalServerError(new PagedList<YachtPrivateCharterViewModel>(Enumerable.Empty<YachtPrivateCharterViewModel>().AsQueryable(), searchModel.PageIndex, searchModel.PageSize), message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public BaseResponse<YachtSingleViewModel> YachtFindingById(string yachtFId)
        {
            try
            {
                var yachtFIdde = Terminator.Decrypt(yachtFId).ToInt32();
                var result = _AQYachtContext.Yachts.Where(x => x.Deleted == false && x.ActiveForOperation == true
                && x.Id == yachtFIdde)
                .Select(x => _mapper.Map<Yachts, YachtSingleViewModel>(x))
                .FirstOrDefault();
                if (result != null)
                    IncreaseYachtTotalViews(yachtFIdde);

                if (result != null)
                    return BaseResponse<YachtSingleViewModel>.Success(result);
                else
                    return BaseResponse<YachtSingleViewModel>.NoContent();

            }
            catch (Exception ex)
            {
                return BaseResponse<YachtSingleViewModel>.InternalServerError(null, ex.Message);
            }
        }

        //*****modified tuantran 10-10-2019
        //*****next modified by 
        public BaseResponse<YachtSingleViewModel> YachtFindingByIdActiveFalse(string yachtFId)
        {
            try
            {
                var yachtFIdde = Terminator.Decrypt(yachtFId).ToInt32();
                var result = _AQYachtContext.Yachts.Where(x => x.Deleted == false
                && x.Id == yachtFIdde)
                .Select(x => _mapper.Map<Yachts, YachtSingleViewModel>(x))
                .FirstOrDefault();
                if (result != null)
                    IncreaseYachtTotalViews(yachtFIdde);

                if (result != null)
                    return BaseResponse<YachtSingleViewModel>.Success(result);
                else
                    return BaseResponse<YachtSingleViewModel>.NoContent();

            }
            catch (Exception ex)
            {
                return BaseResponse<YachtSingleViewModel>.InternalServerError(null, ex.Message);
            }
        }

        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public BaseResponse<List<YachtOptionExclusiveViewModel>> GetYachtAnyExclusive(YachtImageSlideSearchModel searchModel)
        {
            try
            {
                List<int> lstStatusId = GlobalMethod.GetYachtSoldOutStatus();
                DateTime checkinDate = DateTime.Now;
                DateTime checkoutDate = DateTime.Now;
                bool isCheckDate = false;
                if (!string.IsNullOrWhiteSpace(searchModel.CheckIn) && !string.IsNullOrWhiteSpace(searchModel.CheckOut))
                {
                    checkinDate = DateTime.Parse(searchModel.CheckIn);
                    checkoutDate = DateTime.Parse(searchModel.CheckOut);
                    isCheckDate = true;
                }
                var result = _AQYachtContext.Yachts.Where(k => k.Option.IsExclusiveYacht
                && k.ActiveForOperation == true
                && k.Deleted == false
                && (searchModel.CityName == null || searchModel.CityName == "" || k.City == searchModel.CityName)
                && (searchModel.PortId == 0 || k.Ports.Any(c => c.PortFid == searchModel.PortId))
                ).OrderBy(k => Guid.NewGuid()).Take(searchModel.showAmount).Select(k => new YachtOptionExclusiveViewModel()
                {
                    Id = Terminator.Encrypt(k.Id.ToString()),
                    UniqueId = k.UniqueId,
                    MerchantFid = Terminator.Encrypt(k.MerchantFid.ToString()),
                    YachtFileStreamId = k.FileStreams.Where(c => c.YachtFid == k.Id && c.FileCategoryFid == (int)FileCategoriesEnum.YachtPicture && (c.FileTypeFid == (int)FileTypeEnum.RefImage || c.FileTypeFid == (int)FileTypeEnum.Image) && c.Deleted == false && c.ActivatedDate <= DateTime.Now).OrderByDescending(d => d.ActivatedDate).FirstOrDefault().FileStreamFid,
                    LandingPageOptionFid = k.Merchant.LandingPageOptionFid,
                    MerchantFileStreamId = k.Merchant.FileStreams.Where(c => k.MerchantFid == k.MerchantFid && k.Deleted == false && c.ActivatedDate <= DateTime.Now).OrderByDescending(d => d.ActivatedDate).FirstOrDefault().FileStreamFid,
                    MerchantName = k.Merchant.MerchantName,
                    Name = k.Name,
                    City = k.City,
                    Country = k.Country,
                    YachtTypeResKey = k.YachtTypeResKey,
                    CharteringFId = k.Charterings.Where(x => (lstStatusId == null || lstStatusId.Count > 0 || lstStatusId.Contains(x.StatusFid))
                                                                             && (
                                                                                   isCheckDate == false
                                                                                    || (
                                                                                          isCheckDate == true
                                                                                           && (
                                                                                                  (checkinDate >= x.CharterDateFrom && checkinDate <= x.CharterDateTo)
                                                                                                  || (checkoutDate >= x.CharterDateFrom && checkoutDate <= x.CharterDateTo)
                                                                                                  || (checkinDate <= x.CharterDateFrom && checkoutDate >= x.CharterDateTo)
                                                                                             )
                                                                                       )
                                                                               )

                                                                        ).FirstOrDefault().Id
                });

                if (result != null)
                    return BaseResponse<List<YachtOptionExclusiveViewModel>>.Success(result.ToList());
                else
                    return BaseResponse<List<YachtOptionExclusiveViewModel>>.NoContent();

            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtOptionExclusiveViewModel>>.InternalServerError(null, ex.Message);
            }
        }

        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public BaseResponse<List<YachtOptionExclusiveViewModel>> GetYachtAnyNew(int showAmount)
        {
            try
            {
                var result = _AQYachtContext.Yachts.Where(k => k.ActiveForOperation == true && k.Deleted == false).OrderByDescending(k => k.CreatedDate).ThenBy(k => Guid.NewGuid()).Take(showAmount).Select(k => new YachtOptionExclusiveViewModel()
                {
                    Id = Terminator.Encrypt(k.Id.ToString()),
                    UniqueId = k.UniqueId,
                    MerchantFid = Terminator.Encrypt(k.MerchantFid.ToString()),
                    YachtFileStreamId = k.FileStreams.Where(c => c.YachtFid == k.Id && c.FileCategoryFid == (int)FileCategoriesEnum.YachtPicture && (c.FileTypeFid == (int)FileTypeEnum.RefImage || c.FileTypeFid == (int)FileTypeEnum.Image) && c.Deleted == false && c.ActivatedDate <= DateTime.Now).OrderByDescending(d => d.ActivatedDate).FirstOrDefault().FileStreamFid,
                    LandingPageOptionFid = k.Merchant.LandingPageOptionFid,
                    MerchantFileStreamId = k.Merchant.FileStreams.Where(c => k.MerchantFid == k.MerchantFid && k.Deleted == false && c.ActivatedDate <= DateTime.Now).OrderByDescending(d => d.ActivatedDate).FirstOrDefault().FileStreamFid,
                    MerchantName = k.Merchant.MerchantName,
                    Name = k.Name,
                    City = k.City,
                    Country = k.Country,
                    YachtTypeResKey = k.YachtTypeResKey
                });

                if (result != null)
                    return BaseResponse<List<YachtOptionExclusiveViewModel>>.Success(result.ToList());
                else
                    return BaseResponse<List<YachtOptionExclusiveViewModel>>.NoContent();

            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtOptionExclusiveViewModel>>.InternalServerError(null, ex.Message);
            }
        }

        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public BaseResponse<List<YachtOptionExclusiveViewModel>> GetYachtAnyPromotion(int showAmount)
        {
            try
            {

                var result = _AQYachtContext.Yachts.Where(k => k.ActiveForOperation == true && k.Deleted == false)
                    .OrderBy(k => Guid.NewGuid()).Take(showAmount)
                    .Select(k => new YachtOptionExclusiveViewModel()
                    {
                        Id = Terminator.Encrypt(k.Id.ToString()),
                        UniqueId = k.UniqueId,
                        MerchantFid = Terminator.Encrypt(k.MerchantFid.ToString()),
                        YachtFileStreamId = k.FileStreams.Where(c => c.YachtFid == k.Id && c.FileCategoryFid == (int)FileCategoriesEnum.YachtPicture && (c.FileTypeFid == (int)FileTypeEnum.RefImage || c.FileTypeFid == (int)FileTypeEnum.Image) && c.Deleted == false && c.ActivatedDate <= DateTime.Now).OrderByDescending(d => d.ActivatedDate).FirstOrDefault().FileStreamFid,
                        LandingPageOptionFid = k.Merchant.LandingPageOptionFid,
                        MerchantFileStreamId = k.Merchant.FileStreams.Where(c => k.MerchantFid == k.MerchantFid && k.Deleted == false && c.ActivatedDate <= DateTime.Now).OrderByDescending(d => d.ActivatedDate).FirstOrDefault().FileStreamFid,
                        MerchantName = k.Merchant.MerchantName,
                        Name = k.Name,
                        City = k.City,
                        Country = k.Country,
                        YachtTypeResKey = k.YachtTypeResKey

                    });
                if (result != null)
                    return BaseResponse<List<YachtOptionExclusiveViewModel>>.Success(result.ToList());
                else
                    return BaseResponse<List<YachtOptionExclusiveViewModel>>.NoContent();

            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtOptionExclusiveViewModel>>.InternalServerError(null, ex.Message);
            }
        }

        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        //***** using for Profile Booking Detail page
        public BaseResponse<PagedList<YachtCharterBookingViewModel>> GetBookingList(YachtbookingRequestModel requestModel)
        {
            List<YachtCharterBookingViewModel> responseCharteringDl = new List<YachtCharterBookingViewModel>();
            try
            {
                // sort expression
                Expression<Func<YachtCharterings, object>> sortExpression;
                switch (requestModel.SortColumn)
                {
                    case "BookingDate":
                        sortExpression = (x => x.BookingDate);
                        break;
                    case "CharterDateFrom":
                        sortExpression = (x => x.CharterDateFrom);
                        break;
                    default:
                        sortExpression = (x => x.BookingDate);
                        break;
                }

                var result = _AQYachtContext.YachtCharterings
                    .Where(k => k.CustomerFid == new Guid(requestModel.CustomerId)
                        && (requestModel.statusId == null || (requestModel.statusId != null && requestModel.statusId.Count > 0 && requestModel.statusId.Contains(k.StatusFid)))
                        && (requestModel.YachtPortId == 0 || k.YachtPortFid == requestModel.YachtPortId)
                    )
                    .OrderByDescending(sortExpression)
                    .Select(x => new YachtCharterBookingViewModel
                    {
                        CharteringID = Terminator.Encrypt(x.Id.ToString()),
                        YachtFId = Terminator.Encrypt(x.YachtFid.ToString()),
                        UniqueID = x.UniqueId,
                        CharterDateFrom = x.CharterDateFrom,
                        ContactNo = x.ContactNo,
                        CharterDateTo = x.CharterDateTo,
                        YachtName = x.Yacht.Name,
                        MaxPassenger = x.Yacht.MaxPassengers,
                        EngineGenerators = x.Yacht.EngineGenerators,
                        Cabins = x.Yacht.Cabins,
                        LengthMeters = x.Yacht.LengthMeters.GetValueOrDefault(),
                        StatusID = x.StatusFid,
                        CultureCode = x.CultureCode,
                        CurrencyCode = x.CurrencyCode,
                        StatusResourceKey = x.StatusResKey,
                        PortName = x.YachtPortName,
                        PortId = x.YachtPortFid,
                        PrepaidValue = x.PrepaidValue,
                        GrandTotalValue = x.GrandTotalValue,
                        CustomerNote = x.CustomerNote,
                        CustomerName = x.CustomerName,
                        CustomerEmail = x.ReservationEmail,
                        BookingDate = x.BookingDate,
                        streamFileId = _AQYachtContext.GetfnYachtImageIDVal(x.YachtFid, 4) ?? 0
                    });
                if (result != null)
                    return BaseResponse<PagedList<YachtCharterBookingViewModel>>.Success(new PagedList<YachtCharterBookingViewModel>(result, requestModel.PageIndex, requestModel.PageSize));
                else
                    return BaseResponse<PagedList<YachtCharterBookingViewModel>>.NoContent();
            }
            catch (Exception ex)
            {
                return BaseResponse<PagedList<YachtCharterBookingViewModel>>.InternalServerError(new PagedList<YachtCharterBookingViewModel>(Enumerable.Empty<YachtCharterBookingViewModel>().AsQueryable(), requestModel.PageIndex, requestModel.PageSize), message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        public BaseResponse<YachtCharterBookingViewModel> GetBookingByCharteringUniqueId(string uniqueId)
        {
            try
            {
                var result = _AQYachtContext.YachtCharterings.Where(k =>
                                                                      k.UniqueId == uniqueId
                                                                    )
                                                        .Select(x => new YachtCharterBookingViewModel
                                                        {
                                                            CharteringID = Terminator.Encrypt(x.Id.ToString()),
                                                            YachtFId = Terminator.Encrypt(x.YachtFid.ToString()),
                                                            UniqueID = x.UniqueId,
                                                            CharterDateFrom = x.CharterDateFrom,
                                                            ContactNo = x.ContactNo,
                                                            CharterDateTo = x.CharterDateTo,
                                                            YachtName = x.Yacht.Name,
                                                            MaxPassenger = x.Yacht.MaxPassengers,
                                                            EngineGenerators = x.Yacht.EngineGenerators,
                                                            Cabins = x.Yacht.Cabins,
                                                            LengthMeters = x.Yacht.LengthMeters.GetValueOrDefault(),
                                                            StatusID = x.StatusFid,
                                                            CultureCode = x.CultureCode,
                                                            CurrencyCode = x.CurrencyCode,
                                                            StatusResourceKey = x.StatusResKey,
                                                            PortName = x.YachtPortName,
                                                            PortId = x.YachtPortFid,
                                                            PrepaidValue = x.PrepaidValue,
                                                            GrandTotalValue = x.GrandTotalValue,
                                                            CustomerNote = x.CustomerNote,
                                                            CustomerName = x.CustomerName,
                                                            CustomerEmail = x.ReservationEmail,
                                                            BookingDate = x.BookingDate,
                                                            streamFileId = _AQYachtContext.GetfnYachtImageIDVal(x.YachtFid, 4) ?? 0
                                                        }).OrderByDescending(o => o.PortId).ThenByDescending(o => o.CharterDateFrom);
                if (result != null)
                    return BaseResponse<YachtCharterBookingViewModel>.Success(result.FirstOrDefault());
                else
                    return BaseResponse<YachtCharterBookingViewModel>.NoContent();
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtCharterBookingViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        //*****modified by hoangle 10-10-2019 
        //*****next modified by 
        //***** using for Profile Booking List page
        public BaseResponse<YachtCharterBookingViewModel> GetBookingDetail(string charteringId)
        {
            YachtCharterBookingViewModel responseCharteringDl = new YachtCharterBookingViewModel();
            try
            {
                var Id = Terminator.Decrypt(charteringId.ToString()).ToDouble();
                var result = _AQYachtContext.YachtCharterings.Where(
                                                                        k => k.Id == Id
                                                                  )

                                                        .Select(x => new YachtCharterBookingViewModel
                                                        {
                                                            CharteringID = Terminator.Encrypt(x.Id.ToString()),
                                                            YachtFId = Terminator.Encrypt(x.YachtFid.ToString()),
                                                            CharterDateFrom = x.CharterDateFrom,
                                                            ContactNo = x.ContactNo,
                                                            CharterDateTo = x.CharterDateTo,
                                                            YachtName = x.Yacht.Name,
                                                            MaxPassenger = x.Yacht.MaxPassengers,
                                                            EngineGenerators = x.Yacht.EngineGenerators,
                                                            Cabins = x.Yacht.Cabins,
                                                            LengthMeters = x.Yacht.LengthMeters.GetValueOrDefault(),
                                                            StatusID = x.StatusFid,
                                                            CultureCode = x.CultureCode,
                                                            CurrencyCode = x.CurrencyCode,
                                                            StatusResourceKey = x.StatusResKey,
                                                            PortName = x.YachtPortName,
                                                            PortId = x.YachtPortFid,
                                                            PrepaidValue = x.PrepaidValue,
                                                            GrandTotalValue = x.GrandTotalValue,
                                                            CustomerNote = x.CustomerNote,
                                                            CustomerName = x.CustomerName,
                                                            CustomerEmail = x.ReservationEmail,
                                                            streamFileId = _AQYachtContext.GetfnYachtImageIDVal(x.YachtFid, 4) ?? 0,
                                                            Yacht = new YachtBookingViewModel
                                                            {
                                                                Name = x.Yacht.Name,
                                                                Country = x.Yacht.Country,
                                                                City = x.Yacht.City,
                                                                UniqueId = x.Yacht.UniqueId,
                                                                MerchantFid = Terminator.Encrypt(x.Yacht.MerchantFid.ToString()),
                                                                Id = Terminator.Encrypt(x.Yacht.Id.ToString())
                                                            }
                                                            ,
                                                            CharteringDetails = _mapper.Map<List<YachtCharteringDetails>, List<YachtCharteringDetailViewModel>>(x.CharteringDetails.Select(i => i).ToList())

                                                        });
                if (result != null)
                    return BaseResponse<YachtCharterBookingViewModel>.Success(result.FirstOrDefault());
                else
                    return BaseResponse<YachtCharterBookingViewModel>.NoContent();
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtCharterBookingViewModel>.InternalServerError(null, ex.Message);
            }
        }

        #region PAYMENT      

        //*****modified by hoangle 10-10-2019
        //*****next modified by 
        //*****using Yacht/Payment page--> PAYALE , PAYMENT STRIP function.
        public BaseResponse<SaveCharterPaymentResponseViewModel> SaveCharterPrivatePayment(YachtSavePackageServiceModel yachtPackageModel, string PaymentMethod)
        {
            var bookingRequestModel = yachtPackageModel.BookingRequestModel;
            var yachtBooking = yachtPackageModel.YachtBooking;

            #region initalize logging
            string errCode = "0";
            SaveCharterPaymentResponseViewModel errGlobal = new SaveCharterPaymentResponseViewModel();
            #endregion

            string dataSubjectLogging = "";
            try
            {
                #region logging Subject 
                dataSubjectLogging += "{YachtId:" + yachtBooking.YachtId + ",";
                dataSubjectLogging += "Passenger:" + yachtBooking.Passenger + ",";
                dataSubjectLogging += "CheckIn:" + yachtBooking.CheckIn + ",";
                dataSubjectLogging += "CheckOut:" + yachtBooking.CheckOut + ",";
                dataSubjectLogging += "}";

                errGlobal.Name = "CharterPrivatePaymentSave";
                errGlobal.Value = dataSubjectLogging;
                errGlobal.ResuldCode = errCode;
                errGlobal.Id = "0";
                errGlobal.UniqueId = "";
                #endregion

                #region CALCULATION
                MerchantPaymentPackageViewModel responsePackageModel = new MerchantPaymentPackageViewModel();
                List<MerchantPaymentEachPackageViewModel> lstProductInventories = new List<MerchantPaymentEachPackageViewModel>();

                #region YACHT               
                var yachtFIdde = Terminator.Decrypt(yachtBooking.YachtId).ToInt32();

                double dbYachtFee = 0;
                string yachtCultureCode = "";
                string yachtCurrencyCode = "";

                #region NUMBER OF DAY OR WEEK
                int bookingDayNumber = GlobalMethod.BookingDayNumber(yachtBooking.CheckIn, yachtBooking.CheckOut);

                #region GET PRICE
                var responsePricingPlanDetail = _yachtPricingPlanDetailService.GetPricingPlanDetailYachtFId(yachtBooking.YachtId);
                if (responsePricingPlanDetail != null && responsePricingPlanDetail.IsSuccessStatusCode && responsePricingPlanDetail.ResponseData != null)
                {
                    if (responsePricingPlanDetail.ResponseData.Details != null)
                    {
                        var priceDetail = responsePricingPlanDetail.ResponseData.Details.OrderByDescending(x => x.PricingTypeFid).FirstOrDefault();
                        if (priceDetail != null)
                        {
                            yachtCultureCode = priceDetail.CultureCode;
                            yachtCurrencyCode = priceDetail.CurrencyCode;
                        }
                        GlobalMethod.GetPriceRecuse(responsePricingPlanDetail.ResponseData.Details, bookingDayNumber, ref dbYachtFee);
                    }
                }

                #endregion

                responsePackageModel.Id = yachtBooking.YachtId;
                responsePackageModel.Passenger = yachtBooking.Passenger;
                responsePackageModel.YachtTotal = dbYachtFee;
                responsePackageModel.Total = dbYachtFee;
                responsePackageModel.PackageTotal = 0;
                responsePackageModel.DiscountTotal = 0;
                #endregion

                /**INSERT INTO CHARTERING**/

                #region YachtPort Infomation
                var responseYachtPort = (
                               from p in _AQYachtContext.YachtPorts
                                .Where(k => k.YachtFid == yachtFIdde
                                && k.Deleted == false
                                && k.EffectiveDate <= DateTime.Now.Date
                                && k.IsActivated == true
                                && k.EffectiveDate == _AQYachtContext.YachtPorts
                                   .Where(o => o.YachtFid == yachtFIdde
                                    && o.Deleted == false
                                    && o.EffectiveDate <= DateTime.Now.Date
                                    && o.IsActivated == true
                                   )
                                   .OrderByDescending(x => x.EffectiveDate)
                                   .Select(i => i.EffectiveDate).FirstOrDefault()
                                   ).DefaultIfEmpty().Take(1)

                               select p
                     ).FirstOrDefault();
                #endregion

                #region YachtInfomation
                bool isCrewmember = false;
                Yachts yachtOjb = _AQYachtContext.Yachts.FirstOrDefault(x => x.Id == yachtFIdde);
                if (yachtOjb != null)
                {
                    if (yachtOjb.CrewMembers > 0)
                    {
                        isCrewmember = true;
                    }
                }
                #endregion

                #region INSERT INTO CHARTERING
                YachtCharterings charteringModel = new YachtCharterings();
                charteringModel.YachtFid = yachtFIdde;

                charteringModel.SourceFid = 1;
                charteringModel.SourceResKey = "SOURCEAQBOOKINGS";
                charteringModel.UniqueId = UniqueIDHelper.GenarateRandomString(12);

                //customer
                charteringModel.CustomerName = bookingRequestModel.NameOfUser;
                charteringModel.ReservationEmail = bookingRequestModel.EmailOfUser;
                bool isUerExisting = false;
                if (bookingRequestModel.IsEmailExist != 0)
                {
                    isUerExisting = true;
                }
                charteringModel.IsExistingCustomer = isUerExisting;

                if (bookingRequestModel.IdOfUser.Trim() != "")
                {
                    charteringModel.CustomerFid = new Guid(bookingRequestModel.IdOfUser.Trim());
                }
                charteringModel.ContactNo = bookingRequestModel.ContactNo;
                charteringModel.Passengers = yachtBooking.Passenger;
                charteringModel.CharterDateFrom = DateTime.Now;
                charteringModel.CharterDateTo = DateTime.Now;
                charteringModel.BookingDate = DateTime.Now;

                //yacht port
                if (responseYachtPort != null)
                {
                    charteringModel.YachtPortFid = responseYachtPort.PortFid;
                    charteringModel.YachtPortName = responseYachtPort.PortName;
                }
                else
                {
                    charteringModel.YachtPortFid = -1;
                    charteringModel.YachtPortName = "";
                }


                charteringModel.HaveCrewsMember = isCrewmember;
                charteringModel.CultureCode = yachtCultureCode;
                charteringModel.CurrencyCode = yachtCurrencyCode;


                charteringModel.StatusFid = Convert.ToInt32(YachtCharterStatusEnum.Waiting);
                charteringModel.StatusResKey = "WAITINGPAYMENT";
                charteringModel.Processed = false;

                _AQYachtContext.YachtCharterings.Add(charteringModel);
                _AQYachtContext.SaveChanges();
                long charteringModelId = charteringModel.Id;
                #region logging
                errGlobal.Id = Terminator.Encrypt(charteringModelId.ToString());
                errGlobal.UniqueId = charteringModel.UniqueId;

                #endregion

                #endregion

                #endregion

                #region PACKAGE
                bool isPackageAddition = false;
                List<MerchantProductInventoriesModel> lstProductPackage = yachtBooking.ProductPackage;
                double dbPackageFee = 0;
                double dbTotalFinalValue = 0;
                double dbTotalGrandTotalValue = 0;
                double dbTotalDiscountPackage = 0;

                if (lstProductPackage != null)
                {
                    List<string> lstProductId = lstProductPackage.Select(x => x.productInventoryFId).ToList();
                    var responsePriceOfProduct = _yachtMerchantProductInventoryService.GetPriceOfProductInventoryByArrayOfProductId(lstProductId);
                    if (responsePriceOfProduct != null && responsePriceOfProduct.IsSuccessStatusCode && responsePriceOfProduct.ResponseData != null)
                    {

                        foreach (MerchantProductInventoriesModel proItem in lstProductPackage)
                        {
                            #region logging Detail
                            SaveCharterPaymentDetailViewModel errDetail = new SaveCharterPaymentDetailViewModel();

                            string dataSubLogging = "";
                            dataSubLogging += "{ProductInventoryFId:" + proItem.productInventoryFId + ",";
                            dataSubLogging += "{Quantity:" + proItem.quantity + "}";
                            errDetail.Value = dataSubLogging;
                            #endregion
                            try
                            {
                                YachtMerchantProductInventoriesWithPriceViewModel reponseProduct = responsePriceOfProduct.ResponseData.FirstOrDefault(x => x.Id.Trim() == proItem.productInventoryFId.Trim());
                                if (reponseProduct != null)
                                {
                                    MerchantPaymentEachPackageViewModel objPackageWithPrice = new MerchantPaymentEachPackageViewModel();

                                    /**INSERT INTO  CHARTERINGDETAIL**/
                                    #region INSERT INTO  CHARTERINGDETAIL
                                    YachtCharteringDetails charteringDetailModel = new YachtCharteringDetails();
                                    charteringDetailModel.CharteringFid = charteringModelId;
                                    charteringDetailModel.YachtFid = yachtFIdde;
                                    charteringDetailModel.ItemTypeFid = 1;
                                    charteringDetailModel.ItemTypeResKey = "VENDORSERVICES";

                                    charteringDetailModel.RefFid = Terminator.Decrypt(reponseProduct.Id).ToInt32();


                                    charteringDetailModel.ItemName = reponseProduct.ProductName;
                                    charteringDetailModel.CultureCode = reponseProduct.CultureCode;
                                    charteringDetailModel.CurrencyCode = reponseProduct.CurrencyCode;
                                    charteringDetailModel.OrderAmount = proItem.quantity;
                                    charteringDetailModel.DiscountedValue = 0;
                                    charteringDetailModel.OriginalValue = reponseProduct.Price;
                                    charteringDetailModel.FinalValue = reponseProduct.Price - charteringDetailModel.DiscountedValue;
                                    charteringDetailModel.GrandTotalValue = GlobalMethod.PackageTotal(charteringDetailModel.FinalValue, proItem.quantity);
                                    // charteringDetailModel.FinalValue = charteringDetailModel.GrandTotalValue - charteringDetailModel.DiscountedValue;

                                    _AQYachtContext.YachtCharteringDetails.Add(charteringDetailModel);

                                    _AQYachtContext.SaveChanges();
                                    #endregion
                                    dbTotalFinalValue += charteringDetailModel.FinalValue;
                                    dbTotalGrandTotalValue += charteringDetailModel.GrandTotalValue;
                                    dbPackageFee += GlobalMethod.PackageTotal(charteringDetailModel.OriginalValue, proItem.quantity);
                                    dbTotalDiscountPackage += GlobalMethod.PackageTotal(charteringDetailModel.DiscountedValue, proItem.quantity);
                                    isPackageAddition = true;
                                }
                            }
                            catch
                            {
                                #region logging add detail to errGlobal variable
                                errGlobal.Detail.Add(errDetail);
                                #endregion
                            }
                        }
                        responsePackageModel.PackageTotal = dbPackageFee;
                        responsePackageModel.DiscountTotal = responsePackageModel.DiscountTotal + dbTotalDiscountPackage;

                        responsePackageModel.Total = (responsePackageModel.PackageTotal + responsePackageModel.Total);

                        responsePackageModel.lstPaymentPackage = lstProductInventories;

                    }
                }
                #endregion

                responsePackageModel.PrePaidRate = 0.5;
                responsePackageModel.PrepaidValue = responsePackageModel.PrePaidRate * responsePackageModel.Total;

                var newChartering = _AQYachtContext.YachtCharterings.FirstOrDefault(x => x.Id == charteringModelId);

                if (newChartering != null)
                {
                    newChartering.HaveAdditionalServices = isPackageAddition;
                    newChartering.PrepaidRate = responsePackageModel.PrePaidRate;
                    newChartering.PrepaidValue = responsePackageModel.PrepaidValue;
                    newChartering.GrandTotalValue = responsePackageModel.Total - responsePackageModel.DiscountTotal;
                    newChartering.DiscountedValue = responsePackageModel.DiscountTotal;
                    newChartering.OriginalValue = responsePackageModel.Total;

                    _AQYachtContext.SaveChanges();
                }
                YachtCharteringPaymentLogs paymentLogs = new YachtCharteringPaymentLogs();
                paymentLogs.CharteringFid = charteringModelId;

                //call api payment from Mr Long
                paymentLogs.PaymentBy = "";
                paymentLogs.PaymentRef = "";
                paymentLogs.PaymentMethod = PaymentMethod;

                paymentLogs.PaymentDate = DateTime.Now;
                paymentLogs.PaymentAmount = responsePackageModel.PrepaidValue;
                paymentLogs.CultureCode = yachtCultureCode;
                paymentLogs.CurrencyCode = yachtCurrencyCode;
                paymentLogs.StatusFid = Convert.ToInt32(YachtCharterStatusEnum.Waiting);//wating for payment
                _AQYachtContext.YachtCharteringPaymentLogs.Add(paymentLogs);
                _AQYachtContext.SaveChanges();
                #endregion

                #region logging  
                errCode = "1";
                errGlobal.ResuldCode = errCode;
                #endregion
                return BaseResponse<SaveCharterPaymentResponseViewModel>.Success(errGlobal);
            }
            catch (Exception ex)
            {
                errCode = "-1";
                errGlobal.ResuldCode = errCode;
                errGlobal.Describes = ex.Message.ToString();
                DebugHelper.LogBug("SaveCharterPrivatePayment", dataSubjectLogging);
                return BaseResponse<SaveCharterPaymentResponseViewModel>.InternalServerError(errGlobal, ex.Message);
            }
        }
        #endregion

        #region REDIS CACHE

        //*****modified by hoangle 27-11-2019
        //*****next modified by 
        public BaseResponse<List<YachtPackageViewModel>> GetRedisCartStorage(BookingRequestModel requestModel)
        {
            List<YachtPackageViewModel> lstPriceResult = new List<YachtPackageViewModel>();
            string Error = "0";
            try
            {
                var value = _distributedCache.GetString(requestModel.Key);
                if (value != null)
                {
                    var lstRedisStorage = JsonConvert.DeserializeObject<List<RedisStorage>>(value);

                    //requestModel.HashKey ==> Dining or Yacht or other...
                    var RedisStorageModel = lstRedisStorage.FirstOrDefault(k => k.Domain == requestModel.HashKey);
                    #region DOMAIN
                    if (RedisStorageModel != null)// DOMAIN IS  EXIST
                    {
                        var result = RedisStorageModel.PackageStorage;
                        if (result != null)
                        {
                            List<YachtPackageServiceModel> lstYachtPackage = result.Where(x => requestModel.itemList.Contains(x.YachtId)).Select(i => i).ToList();

                            if (lstYachtPackage != null)
                            {

                                ///Foreach Yacht choosed
                                foreach (YachtPackageServiceModel yachtItem in lstYachtPackage)
                                {
                                    YachtPackageViewModel modelView = new YachtPackageViewModel();
                                    var yachtFIdde = Terminator.Decrypt(yachtItem.YachtId).ToInt32();

                                    var yachtRs = _AQYachtContext.Yachts.Where(x => x.Deleted == false
                                      && x.ActiveForOperation == true
                                      && x.Id == yachtFIdde
                                    ).Select(x => new YachtCartInfo { Name = x.Name, FileStreamId = x.FileStreams.Where(k => k.FileTypeFid == (int)FileTypeEnum.Image || k.FileTypeFid == (int)FileTypeEnum.RefImage && k.ActivatedDate <= DateTime.Now.Date).OrderByDescending(k => k.ActivatedDate).FirstOrDefault().FileStreamFid }).FirstOrDefault();
                                    if (yachtRs != null)
                                    {
                                        modelView.YachtName = yachtRs.Name;

                                        modelView.FileStreamFid = yachtRs.FileStreamId;
                                    }
                                    else
                                    {
                                        Error = "-3"; // Yacht Was not Found
                                    }
                                    modelView.YachtId = yachtItem.YachtId;
                                    modelView.Passenger = yachtItem.Passenger;
                                    modelView.CheckIn = yachtItem.CheckIn;
                                    modelView.CheckOut = yachtItem.CheckOut;
                                    double dbPrePaidRate = 0.5;
                                    double dbYachtFee = 0;
                                    string yachtCurrencyCode = "";
                                    string yachtCultureCode = "";
                                    int bookingDayNumber = GlobalMethod.BookingDayNumber(modelView.CheckIn, modelView.CheckOut);

                                    #region GET PRICE
                                    //calling Api
                                    var responsePricingPlanDetail = _yachtPricingPlanDetailService.GetPricingPlanDetailYachtFId(yachtItem.YachtId);
                                    if (responsePricingPlanDetail != null && responsePricingPlanDetail.IsSuccessStatusCode && responsePricingPlanDetail.ResponseData != null)
                                    {
                                        if (responsePricingPlanDetail.ResponseData.Details != null)
                                        {
                                            var priceDetail = responsePricingPlanDetail.ResponseData.Details.OrderByDescending(x => x.PricingTypeFid).FirstOrDefault();
                                            if (priceDetail != null)
                                            {
                                                yachtCultureCode = priceDetail.CultureCode;
                                                yachtCurrencyCode = priceDetail.CurrencyCode;
                                            }
                                            GlobalMethod.GetPriceRecuse(responsePricingPlanDetail.ResponseData.Details, bookingDayNumber, ref dbYachtFee);
                                        }
                                    }

                                    #endregion

                                    modelView.CurrencyCode = yachtCurrencyCode;
                                    modelView.CultureCode = yachtCultureCode;
                                    modelView.YachtFee = dbYachtFee;
                                    modelView.TotalFee = modelView.YachtFee;
                                    double dbPackageFee = 0;
                                    var lstProductPackage = yachtItem.ProductPackage;
                                    string detailError = "";
                                    if (lstProductPackage != null)
                                    {
                                        List<string> lstProductId = lstProductPackage.Select(x => x.productInventoryFId).ToList();
                                        var responsePriceOfProduct = _yachtMerchantProductInventoryService.GetPriceOfProductInventoryByArrayOfProductId(lstProductId);
                                        if (responsePriceOfProduct != null && responsePriceOfProduct.IsSuccessStatusCode && responsePriceOfProduct.ResponseData != null)
                                        {
                                            foreach (MerchantProductInventoriesModel proItem in lstProductPackage)
                                            {
                                                try
                                                {
                                                    YachtMerchantProductInventoriesWithPriceViewModel reponseProduct = responsePriceOfProduct.ResponseData.FirstOrDefault(x => x.Id.Trim() == proItem.productInventoryFId.Trim());
                                                    if (reponseProduct != null)
                                                    {
                                                        dbPackageFee += GlobalMethod.PackageTotal(reponseProduct.Price, proItem.quantity);
                                                    }
                                                }
                                                catch
                                                {
                                                    #region logging add detail to errGlobal variable    
                                                    if (detailError != "")
                                                    {
                                                        detailError += ",";
                                                    }
                                                    detailError += $"productInventoryFId:{proItem.productInventoryFId}";
                                                    #endregion
                                                }
                                            }
                                        }
                                    }
                                    modelView.TotalFee = modelView.TotalFee + dbPackageFee;
                                    modelView.Prepaid = dbPrePaidRate * modelView.TotalFee;
                                    if (modelView.TotalFee == 0)
                                    {
                                        modelView.DisplayPackageFee = "0";
                                        modelView.DisplayYachtFee = "0";
                                        modelView.DisplayTotalFee = "0";
                                        modelView.DisplayPrepaid = "0";
                                    }
                                    if (modelView.TotalFee > 0)
                                    {
                                        modelView.DisplayPackageFee = modelView.PackageFee.ToCurrencyText(modelView.CultureCode);
                                        modelView.DisplayYachtFee = modelView.YachtFee.ToCurrencyText(modelView.CultureCode);
                                        modelView.DisplayTotalFee = modelView.TotalFee.ToCurrencyText(modelView.CultureCode);
                                        modelView.DisplayPrepaid = modelView.Prepaid.ToCurrencyText(modelView.CultureCode);
                                    }
                                    modelView.ErrorCode = "{YACHT_ERROR:" + Error + "\",DETAIL:{\"" + detailError + "\"}}";
                                    lstPriceResult.Add(modelView);
                                }
                            }
                        }
                    }
                    #endregion
                }
                return BaseResponse<List<YachtPackageViewModel>>.Success(lstPriceResult);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtPackageViewModel>>.InternalServerError(lstPriceResult, message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        //*****modified by hoangle 27-11-2019
        //*****next modified by 
        public BaseResponse<BookingTotalFee> GetRedisCartStorageTotalFee(BookingRequestModel requestModel)
        {
            BookingTotalFee TotalFeeModel = new BookingTotalFee();
            try
            {
                var value = _distributedCache.GetString(requestModel.Key);
                if (value != null)
                {
                    var lstRedisStorage = JsonConvert.DeserializeObject<List<RedisStorage>>(value);

                    //requestModel.HashKey ==> Dining or Yacht or other...
                    var RedisStorageModel = lstRedisStorage.FirstOrDefault(k => k.Domain == requestModel.HashKey);
                    #region DOMAIN
                    if (RedisStorageModel != null)// DOMAIN IS  EXIST
                    {
                        var result = RedisStorageModel.PackageStorage;
                        if (result != null)
                        {
                            List<YachtPackageServiceModel> lstYachtPackage = result.Where(x => requestModel.itemList.Contains(x.YachtId)).Select(i => i).ToList();

                            if (lstYachtPackage != null)
                            {
                                double dbPrePaidRate = 0.5;
                                double dbYachtFee = 0;
                                string yachtCurrencyCode = "";
                                string yachtCultureCode = "";
                                double dbPackageFee = 0;

                                ///Foreach Yacht choosed
                                foreach (YachtPackageServiceModel yachtItem in lstYachtPackage)
                                {
                                    int bookingDayNumber = GlobalMethod.BookingDayNumber(yachtItem.CheckIn, yachtItem.CheckOut);

                                    #region GET PRICE
                                    double dbYachtFeeEach = 0;
                                    //calling Api
                                    var responsePricingPlanDetail = _yachtPricingPlanDetailService.GetPricingPlanDetailYachtFId(yachtItem.YachtId);
                                    if (responsePricingPlanDetail != null && responsePricingPlanDetail.IsSuccessStatusCode && responsePricingPlanDetail.ResponseData != null)
                                    {
                                        if (responsePricingPlanDetail.ResponseData.Details != null)
                                        {
                                            var priceDetail = responsePricingPlanDetail.ResponseData.Details.OrderByDescending(x => x.PricingTypeFid).FirstOrDefault();
                                            if (priceDetail != null)
                                            {
                                                if (string.IsNullOrEmpty(yachtCultureCode))
                                                {
                                                    yachtCultureCode = priceDetail.CultureCode;
                                                }
                                                if (string.IsNullOrEmpty(yachtCurrencyCode))
                                                {
                                                    yachtCurrencyCode = priceDetail.CurrencyCode;
                                                }
                                            }
                                            GlobalMethod.GetPriceRecuse(responsePricingPlanDetail.ResponseData.Details, bookingDayNumber, ref dbYachtFeeEach);
                                        }
                                    }

                                    #endregion
                                    dbYachtFee += dbYachtFeeEach;


                                    var lstProductPackage = yachtItem.ProductPackage;
                                    if (lstProductPackage != null)
                                    {
                                        List<string> lstProductId = lstProductPackage.Select(x => x.productInventoryFId).ToList();
                                        var responsePriceOfProduct = _yachtMerchantProductInventoryService.GetPriceOfProductInventoryByArrayOfProductId(lstProductId);
                                        if (responsePriceOfProduct != null && responsePriceOfProduct.IsSuccessStatusCode && responsePriceOfProduct.ResponseData != null)
                                        {
                                            foreach (MerchantProductInventoriesModel proItem in lstProductPackage)
                                            {
                                                try
                                                {
                                                    YachtMerchantProductInventoriesWithPriceViewModel reponseProduct = responsePriceOfProduct.ResponseData.FirstOrDefault(x => x.Id.Trim() == proItem.productInventoryFId.Trim());
                                                    if (reponseProduct != null)
                                                    {
                                                        dbPackageFee += GlobalMethod.PackageTotal(reponseProduct.Price, proItem.quantity);
                                                    }
                                                }
                                                catch
                                                {
                                                    #region logging add detail to errGlobal variable                                                
                                                    #endregion
                                                }
                                            }
                                        }
                                    }
                                }
                                TotalFeeModel.dbTotalFee = dbYachtFee + dbPackageFee;
                                TotalFeeModel.dbTotalRepaid = dbPrePaidRate * TotalFeeModel.dbTotalFee;
                                TotalFeeModel.DisplayTotalFee = TotalFeeModel.dbTotalFee.ToCurrencyText(yachtCultureCode);
                                TotalFeeModel.DisplayTotalRepaid = TotalFeeModel.dbTotalRepaid.ToCurrencyText(yachtCultureCode);
                                TotalFeeModel.CultureCode = yachtCultureCode;
                                TotalFeeModel.CurrencyCode = yachtCurrencyCode;
                            }
                        }
                    }
                    #endregion
                }
                return BaseResponse<BookingTotalFee>.Success(TotalFeeModel);
            }
            catch (Exception ex)
            {
                return BaseResponse<BookingTotalFee>.InternalServerError(TotalFeeModel, message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        //*****modified by hoangle 27-11-2019
        //*****next modified by 
        public BaseResponse<List<YachtPackageViewModel>> GetRedisCartStorageAll(string hashKey, string key)
        {
            List<YachtPackageViewModel> lstPriceResult = new List<YachtPackageViewModel>();
            string Error = "0";
            try
            {
                var value = _distributedCache.GetString(key);
                if (value != null)
                {
                    var lstRedisStorage = JsonConvert.DeserializeObject<List<RedisStorage>>(value);

                    //requestModel.HashKey ==> Dining or Yacht or other...
                    var RedisStorageModel = lstRedisStorage.FirstOrDefault(k => k.Domain == hashKey);
                    #region DOMAIN
                    if (RedisStorageModel != null)// DOMAIN IS  EXIST
                    {
                        var result = RedisStorageModel.PackageStorage;

                        if (result != null)
                        {
                            foreach (YachtPackageServiceModel yachtItem in result)
                            {
                                YachtPackageViewModel modelView = new YachtPackageViewModel();
                                var yachtFIdde = Terminator.Decrypt(yachtItem.YachtId).ToInt32();

                                var yachtRs = _AQYachtContext.Yachts.Where(x => x.Deleted == false
                                  && x.ActiveForOperation == true
                                  && x.Id == yachtFIdde
                                ).Select(x => new YachtCartInfo { Name = x.Name, FileStreamId = x.FileStreams.Where(k => k.FileTypeFid == (int)FileTypeEnum.Image || k.FileTypeFid == (int)FileTypeEnum.RefImage && k.ActivatedDate <= DateTime.Now.Date).OrderByDescending(k => k.ActivatedDate).FirstOrDefault().FileStreamFid }).FirstOrDefault();
                                if (yachtRs != null)
                                {
                                    modelView.YachtName = yachtRs.Name;

                                    modelView.FileStreamFid = yachtRs.FileStreamId;
                                }
                                else
                                {
                                    Error = "-3"; // Yacht Was not Found
                                }
                                modelView.YachtId = yachtItem.YachtId;
                                modelView.Passenger = yachtItem.Passenger;
                                modelView.CheckIn = yachtItem.CheckIn;
                                modelView.CheckOut = yachtItem.CheckOut;
                                double dbPrePaidRate = 0.5;
                                double dbYachtFee = 0;
                                string yachtCurrencyCode = "";
                                string yachtCultureCode = "";
                                int bookingDayNumber = GlobalMethod.BookingDayNumber(modelView.CheckIn, modelView.CheckOut);

                                #region GET PRICE
                                //calling Api
                                var responsePricingPlanDetail = _yachtPricingPlanDetailService.GetPricingPlanDetailYachtFId(yachtItem.YachtId);
                                if (responsePricingPlanDetail != null && responsePricingPlanDetail.IsSuccessStatusCode && responsePricingPlanDetail.ResponseData != null)
                                {
                                    if (responsePricingPlanDetail.ResponseData.Details != null)
                                    {
                                        var priceDetail = responsePricingPlanDetail.ResponseData.Details.OrderByDescending(x => x.PricingTypeFid).FirstOrDefault();
                                        if (priceDetail != null)
                                        {
                                            yachtCultureCode = priceDetail.CultureCode;
                                            yachtCurrencyCode = priceDetail.CurrencyCode;
                                        }
                                        GlobalMethod.GetPriceRecuse(responsePricingPlanDetail.ResponseData.Details, bookingDayNumber, ref dbYachtFee);
                                    }
                                }

                                #endregion

                                modelView.CurrencyCode = yachtCurrencyCode;
                                modelView.CultureCode = yachtCultureCode;
                                modelView.YachtFee = dbYachtFee;
                                modelView.TotalFee = modelView.YachtFee;
                                double dbPackageFee = 0;
                                var lstProductPackage = yachtItem.ProductPackage;
                                string detailError = "";
                                if (lstProductPackage != null)
                                {
                                    List<string> lstProductId = lstProductPackage.Select(x => x.productInventoryFId).ToList();
                                    var responsePriceOfProduct = _yachtMerchantProductInventoryService.GetPriceOfProductInventoryByArrayOfProductId(lstProductId);
                                    if (responsePriceOfProduct != null && responsePriceOfProduct.IsSuccessStatusCode && responsePriceOfProduct.ResponseData != null)
                                    {
                                        foreach (MerchantProductInventoriesModel proItem in lstProductPackage)
                                        {
                                            try
                                            {
                                                YachtMerchantProductInventoriesWithPriceViewModel reponseProduct = responsePriceOfProduct.ResponseData.FirstOrDefault(x => x.Id.Trim() == proItem.productInventoryFId.Trim());
                                                if (reponseProduct != null)
                                                {
                                                    dbPackageFee += GlobalMethod.PackageTotal(reponseProduct.Price, proItem.quantity);
                                                }
                                            }
                                            catch
                                            {
                                                #region logging add detail to errGlobal variable    
                                                if (detailError != "")
                                                {
                                                    detailError += ",";
                                                }
                                                detailError += $"productInventoryFId:{proItem.productInventoryFId}";
                                                #endregion
                                            }
                                        }
                                    }
                                }
                                modelView.TotalFee = modelView.TotalFee + dbPackageFee;
                                modelView.Prepaid = dbPrePaidRate * modelView.TotalFee;
                                if (modelView.TotalFee == 0)
                                {
                                    modelView.DisplayPackageFee = "0";
                                    modelView.DisplayYachtFee = "0";
                                    modelView.DisplayTotalFee = "0";
                                    modelView.DisplayPrepaid = "0";
                                }
                                if (modelView.TotalFee > 0)
                                {
                                    modelView.DisplayPackageFee = modelView.PackageFee.ToCurrencyText(modelView.CultureCode);
                                    modelView.DisplayYachtFee = modelView.YachtFee.ToCurrencyText(modelView.CultureCode);
                                    modelView.DisplayTotalFee = modelView.TotalFee.ToCurrencyText(modelView.CultureCode);
                                    modelView.DisplayPrepaid = modelView.Prepaid.ToCurrencyText(modelView.CultureCode);
                                }
                                modelView.ErrorCode = "{YACHT_ERROR:" + Error + "\",DETAIL:{\"" + detailError + "\"}}";
                                lstPriceResult.Add(modelView);
                            }
                        }
                    }
                    #endregion
                }
                return BaseResponse<List<YachtPackageViewModel>>.Success(lstPriceResult);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtPackageViewModel>>.InternalServerError(lstPriceResult, message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        //*****modified by hoangle 27-11-2019
        //*****next modified by 
        public BaseResponse<YachtPackageViewModel> GetRedisCartStorageDetail(string hashKey, string key, string yachtFId)
        {
            YachtPackageViewModel modelView = new YachtPackageViewModel();
            try
            {

                var value = _distributedCache.GetString(key);
                if (value != null)
                {
                    var lstRedisStorage = JsonConvert.DeserializeObject<List<RedisStorage>>(value);

                    //requestModel.HashKey ==> Dining or Yacht or other...
                    var RedisStorageModel = lstRedisStorage.FirstOrDefault(k => k.Domain == hashKey);
                    #region DOMAIN
                    if (RedisStorageModel != null)// DOMAIN IS  EXIST
                    {
                        var result = RedisStorageModel.PackageStorage;
                        if (result != null)
                        {

                            var yachtStoraged = result.FirstOrDefault(k => k.YachtId == yachtFId);

                            if (yachtStoraged != null)
                            {
                                var yachtFIdde = Terminator.Decrypt(yachtStoraged.YachtId).ToInt32();
                                var yachtRs = _AQYachtContext.Yachts.FirstOrDefault(x => x.Deleted == false
                                  && x.ActiveForOperation == true
                                  && x.Id == yachtFIdde
                                );
                                if (yachtRs != null)
                                {
                                    modelView.YachtName = yachtRs.Name;
                                }
                                modelView.Passenger = yachtStoraged.Passenger;
                                modelView.CheckIn = yachtStoraged.CheckIn;
                                modelView.CheckOut = yachtStoraged.CheckOut;
                                double dbPrePaidRate = 0.5;
                                double dbYachtFee = 0;
                                string yachtCurrencyCode = "";
                                string yachtCultureCode = "";
                                int bookingDayNumber = GlobalMethod.BookingDayNumber(modelView.CheckIn, modelView.CheckOut);

                                #region GET PRICE
                                //calling Api
                                var responsePricingPlanDetail = _yachtPricingPlanDetailService.GetPricingPlanDetailYachtFId(yachtStoraged.YachtId);
                                if (responsePricingPlanDetail != null && responsePricingPlanDetail.IsSuccessStatusCode && responsePricingPlanDetail.ResponseData != null)
                                {
                                    if (responsePricingPlanDetail.ResponseData.Details != null)
                                    {
                                        var priceDetail = responsePricingPlanDetail.ResponseData.Details.OrderByDescending(x => x.PricingTypeFid).FirstOrDefault();
                                        if (priceDetail != null)
                                        {
                                            yachtCultureCode = priceDetail.CultureCode;
                                            yachtCurrencyCode = priceDetail.CurrencyCode;
                                        }
                                        GlobalMethod.GetPriceRecuse(responsePricingPlanDetail.ResponseData.Details, bookingDayNumber, ref dbYachtFee);
                                    }
                                }

                                #endregion

                                modelView.YachtId = yachtStoraged.YachtId;
                                modelView.CurrencyCode = yachtCurrencyCode;
                                modelView.CultureCode = yachtCultureCode;
                                modelView.YachtFee = dbYachtFee;
                                modelView.TotalFee = modelView.YachtFee;

                                #region PACKAGE
                                double dbPackageFee = 0;
                                var lstProductPackage = yachtStoraged.ProductPackage;
                                if (lstProductPackage != null)
                                {
                                    List<string> lstProductId = lstProductPackage.Select(x => x.productInventoryFId).ToList();
                                    var responsePriceOfProduct = _yachtMerchantProductInventoryService.GetPriceOfProductInventoryByArrayOfProductId(lstProductId);
                                    if (responsePriceOfProduct != null && responsePriceOfProduct.IsSuccessStatusCode && responsePriceOfProduct.ResponseData != null)
                                    {
                                        List<MerchantProductInventoriesViewModel> ProductPackageList = new List<MerchantProductInventoriesViewModel>();
                                        foreach (MerchantProductInventoriesModel proItem in lstProductPackage)
                                        {
                                            try
                                            {
                                                YachtMerchantProductInventoriesWithPriceViewModel reponseProduct = responsePriceOfProduct.ResponseData.FirstOrDefault(x => x.Id.Trim() == proItem.productInventoryFId.Trim());
                                                if (reponseProduct != null)
                                                {
                                                    MerchantProductInventoriesViewModel ProductPackageItem = new MerchantProductInventoriesViewModel();
                                                    ProductPackageItem.PackageFee = reponseProduct.Price;
                                                    ProductPackageItem.TotalPackageFee = GlobalMethod.PackageTotal(reponseProduct.Price, proItem.quantity);
                                                    ProductPackageItem.productInventoryFId = proItem.productInventoryFId;
                                                    ProductPackageItem.productName = proItem.productName;
                                                    ProductPackageItem.categroryFId = proItem.categroryFId;
                                                    ProductPackageItem.quantity = proItem.quantity;
                                                    ProductPackageItem.DisplayPackageFee = ProductPackageItem.PackageFee.ToCurrencyText(modelView.CultureCode);
                                                    ProductPackageItem.DisplayTotalPackageFee = ProductPackageItem.TotalPackageFee.ToCurrencyText(modelView.CultureCode);

                                                    ProductPackageList.Add(ProductPackageItem);

                                                    dbPackageFee += GlobalMethod.PackageTotal(reponseProduct.Price, proItem.quantity);
                                                }
                                            }
                                            catch
                                            {
                                                #region logging add detail to errGlobal variable                                                
                                                #endregion
                                            }
                                        }
                                        if (ProductPackageList.Count > 0)
                                        {
                                            modelView.ProductPackage = ProductPackageList;
                                        }
                                    }
                                }
                                #endregion
                                modelView.PackageFee = dbPackageFee;
                                modelView.TotalFee = modelView.TotalFee + dbPackageFee;
                                modelView.Prepaid = dbPrePaidRate * modelView.TotalFee;
                                modelView.DisplayPackageFee = modelView.PackageFee.ToCurrencyText(modelView.CultureCode);
                                modelView.DisplayYachtFee = modelView.YachtFee.ToCurrencyText(modelView.CultureCode);
                                modelView.DisplayTotalFee = modelView.TotalFee.ToCurrencyText(modelView.CultureCode);
                                modelView.DisplayPrepaid = modelView.Prepaid.ToCurrencyText(modelView.CultureCode);
                            }
                        }
                    }
                    #endregion
                }
                return BaseResponse<YachtPackageViewModel>.Success(modelView);
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtPackageViewModel>.InternalServerError(modelView, message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        //*****modified by hoangle 27-11-2019
        //*****next modified by 
        public BaseResponse<ResponseModel> DeleteRedisCartStorage(RedisCacheYacthRequestModel requestModel)
        {
            ResponseModel resultModel = new ResponseModel();
            try
            {
                var value = _distributedCache.GetString(requestModel.Key);
                if (value != null)
                {
                    var lstRedisStorage = JsonConvert.DeserializeObject<List<RedisStorage>>(value);

                    //requestModel.HashKey ==> Dining or Yacht or other...
                    var RedisStorageModel = lstRedisStorage.FirstOrDefault(k => k.Domain == requestModel.HashKey);
                    #region DOMAIN
                    if (RedisStorageModel != null)// DOMAIN IS  EXIST
                    {
                        var result = RedisStorageModel.PackageStorage;
                        if (result != null)
                        {
                            result.RemoveAll(k => k.YachtId == requestModel.YachtId);
                        }
                    }
                    #endregion
                    string strSave = JsonConvert.SerializeObject(lstRedisStorage);
                    _distributedCache.SetString(requestModel.Key, strSave);
                }
                return BaseResponse<ResponseModel>.Success(resultModel);
            }
            catch (Exception ex)
            {
                return BaseResponse<ResponseModel>.InternalServerError(resultModel, message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        #endregion


        #region NEW YATCH FUNCTION
        public BaseResponse<PagedList<YachtSearchItem>> PrivateCharterSearchStoreProcedure(YachtSearchModel searchModel)
        {
            try
            {
                var parameter = new DbParameter[]
                {
                _AQYachtContext.GetParameter("@pCheckinDate",searchModel.CheckIn),
                _AQYachtContext.GetParameter("@pCheckoutDate",searchModel.CheckOut),
                 _AQYachtContext.GetParameter("@pCountry",searchModel.Country),
                _AQYachtContext.GetParameter("@pCity",searchModel.City),
                  _AQYachtContext.GetParameter("@pDistrict",searchModel.District),
                 _AQYachtContext.GetParameter("@pPortID",searchModel.PortFID),
                _AQYachtContext.GetParameter("@pNumOfPassenger",searchModel.Passengers),              
                _AQYachtContext.GetParameter("@pYachtTypeFID",searchModel.YachtTypeFID),
                _AQYachtContext.GetParameter("@pYachtName",searchModel.YachtName),
                _AQYachtContext.GetParameter("@pPageSize",searchModel.PageSize),
                _AQYachtContext.GetParameter("@pPageNumber",searchModel.PageIndex),

                };
                var result = _AQYachtContext.EntityFromSql<YachtSearchItem>("usp_Yacht_Search", parameter).ToList();
                int totalRows = 0;
                if (result.Count > 0)
                    totalRows = result[0].TotalRows;
                var pagedListModel = new PagedList<YachtSearchItem>(result, searchModel.PageIndex, searchModel.PageSize, totalRows);
                return BaseResponse<PagedList<YachtSearchItem>>.Success(pagedListModel);
            }
            catch(Exception ex)
            {
                return BaseResponse<PagedList<YachtSearchItem>>.InternalServerError(ex);
            }
        }
        public BaseResponse<List<YachtSimilarItem>> YachtSearchSimilarStoreProcedure(YachtSimilarSearchModel searchModel)
        {
            try
            {
                var parameter = new DbParameter[]
                {
                _AQYachtContext.GetParameter("@pCheckinDate",searchModel.CheckIn),
                _AQYachtContext.GetParameter("@pCheckoutDate",searchModel.CheckOut),
                _AQYachtContext.GetParameter("@pCity",searchModel.City),
                _AQYachtContext.GetParameter("@pExcludeYachtID",searchModel.ExcludeYachtID),
                _AQYachtContext.GetParameter("@pPrice",searchModel.Price),
                _AQYachtContext.GetParameter("@pPricingTypeFID",searchModel.PricingTypeId)            
                };
                var result = _AQYachtContext.EntityFromSql<YachtSimilarItem>("usp_Yacht_Search_SimilarExperiences", parameter).ToList();
               
                return BaseResponse<List<YachtSimilarItem>>.Success(result);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtSimilarItem>>.InternalServerError(ex);
            }
        }
        #endregion
    }
}
