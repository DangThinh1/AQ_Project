using APIHelpers.Response;
using AQBooking.Core.Helpers;
using AQConfigurations.Core.Services.Interfaces;
using AutoMapper;
using ExtendedUtility;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YachtMerchant.Core.Enum;
using YachtMerchant.Core.Helpers;
using YachtMerchant.Core.Models.Yacht;
using YachtMerchant.Core.Models.YachtPort;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Infrastructure.Services
{
    public class YachtService : ServiceBase, IYachtService
    {
        #region Fields

        private readonly IWorkContext _workContext;
        private readonly ICurrencyRequestService _currencyService;
        private readonly IMapper _mapper;

        #endregion Fields

        #region Ctor

        public YachtService(
            IWorkContext workContext,
            ICurrencyRequestService currencyService,
            IMapper mapper,
            YachtOperatorDbContext context) : base(context)
        {
            _workContext = workContext;
            _currencyService = currencyService;
            _mapper = mapper;
        }

        #endregion Ctor

        #region Methods

        public BaseResponse<YachtBasicProfileModel> GetYachtBasicProfile(int yachtId)
        {
            if (yachtId > 0)
            {
                var result = _context.Yachts.AsNoTracking().FirstOrDefault(k => k.Id == yachtId && k.Deleted == false);
                if (result != null)
                {
                    return BaseResponse<YachtBasicProfileModel>.Success(new YachtBasicProfileModel()
                    {
                        YachtId = result.Id,
                        YachtUniqueId = result.UniqueId,
                        MerchantId = result.MerchantFid,
                        YachtName = result.Name,
                    });
                }
                else
                    return BaseResponse<YachtBasicProfileModel>.Success(new YachtBasicProfileModel() { YachtId = 0, MerchantId = 0, YachtUniqueId = "", YachtName = "" });
            }
            return BaseResponse<YachtBasicProfileModel>.Success(new YachtBasicProfileModel() { YachtId = 0, MerchantId = 0, YachtUniqueId = "", YachtName = "" });
        }

        public BaseResponse<bool> CreateYacht(YachtCreateModel model)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var userId = GetUserGuidId();
                    //Add yacht
                    var yacht = new Yachts();
                    yacht = _mapper.Map<YachtCreateModel, Yachts>(model, yacht);
                    yacht.UniqueId = UniqueIDHelper.GenarateRandomString(12);
                    yacht.Deleted = false;
                    yacht.CreatedBy = userId;
                    yacht.CreatedDate = DateTime.Now;
                    yacht.LastModifiedBy = userId;
                    yacht.LastModifiedDate = DateTime.Now;
                    _context.Yachts.Add(yacht);
                    var result = _context.SaveChanges();

                    var listYachtAttrVal = new List<YachtAttributeValues>();
                    //Add yacht attributevalue accomodations
                    if (!string.IsNullOrEmpty(model.ListAccomodation))
                    {
                        var attrAccomodations = model.ListAccomodation.Split(",");
                        foreach (var item in attrAccomodations)
                        {
                            var yachtAttributeValue = new YachtAttributeValues
                            {
                                AttributeCategoryFid = (int)YachtAttributeEnum.Accomodations,
                                AttributeFid = item.Split("_")[0].ToInt32(),
                                AttributeValue = item.Split("_")[1],
                                YachtFid = yacht.Id,
                                LastModifiedBy = userId,
                                LastModifiedDate = DateTime.Now
                            };

                            listYachtAttrVal.Add(yachtAttributeValue);
                        }
                    }

                    //Add yacht port
                    if (model.PortLocationId > 0)
                    {
                        var portInfo = _context.PortLocations.AsNoTracking().Where(x => x.Id == model.PortLocationId && x.Deleted == false).FirstOrDefault();
                        if (portInfo != null)
                        {
                            var yachtPort = new YachtPorts
                            {
                                YachtFid = yacht.Id,
                                PortFid = portInfo.Id,
                                PortName = portInfo.PickupPointName,
                                IsActivated = true,
                                Deleted = false,
                                CreatedBy = userId,
                                CreatedDate = DateTime.Now,
                                EffectiveDate = DateTime.Now
                            };

                            //update location for yacht
                            yacht.Country = portInfo.Country;
                            yacht.City = portInfo.City;
                            _context.Yachts.Update(yacht);

                            _context.YachtPorts.Add(yachtPort);
                        }
                    }

                    //Add yacht attributevalue amenities
                    if (!string.IsNullOrEmpty(model.ListAmenities))
                    {
                        var attrAmenities = model.ListAmenities.Split(",");
                        foreach (var item in attrAmenities)
                        {
                            var yachtAttributeValue = new YachtAttributeValues
                            {
                                AttributeCategoryFid = (int)YachtAttributeEnum.Ametities,
                                AttributeFid = item.Split("_")[0].ToInt32(),
                                AttributeValue = item.Split("_")[1],
                                YachtFid = yacht.Id,
                                LastModifiedBy = userId,
                                LastModifiedDate = DateTime.Now
                            };

                            listYachtAttrVal.Add(yachtAttributeValue);
                        }
                    }

                    if (listYachtAttrVal.Count > 0)
                        _context.YachtAttributeValues.AddRange(listYachtAttrVal);

                    //Add yacht non bussinessday
                    if (!string.IsNullOrEmpty(model.ListNonBusinessDay))
                    {
                        var listNonBizDay = model.ListNonBusinessDay.Split(",");
                        var listYachtNonBizDay = new List<YachtNonOperationDays>();
                        foreach (var item in listNonBizDay)
                        {
                            var date = item.Replace(".", "-");
                            var dateTime = (date + "-" + DateTime.Now.Year).ToNullDateTime();
                            if (dateTime != null)
                            {
                                var yachtNonBizDay = new YachtNonOperationDays
                                {
                                    YachtFid = yacht.Id,
                                    Recurring = false,
                                    Remark = "",
                                    StartDate = dateTime.Value,
                                    EndDate = dateTime.Value,
                                    CreatedBy = userId,
                                    CreatedDate = DateTime.Now,
                                    LastModifiedBy = userId,
                                    LastModifiedDate = DateTime.Now
                                };
                                listYachtNonBizDay.Add(yachtNonBizDay);
                            }
                        }

                        _context.YachtNonOperationDays.AddRange(listYachtNonBizDay);
                    }

                    //Add yachtCounter
                    var yachtCounter = new YachtCounters
                    {
                        YachtId = yacht.Id,
                        YachtUniqueId = yacht.UniqueId,
                        TotalViews = 0,
                        TotalBookings = 0,
                        TotalSuccessBookings = 0,
                        TotalReviews = 0,
                        TotalRecommendeds = 0,
                        TotalNotRecommendeds = 0
                    };

                    _context.YachtCounters.Add(yachtCounter);
                    result = _context.SaveChanges();

                    transaction.Commit();
                    transaction.Dispose();
                    return BaseResponse<bool>.Success();
                }
                catch (Exception ex)
                {
                    return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
                }
            }
        }

        public async Task<BaseResponse<YachtSetupModel>> GetYachtSetup(int merchantId)
        {
            if (_context.YachtMerchants.AsNoTracking().Any(x => x.Id == merchantId))
            {
                var merchant = await _context.YachtMerchants.FindAsync(merchantId);
                var currency = _currencyService.FindByCountryName(merchant.Country).ResponseData;
                var setup = new YachtSetupInfo
                {
                    City = merchant.City,
                    Country = merchant.Country,
                    CultureCode = currency.CultureCode,
                    Currency = currency.CurrencyCode,
                    CurrencyResourceKey = currency.ResourceKey
                };

                return BaseResponse<YachtSetupModel>.Success(new YachtSetupModel(true, "", setup));
            }
            else
                return BaseResponse<YachtSetupModel>.Success(new YachtSetupModel(false, "Not existed"));
        }

        public BaseResponse<YachtCheckActiveModel> ActiveYacht(int yachtId)
        {
            try
            {
                var yacht = _context.Yachts.FirstOrDefault(x => x.Id == yachtId && !x.Deleted);
                if (yacht == null)
                    return BaseResponse<YachtCheckActiveModel>.BadRequest();

                var infor = _context.YachtInformations.Any(x => x.YachtFid == yachtId && !x.Deleted && x.IsActivated == true);
                var pricingPlan = _context.YachtPricingPlans.Any(x => x.YachtFid == yachtId && !x.Deleted && x.IsActivated == true);
                var refImage = _context.YachtFileStreams.Any(x => x.YachtFid == yachtId && !x.Deleted 
                                                    && x.FileTypeFid == (int)YachtImageTypeEnum.RefImage
                                                    && x.ActivatedDate.Date <= DateTime.Now.Date);

                var model = new YachtCheckActiveModel()
                {
                    CheckInformation = infor,
                    CheckRefImage = refImage,
                    CheckPricingPlan = pricingPlan,
                    CheckActiveForOperation = yacht.ActiveForOperation,
                };
                if (infor && pricingPlan && refImage)
                    model.Allow = true;

                return BaseResponse<YachtCheckActiveModel>.Success(model);
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtCheckActiveModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> SetActiveYacht(int yachtId, bool isActiveOperation)
        {
            try
            {
                bool check = ActiveYacht(yachtId).IsSuccessStatusCode;
                if (check)
                {
                    var entity = _context.Yachts.FirstOrDefault(x => x.Id == yachtId && !x.Deleted && x.ActiveForOperation != isActiveOperation);
                    if (entity == null)
                        return BaseResponse<bool>.BadRequest(false);
                    entity.ActiveForOperation = isActiveOperation;
                    entity.LastModifiedBy = GetUserGuidId();
                    entity.LastModifiedDate = DateTime.Now;
                    _context.Yachts.Update(entity);
                    var result = _context.SaveChanges();
                    return result == 1 ? BaseResponse<bool>.Success(true) : BaseResponse<bool>.BadRequest(false);
                }

                return BaseResponse<bool>.Success();
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> UpdateYachtInfo(YachtUpdateModel model)
        {
            using (var trans = _context.Database.BeginTransaction())
            {
                try
                {
                    var userId = GetUserGuidId();
                    //Update yacht basic info
                    var yachtInfo = _context.Yachts.FirstOrDefault(x => x.Id == model.Id);
                    if (yachtInfo == null)
                        return BaseResponse<bool>.BadRequest(false);
                    yachtInfo = _mapper.Map<YachtUpdateModel, Yachts>(model, yachtInfo);
                    yachtInfo.LastModifiedDate = DateTime.Now;
                    yachtInfo.LastModifiedBy = userId;
                    _context.Yachts.Update(yachtInfo);
                    _context.SaveChanges();

                    //Update yacht port
                    var yachtPort = _context.YachtPorts.AsNoTracking().Where(x => x.YachtFid == yachtInfo.Id && x.PortFid == model.PortLocationId)
                        .OrderByDescending(x => x.EffectiveDate)
                        .FirstOrDefault();
                    var portInfo = _context.PortLocations.Where(x => x.Id == model.PortLocationId && x.Deleted == false).FirstOrDefault();

                    if (yachtPort != null)
                    {
                        //Has been port
                        if (yachtPort.Deleted == true)
                            yachtPort.Deleted = false;
                        if (yachtPort.IsActivated == false)
                            yachtPort.IsActivated = true;
                        if (yachtPort.EffectiveDate != DateTime.Now)
                            yachtPort.EffectiveDate = DateTime.Now;
                        yachtPort.LastModifiedDate = DateTime.Now;
                        yachtPort.LastModifiedBy = userId;
                        _context.YachtPorts.Update(yachtPort);
                    }
                    else
                    {
                        //Add new port
                        var newYachtPort = new YachtPorts
                        {
                            PortName = portInfo.PickupPointName,
                            YachtFid = yachtInfo.Id,
                            PortFid = model.PortLocationId,
                            EffectiveDate = DateTime.Now,
                            Deleted = false,
                            IsActivated = true,
                            CreatedBy = userId,
                            CreatedDate = DateTime.Now
                        };

                        _context.YachtPorts.Add(newYachtPort);
                    }

                    //Update new location
                    yachtInfo.Country = portInfo.Country;
                    yachtInfo.City = portInfo.City;
                    _context.Yachts.Update(yachtInfo);

                    _context.SaveChanges();
                    trans.Commit();
                    trans.Dispose();

                    return BaseResponse<bool>.Success(true);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    trans.Dispose();
                    return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
                }
            }
        }

        public BaseResponse<YachtUpdateModel> GetYachtInfoYacht(int yachtId)
        {
            try
            {
                var yachtInfo = new YachtUpdateModel();
                if (yachtId > 0)
                {
                    var result = _context.Yachts.AsNoTracking().FirstOrDefault(k => k.Id == yachtId && k.Deleted == false);
                    if (result != null)
                    {
                        var yachtPort = _context.YachtPorts.AsNoTracking()
                            .Where(x => x.YachtFid == yachtId && x.Deleted == false && x.IsActivated == true && x.EffectiveDate <= DateTime.Now)
                            .OrderByDescending(x => x.EffectiveDate)
                            .FirstOrDefault();
                        yachtInfo = new YachtUpdateModel();
                        yachtInfo.InjectFrom(result);

                        if (yachtPort != null)
                            yachtInfo.PortLocationId = yachtPort.PortFid;
                        return BaseResponse<YachtUpdateModel>.Success(yachtInfo);
                    }
                }

                return BaseResponse<YachtUpdateModel>.Success(yachtInfo);
            }
            catch (Exception ex)
            {
                return BaseResponse<YachtUpdateModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<bool> UpdateYachtPort(YachtPortViewModel model)
        {
            try
            {
                var entity = _context.YachtPorts.AsNoTracking().FirstOrDefault(k => k.YachtFid == model.YachtId && !k.Deleted && k.PortFid == model.YachtPortId);
                if (entity == null)
                    return BaseResponse<bool>.BadRequest(false);
                entity.PortFid = model.YachtPortIdNew;
                entity.PortName = _context.PortLocations.FirstOrDefault(x => x.Id == model.YachtPortIdNew).PickupPointName;
                entity.EffectiveDate = DateTime.Now;
                entity.LastModifiedDate = DateTime.Now;
                entity.LastModifiedBy = GetUserGuidId();
                _context.YachtPorts.Update(entity);
                var result = _context.SaveChanges();
                return result == 1 ? BaseResponse<bool>.Success(true) : BaseResponse<bool>.BadRequest(false);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        #endregion Methods
    }
}