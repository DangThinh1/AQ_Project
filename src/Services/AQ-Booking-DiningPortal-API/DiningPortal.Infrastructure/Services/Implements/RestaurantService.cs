using System;
using AutoMapper;
using System.Linq;
using APIHelpers.Response;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AQDiningPortal.Core.Models.PagingPortal;
using Microsoft.AspNetCore.Mvc.Rendering;
using AQDiningPortal.Infrastructure.Models;
using AQDiningPortal.Core.Models.Restaurants;
using AQDiningPortal.Infrastructure.Database;
using AQDiningPortal.Infrastructure.Interfaces;
using AQDiningPortal.Core.Models.RestaurantMenus;
using AQDiningPortal.Core.Models.RestaurantCounters;
using AQDiningPortal.Infrastructure.Database.Entities;
using AQDiningPortal.Core.Models.RestaurantFileStreams;
using AQDiningPortal.Core.Models.RestaurantBusinessDays;
using AQDiningPortal.Core.Models.RestaurantInformations;
using AQDiningPortal.Core.Models.RestaurantAttributeValues;
using AQDiningPortal.Core.Models.RestaurantNonBusinessDays;
using AQDiningPortal.Core.Models.RestaurantOtherInformations;
using AQDiningPortal.Core.Models.RestaurantInformationDetails;
using AQDiningPortal.Core.Models.RestaurantReservationFees;
using AQDiningPortal.Infrastructure.Helpers;
using AQEncrypts;

namespace BookingPortal.Infrastructure.Services
{
    public partial class RestaurantService : IRestaurantService
    {
        private readonly DiningSearchContext _searchContext;
        private readonly IRestaurantCounterService _restaurantCounterService;
        private readonly IMapper _mapper;
        public RestaurantService(DiningSearchContext searchContext, IRestaurantCounterService restaurantCounterService, IMapper mapper)
        {
            _searchContext = searchContext;
            _restaurantCounterService = restaurantCounterService;
            _mapper = mapper;
        }

        public Restaurants Find(int id)
        {
            try
            {
                return _searchContext.Restaurants.FirstOrDefault(k => k.Deleted == false && k.ActiveForOperation == true);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public BaseResponse<PageListPortal<RestaurantSearchViewModel>> Search(RestaurantSearchModel searchModel)
        {
            try
            {
                if (searchModel == null)
                    return BaseResponse<PageListPortal<RestaurantSearchViewModel>>.NotFound(new PageListPortal<RestaurantSearchViewModel>(Enumerable.Empty<RestaurantSearchViewModel>().AsQueryable(), searchModel.PageIndex, searchModel.PageSize), message: "searchModel variable is null", fullMsg: "");

                searchModel.BusinessDay = searchModel.BusinessDay ?? "";
                var sortString = !string.IsNullOrEmpty(searchModel.SortString)
                ? searchModel.SortString
                : "RestaurantName DESC";

                searchModel.PageIndex = searchModel.PageIndex > 0 ? searchModel.PageIndex : 1;
                searchModel.PageSize = searchModel.PageSize > 0 ? searchModel.PageSize : 10;
                string searchCity = string.IsNullOrWhiteSpace(searchModel.City) ? "\"\"" : "\"" + searchModel.City.Trim() + "\"";
                string searchZoneDistrict = string.IsNullOrWhiteSpace(searchModel.ZoneDistrict) ? "\"\"" : "\"" + searchModel.ZoneDistrict.Trim() + "\"";
                string searchRestaurantName = string.IsNullOrWhiteSpace(searchModel.RestaurantName) ? "\"\"" : "\"" + searchModel.RestaurantName.Trim() + "\"";
                string extendFilter = string.Empty;

                #region Attribute define
                var listCuisine = new List<int>();
                var listSuitable = new List<int>();
                var listServing = new List<int>();
                if (searchModel.AttributeFid != null && searchModel.AttributeFid.Count > 0)
                {
                    List<AttributeValueModel> attributeList = new List<AttributeValueModel>();
                    foreach (string attItem in searchModel.AttributeFid)
                    {
                        if (!string.IsNullOrEmpty(attItem))
                        {
                            string[] arrResponse = attItem.Split('_');
                            if (arrResponse.Count() > 1)
                            {
                                int categoryId = string.IsNullOrEmpty(arrResponse[0]) != true ? int.Parse(arrResponse[0]) : 0;

                                AttributeValueModel attributeItem = new AttributeValueModel { CategoryId = string.IsNullOrEmpty(arrResponse[0]) != true ? int.Parse(arrResponse[0]) : 0, AttributeValue = string.IsNullOrEmpty(arrResponse[1]) != true ? int.Parse(arrResponse[1]) : 0 };
                                attributeList.Add(attributeItem);
                            }
                        }
                    }
                    listCuisine = (attributeList.Where(k => k.CategoryId == (int)AttributeEnum.Cuisines).Select(x => x.AttributeValue)).ToList();
                    listServing = (attributeList.Where(k => k.CategoryId == (int)AttributeEnum.Servings).Select(x => x.AttributeValue)).ToList();
                    listSuitable = (attributeList.Where(k => k.CategoryId == (int)AttributeEnum.SuitableFor).Select(x => x.AttributeValue)).ToList();
                    
                }

                #endregion


              
                var query = (_searchContext.Restaurants
                   .Where(
                    k => k.Deleted == false && k.ActiveForOperation == true
                    && (listCuisine.Count() == 0 || (listCuisine.Count() > 0 && k.AttributeValues.Any(l => l.AttributeCategoryFid == (int)AttributeEnum.Cuisines && listCuisine.Contains(l.AttributeFid))))
                    && (listServing.Count() == 0 || (listServing.Count() > 0 && k.AttributeValues.Any(l => l.AttributeCategoryFid == (int)AttributeEnum.Servings && listServing.Contains(l.AttributeFid))))
                    && (listSuitable.Count() == 0 || (listSuitable.Count() > 0 && k.AttributeValues.Any(l => l.AttributeCategoryFid == (int)AttributeEnum.SuitableFor && listSuitable.Contains(l.AttributeFid))))
                    && (string.IsNullOrWhiteSpace(searchModel.City) || (!string.IsNullOrWhiteSpace(searchModel.City) && k.City == searchModel.City.Trim()))
                    && (string.IsNullOrWhiteSpace(searchModel.ZoneDistrict) || (!string.IsNullOrWhiteSpace(searchModel.ZoneDistrict) && k.ZoneDistrict == searchModel.ZoneDistrict.Trim()))
                    && (string.IsNullOrWhiteSpace(searchModel.RestaurantName) || (!string.IsNullOrWhiteSpace(searchModel.RestaurantName) && k.RestaurantName.StartsWith(searchModel.RestaurantName.Trim())))                    
                    //&& (k.ServingDining == searchModel.ServingType
                    
                    )
                   .Select(k => new RestaurantSearchViewModel()
                   {
                       Id = k.Id
                       ,
                       UniqueId = k.UniqueId
                       ,
                       RestaurantName = k.RestaurantName
                       ,
                       IsBusinessDay = _searchContext.GetfnIsBusinessDayOperation(k.Id, searchModel.BusinessDay.Trim() ?? "")
                       ,
                       CuisineName = _searchContext.GetCuisines(k.Id, 1) ?? ""
                       ,
                       City = k.City ?? ""
                       ,
                       ZoneDistrict = k.ZoneDistrict ?? ""
                       ,
                       Country = k.Country ?? ""
                       ,
                       StartingPrice = k.StartingPrice
                       ,
                       CultureCode = k.CultureCode ?? ""
                       ,
                       CurrencyCode = k.CurrencyCode ?? ""
                       ,
                       FileStreamId = _searchContext.GetfnRestaurantImageIDVal(k.Id, 4),
                       ServingDining = k.ServingDining,
                       ServingVenue = k.ServingVenue

                   })).OrderBy(sortString);                
                return BaseResponse<PageListPortal<RestaurantSearchViewModel>>.Success(new PageListPortal<RestaurantSearchViewModel>(query, searchModel.PageIndex, searchModel.PageSize));

            }
            catch (Exception ex)
            {
                return BaseResponse<PageListPortal<RestaurantSearchViewModel>>.InternalServerError(new PageListPortal<RestaurantSearchViewModel>(Enumerable.Empty<RestaurantSearchViewModel>().AsQueryable(), searchModel.PageIndex, searchModel.PageSize), message: ex.Message, fullMsg: ex.StackTrace);
            }
        }


        public BaseResponse<List<SelectListItem>> GetComboBindingByCityAndZone(string City = "", string Zone = "")
        {
            try
            {
                var query = _searchContext.Restaurants.Where(x => x.Deleted == false && x.ActiveForOperation == true && (string.IsNullOrWhiteSpace(City) || x.City == City) && (string.IsNullOrWhiteSpace(Zone) || x.ZoneDistrict == Zone)).Select(k =>

                           new SelectListItem
                           {
                               Text = k.RestaurantName,
                               Value = k.Id.ToString()
                           }
                 ).ToList();
                return BaseResponse<List<SelectListItem>>.Success(query, "get restaurant to combobox was success", "");
            }
            catch (Exception ex)
            {
                return BaseResponse<List<SelectListItem>>.InternalServerError(new List<SelectListItem>(), message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<RestaurantDetailViewModel> GetDetail(int id, int languageId = 1, DateTime? activatedDate = null)
        {
            try
            {
                var restaurantDb = _searchContext.Restaurants.Find(id);
                if (restaurantDb == null)
                    return BaseResponse<RestaurantDetailViewModel>.NotFound();

                var filterDate = activatedDate.HasValue ? activatedDate.GetValueOrDefault() : DateTime.Now.Date;

                //load RestaurantAttributeValues 
                _searchContext.RestaurantAttributeValues
                    .Where(k => k.RestaurantFid == restaurantDb.Id && (k.EffectiveDate == null || k.EffectiveDate.Value.Date <= filterDate))
                    .OrderBy(k => k.AttributeCategoryFid)
                    .Load();

                //load Information with InformationDetails
                _searchContext.RestaurantInformations
                    .Where(k => k.Deleted == false && k.IsActivated == true && k.RestaurantFid == restaurantDb.Id && (k.ActivatedDate == null || k.ActivatedDate.Value.Date <= filterDate))
                    .OrderByDescending(k => k.ActivatedDate)
                    .Load();
                var informationDb = restaurantDb.Informations.FirstOrDefault();
                var information = _mapper.Map<RestaurantInformations, RestaurantInformationViewModel>(informationDb);
                if (informationDb != null)
                {
                    //load InformationDetail with languageId
                    _searchContext.RestaurantInformationDetails
                        .Where(k => k.Deleted == false && k.IsActivated == true && k.InformationFid == informationDb.Id && (k.ActivatedDate == null) || k.ActivatedDate.Value.Date <= filterDate)
                        .OrderBy(k => k.Id)
                        .Load();
                    var informationDetail = informationDb.InformationDetails.Where(k => k.LanguageFid == languageId).Select(k => _mapper.Map<RestaurantInformationDetails, RestaurantInformationDetailViewModel>(k)).FirstOrDefault();
                    if (informationDetail == null && languageId != 1)
                    {
                        informationDetail = informationDb.InformationDetails.Where(k => k.LanguageFid == 1).Select(k => _mapper.Map<RestaurantInformationDetails, RestaurantInformationDetailViewModel>(k)).FirstOrDefault();
                    }

                    information.InformationDetail = informationDetail;
                }

                //Load OtherInformation with languageId
                _searchContext.RestaurantOtherInformations
                    .Where(k => k.Deleted == false && k.IsActivated == true && k.RestaurantFid == restaurantDb.Id && k.LanguageFid == languageId && (k.ActivatedDate == null || k.ActivatedDate.Value.Date <= filterDate))
                    .OrderByDescending(k => k.ActivatedDate)
                    .Load();

                //Load BusinessDays
                _searchContext.RestaurantBusinessDays
                    .Include(k => k.BusinessDayOperations)
                    .Where(k => k.Deleted == false && k.RestaurantFid == restaurantDb.Id)
                    .Load();

                //Load NonBusinessDays
                _searchContext.RestaurantNonBusinessDays
                    .Where(k => k.Deleted == false && k.RestaurantFid == restaurantDb.Id)
                    .Load();

                //Load FileStream
                _searchContext.RestaurantFileStreams
                    .Where(k => k.Deleted == false && k.RestaurantFid == restaurantDb.Id && k.ActivatedDate.Date <= filterDate)
                    .OrderByDescending(k => k.ActivatedDate)
                    .Load();

                //Topup and load Counter
                var topup = _restaurantCounterService.IncreaseTotalViews(restaurantDb.Id);//Increase TotalViews by 1
                _searchContext.RestaurantCounters
                    .Where(k => k.RestaurantId == restaurantDb.Id)
                    .Load();

                //Load Menus
                _searchContext.RestaurantMenus
                    .Include(k => k.Pricings)
                    .Include(k => k.InfoDetails)
                    .Where(k => k.Deleted == false && k.IsActive == true && k.RestaurantFid == restaurantDb.Id && k.EffectiveDate == null || k.EffectiveDate.Date <= filterDate)
                    .OrderByDescending(k => k.EffectiveDate)
                    .Load();

                //Load Reservation fee
                _searchContext.RestaurantReservationFees
                    .Where(k => k.Deleted == false && k.RestaurantFid == restaurantDb.Id && (k.EffectiveDate == null || k.EffectiveDate.Date <= filterDate))
                    .OrderByDescending(k => k.EffectiveDate)
                    .Load();

                var restaurant = _mapper.Map<Restaurants, RestaurantViewModel>(restaurantDb);
                var attributes = (from a in restaurantDb.AttributeValues
                                  join b in _searchContext.RestaurantAttributes on a.AttributeFid equals b.Id
                                  select _mapper.Map<(RestaurantAttributeValues, RestaurantAttributes), RestaurantAttributeValueViewModel>((a, b)))
                                  .ToList();

                var otherInformation = restaurantDb.OtherInformations.Select(k => _mapper.Map<RestaurantOtherInformations, RestaurantOtherInformationViewModel>(k)).FirstOrDefault();
                var bizs = restaurantDb.BusinessDays.Select(k => _mapper.Map<RestaurantBusinessDays, RestaurantBusinessDayViewModel>(k)).ToList();
                var nonBizs = restaurantDb.NonBusinessDays.Select(k => _mapper.Map<RestaurantNonBusinessDays, RestaurantNonBusinessDayViewModel>(k)).ToList();
                var fileStreams = restaurantDb.FileStreams.Select(k => _mapper.Map<RestaurantFileStreams, RestaurantFileStreamViewModel>(k)).ToList();
                var counter = _mapper.Map<RestaurantCounters, RestaurantCounterViewModel>(restaurantDb.Counter);
                var menus = restaurantDb.Menus.Select(k => _mapper.Map<RestaurantMenus, RestaurantMenuViewModel>(k)).ToList();
                var fee = _mapper.Map<RestaurantReservationFees, RestaurantReservationFeeViewModel>(restaurantDb.ReservationFees.FirstOrDefault());
                var restaurantDetail = new RestaurantDetailViewModel()
                {
                    Restaurant = restaurant,
                    AttributeValues = attributes,
                    Information = information,
                    OtherInformation = otherInformation,
                    BusinessDays = bizs,
                    NonBusinessDays = nonBizs,
                    FileStreams = fileStreams,
                    Counter = counter,
                    Menus = menus,
                    ReservationFee = fee
                };
                return BaseResponse<RestaurantDetailViewModel>.Success(restaurantDetail);
            }
            catch (Exception ex)
            {
                return BaseResponse<RestaurantDetailViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        public BaseResponse<List<RestaurantPartnerViewModel>> GetRestaurantPartners(int DisplayNumber = 4, int ImageType = 4)
        {
            try
            {
                List<RestaurantPartnerViewModel> restaurantPartnerList = (_searchContext.Restaurants.Where(k => k.Deleted == false && k.ActiveForOperation == true)
                    .OrderBy(x => Guid.NewGuid()).Take(DisplayNumber).Select(l => new RestaurantPartnerViewModel
                    {
                        Id = l.Id
                        ,
                        FileStreamId = _searchContext.GetfnRestaurantImageIDVal(l.Id, ImageType)
                        ,
                        RestaurantName = l.RestaurantName
                        ,
                        UniqueId = l.UniqueId
                    })).ToList();
                return BaseResponse<List<RestaurantPartnerViewModel>>.Success(restaurantPartnerList);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<RestaurantPartnerViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        
        public BaseResponse<PageListPortal<RestaurantSearchViewModel>> GetRestaurantsByMerchantFId(SearchRestaurantWithMerchantIdModel searchModel)
        {
            try
            {
                
                var sortString = !string.IsNullOrEmpty(searchModel.SortString)
                ? searchModel.SortString
                : "RestaurantName DESC";

                searchModel.PageIndex = searchModel.PageIndex > 0 ? searchModel.PageIndex : 1;
                searchModel.PageSize = searchModel.PageSize > 0 ? searchModel.PageSize : 10;
                
                var IdDecrypted = Terminator.Decrypt(searchModel.MerchantId);
                int mechantId = Convert.ToInt32(IdDecrypted);
                var restaurantList=(_searchContext.Restaurants
                    .Where(k => k.Deleted == false 
                                && k.ActiveForOperation == true
                                && k.MerchantFid== mechantId
                          )
                    .Select(k => new RestaurantSearchViewModel()
                    {
                        Id = k.Id
                       ,
                        UniqueId = k.UniqueId
                       ,
                        RestaurantName = k.RestaurantName
                        ,
                        StreetName = k.StreetName
                       ,
                        City = k.City ?? ""
                       ,
                        ZoneDistrict = k.ZoneDistrict ?? ""
                       ,
                        Country = k.Country ?? ""
                       ,
                        StartingPrice = k.StartingPrice
                       ,
                        CultureCode = k.CultureCode ?? ""
                       ,
                        CurrencyCode = k.CurrencyCode ?? ""
                       ,
                        FileStreamId = _searchContext.GetfnRestaurantImageIDVal(k.Id, 4)
                    })).OrderBy(sortString);
                return BaseResponse<PageListPortal<RestaurantSearchViewModel>>.Success(new PageListPortal<RestaurantSearchViewModel>(restaurantList, searchModel.PageIndex, searchModel.PageSize));
            }
            catch (Exception ex)
            {
                return BaseResponse<PageListPortal<RestaurantSearchViewModel>>.InternalServerError(new PageListPortal<RestaurantSearchViewModel>(Enumerable.Empty<RestaurantSearchViewModel>().AsQueryable(), searchModel.PageIndex, searchModel.PageSize), message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}