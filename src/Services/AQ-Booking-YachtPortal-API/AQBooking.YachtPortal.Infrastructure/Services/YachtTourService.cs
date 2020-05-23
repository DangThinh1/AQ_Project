using AQBooking.YachtPortal.Core.Enum;
using AQBooking.YachtPortal.Core.Models.YachtFileStreams;
using AQBooking.YachtPortal.Core.Models.YachtTourCharter;
using AQBooking.YachtPortal.Core.Models.YachtTours;
using AQBooking.YachtPortal.Infrastructure.Entities;
using AQBooking.YachtPortal.Infrastructure.Helpers;
using AQBooking.YachtPortal.Infrastructure.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Services
{
    public class YachtTourService : ServiceBase, IYachtTourService
    {
        #region Fields
        #endregion

        #region Ctor
        public YachtTourService(
            AQYachtContext yachtDbContext,
            IMapper mapper
            ) : base(yachtDbContext, mapper)
        {
        }
        #endregion

        #region Utilities
        public double TourPriceEstimate(TourPricingTypeEnum pricingType, double tourFee, int maximumPaxToGo, int passenger = 0)
        {
            double result = 0;

            if (pricingType == TourPricingTypeEnum.FlatFee)
            {
                result = Math.Round(tourFee / passenger);
            }

            if (pricingType == TourPricingTypeEnum.BasePersion)
            {
                if (passenger < maximumPaxToGo)
                    result = Math.Round(tourFee / passenger);
                else if (passenger >= maximumPaxToGo)
                    result = tourFee;
            }

            return result;
        }

        #endregion

        #region Methods
        public IPagedList<YachtTourViewModel> SearchYachtTour(YachtTourSearchModel searchModel)
        {
            var yachtTourSearchResult = new PagedList<YachtTourViewModel>();
            var departTimeSearch = searchModel.DepartTime != null ? searchModel.DepartTime : DateTime.Now;
            var returnTimeSearch = searchModel.ReturnTime != null ? searchModel.ReturnTime : DateTime.Now;
            var durationSearch = (returnTimeSearch - departTimeSearch).TotalDays;

            var sortString = string.IsNullOrEmpty(searchModel.SortString) ? "CreatedDate DESC" : searchModel.SortString;

            //Get all yacht tour and yacht model by searchmodel
            var query = (from yo in _yachtDbContext.YachtTourOperationDetails.AsNoTracking()
                         join yt in _yachtDbContext.YachtTours.AsNoTracking() on yo.TourFid equals yt.Id
                         join y in _yachtDbContext.Yachts.AsNoTracking() on yo.YachtFid equals y.Id
                         where yo.Deleted == false && yt.Deleted == false
                         && DateTime.Now >= yo.EffectiveDate && DateTime.Now <= yo.EffectiveEndDate
                         && (searchModel.LocationId == 0 || yt.LocationFid == searchModel.LocationId)
                         && (searchModel.CityFid == 0 || yt.CityFid == searchModel.CityFid)
                         && (string.IsNullOrEmpty(searchModel.CityName) || yt.City.Contains(searchModel.CityName))
                         && (durationSearch == 0 || yt.TourDurationValue <= durationSearch)
                         let tdv = yt.TourDurationValue
                         select new YachtTourViewModel
                         {
                             TourId = yt.Id,
                             TourName = yt.TourName,
                             MerchantFid = yo.MerchantFid,
                             Duration = TimeSpan.FromHours(tdv).TotalHours.ToString(),
                             TourDurationUnitTypeFid = yt.TourDurationUnitTypeFid,
                             TourDurationUnitResKey = yt.TourDurationUnitResKey,
                             TourDurationValue = yt.TourDurationValue,
                             YachtFid = y.Id,
                             YachtModel = y.Name,
                         });

            //Get default picture by yacht tour
            query = (from ytd in query
                     join f in _yachtDbContext.YachtTourFileStreams.AsNoTracking()
                       .GroupBy(x => x.YachtTourFid)
                       .Select(x => x.FirstOrDefault()) on ytd.TourId equals f.YachtTourFid
                     select new YachtTourViewModel
                     {
                         TourId = ytd.TourId,
                         TourName = ytd.TourName,
                         MerchantFid = ytd.MerchantFid,
                         Duration = ytd.Duration,
                         TourDurationUnitTypeFid = ytd.TourDurationUnitTypeFid,
                         TourDurationUnitResKey = ytd.TourDurationUnitResKey,
                         TourDurationValue = ytd.TourDurationValue,
                         YachtFid = ytd.YachtFid,
                         YachtModel = ytd.YachtModel,
                         TourImageFileFid = f.FileStreamFid,
                     });

            //Get yacht tour counter and pricing
            query = (from ytd in query
                     join ytp in _yachtDbContext.YachtTourPricings.AsNoTracking()
                     .Where(p => p.EffectiveDate <= DateTime.Now && DateTime.Now <= p.EffectiveEndDate)
                     .GroupBy(g => g.TourFid)
                     .Select(p => p.FirstOrDefault()).OrderBy("EffectiveDate DESC") on ytd.TourId equals ytp.TourFid
                     join ytc in _yachtDbContext.YachtTourCounters.AsNoTracking() on ytd.TourId equals ytc.YachtTourId
                     select new YachtTourViewModel
                     {
                         TourId = ytd.TourId,
                         TourName = ytd.TourName,
                         MerchantFid = ytd.MerchantFid,
                         Duration = ytd.Duration,
                         TourDurationUnitTypeFid = ytd.TourDurationUnitTypeFid,
                         TourDurationUnitResKey = ytd.TourDurationUnitResKey,
                         TourDurationValue = ytd.TourDurationValue,
                         YachtFid = ytd.YachtFid,
                         YachtModel = ytd.YachtModel,
                         TourImageFileFid = ytd.TourImageFileFid,
                         TotalView = ytc.TotalViews,
                         TotalSuccessBooking = ytc.TotalSuccessBookings,
                         CurrencyCode = ytp.CurrencyCode,
                         CultureCode = ytp.CultureCode,
                         TourFee = TourPriceEstimate((TourPricingTypeEnum)ytp.TourPricingTypeFid, ytp.TourFee, ytp.MinimumPaxToGo, searchModel.Passenger)
                     }).OrderBy(sortString);

            //Get yacht tour counter

            var checkItems = query.ToList();

            if (query.Count() > 0)
                yachtTourSearchResult = new PagedList<YachtTourViewModel>(query, searchModel.PageIndex, searchModel.PageSize);
            return yachtTourSearchResult;
        }

        public IList<YachtTourViewModel> GetYachTourRelated(int locationId, DateTime DepartTime, DateTime ReturnTime, int pax)
        {
            return new List<YachtTourViewModel>();
        }

        public YachtTourDetailModel GetYachtTourDetail(int tourId, int langId = 1)
        {
            var yachtTourDetail = new YachtTourDetailModel();
            var yachtTour = _yachtDbContext.YachtTours.AsNoTracking()
                .Where(x => x.Deleted == false && x.Id == tourId)
                .Select(x => x);

            var query = from t in yachtTour
                        join i in _yachtDbContext.YachtTourInformations.AsNoTracking() on t.Id equals i.TourFid
                        join id in _yachtDbContext.YachtTourInformationDetails.AsNoTracking() on i.Id equals id.InformationFid
                        join c in _yachtDbContext.YachtTourCounters.AsNoTracking() on t.Id equals c.YachtTourId
                        where i.Deleted == false && id.LanguageFid == langId
                        select new YachtTourDetailModel
                        {
                            TourId = t.Id,
                            TourName = t.TourName,
                            TotalView = c.TotalViews,
                            TotalSuccessBooking = c.TotalSuccessBookings,
                            RatingStar = 4,
                            ShortDescription = id.ShortDescriptions,
                            FullDescription = id.FullDescriptions,
                            TourCategoryFid = t.TourCategoryFid,
                            TourCategoryResKey = t.TourCategoryResKey,
                        };

            if (query.Count() > 0)
                yachtTourDetail = query.FirstOrDefault();
            return yachtTourDetail;
        }

        public List<YachtTourFileStreamModel> GetYachtTourFileStream(int tourId, int fileCategoryFid)
        {
            var query = _yachtDbContext.YachtTourFileStreams.AsNoTracking()
                .Where(x => x.Deleted == false 
                && x.YachtTourFid == tourId 
                && x.FileCategoryFid == fileCategoryFid)
                .Select(x => new YachtTourFileStreamModel
                {
                    FileCategoryFid = x.FileCategoryFid,
                    FileCategoryResKey = x.FileCategoryResKey,
                    FileStreamFid = x.FileStreamFid,
                    FileTypeFid = x.FileTypeFid
                });
            var lstYachtTourFileStream = query.ToList();
            return lstYachtTourFileStream;
        }

        public List<YachtTourAttributeModel> GetYachtTourAttribute(int tourId)
        {
            var query = from ytAttrVal in _yachtDbContext.YachtTourAttributeValues.AsNoTracking()
                        join ytAttr in _yachtDbContext.YachtTourAttributes.AsNoTracking() on ytAttrVal.AttributeFid equals ytAttr.Id
                        where ytAttr.Deleted == false && ytAttrVal.YachtTourFid == tourId
                        select new YachtTourAttributeModel
                        {
                            AttributeCategoryFid = ytAttr.AttributeCategoryFid,
                            AttributeFid = ytAttrVal.AttributeFid,
                            AttributeName = ytAttr.AttributeName,
                            AttributeValue = ytAttrVal.AttributeValue,
                            IconClassCss = ytAttr.IconCssClass,
                            BasedAffective = ytAttrVal.BasedAffective,
                            Resourcekey = ytAttr.ResourceKey
                        };
            var lstYachtTourAttribute = query.ToList();
            return lstYachtTourAttribute;
        }

        public YachtTourYachtModel GetYachtTourYacht(int yachtId, int langId)
        {
            var yacht = new YachtTourYachtModel();
            var query = from y in _yachtDbContext.Yachts.AsNoTracking()
                        join yi in _yachtDbContext.YachtInformations on y.Id equals yi.YachtFid
                        join yid in _yachtDbContext.YachtInformationDetails on yi.Id equals yid.InformationFid
                        where y.Deleted == false && yi.Deleted == false && y.Id == yachtId && yid.LanguageFid == langId
                        orderby yi.ActivatedDate descending
                        select new YachtTourYachtModel
                        {
                            YachtId = y.Id,
                            YachtName = y.Name,
                            YachtModel = y.YachtTypeResKey,
                            ShortDescription = yid.ShortDescriptions,
                            Size = $"{y.LengthMeters} x {y.BeamMeters}",
                            Cabins = y.Cabins,
                            CrewMembers = y.CrewMembers,
                            EngineGenerators = y.EngineGenerators,
                            MaxPassengers = y.MaxPassengers,
                            YachtDefaultImage = _yachtDbContext.YachtFileStreams.AsNoTracking()
                                                  .Where(x => x.Deleted == false
                                                  && x.YachtFid == y.Id
                                                  && x.FileTypeFid == (int)FileTypeEnum.RefImage
                                                  && x.FileCategoryFid == (int)FileCategoriesEnum.ExtoriorPicture)
                                                  .Select(x => x.FileStreamFid).FirstOrDefault(),
                            ListYachtFileStream = _yachtDbContext.YachtFileStreams.AsNoTracking()
                                                  .Where(x => x.Deleted == false 
                                                  && x.YachtFid == y.Id 
                                                  && x.FileTypeFid == (int)FileTypeEnum.Image
                                                  && x.FileCategoryFid == (int)FileCategoriesEnum.ExtoriorPicture)
                                                  .Select(x => new YachtFileStreamViewModel
                                                  {
                                                      FileStreamFid = x.FileStreamFid
                                                  }).ToList()
                        };

            if (query.Count() > 0)
                yacht = query.FirstOrDefault();
            return yacht;
        }

        public YachtTourPriceResultModel GetYachtTourPrice(int tourId = 0, int yachtId = 0, int pax = 0)
        {
            var priceReturn = new YachtTourPriceResultModel();
            var query = _yachtDbContext.YachtTourPricings.AsNoTracking()
                .Where(x => x.Deleted == false && x.TourFid == tourId && x.YachtFid == yachtId && x.EffectiveDate <= DateTime.Now && DateTime.Now <= x.EffectiveEndDate);
            var tourPrice = new YachtTourPricings();
            if (query.Count() > 0)
            {
                tourPrice = query.OrderBy("EffectiveDate DESC").FirstOrDefault();
                var priceTemp = TourPriceEstimate((TourPricingTypeEnum)tourPrice.TourPricingTypeFid, tourPrice.TourFee, tourPrice.MinimumPaxToGo, pax);
                priceReturn.Price = priceTemp;
                priceReturn.CulturCode = tourPrice.CultureCode;
            }

            return priceReturn;
        }

        public YachtTourCharterResultModel CreateYachtTourCharter(YachtTourCharterCreateModel model)
        {
            using (var trans = _yachtDbContext.Database.BeginTransaction())
            {
                try
                {
                    var result = new YachtTourCharterResultModel();

                    var tourPrice = _yachtDbContext.YachtTourPricings.AsNoTracking()
                        .Where(x => x.TourFid == model.YachtTourFid && x.YachtFid == model.YachtFid && x.EffectiveDate <= DateTime.Now && DateTime.Now <= x.EffectiveEndDate)
                        .OrderBy("CreatedDate DESC").FirstOrDefault();

                    //Insert into the table yachttourcharters
                    var yachtTourCharter = new YachtTourCharters();
                    yachtTourCharter = _mapper.Map<YachtTourCharterCreateModel, YachtTourCharters>(model, yachtTourCharter);
                    yachtTourCharter.UniqueId = UniqueIDHelper.GenarateRandomString(12);
                    yachtTourCharter.SourceResKey = "TEMP";
                    yachtTourCharter.IsExistingCustomer = true;
                    yachtTourCharter.YachtPortFid = 0;
                    yachtTourCharter.DateFrom = DateTime.Now;
                    yachtTourCharter.DateTo = DateTime.Now;
                    yachtTourCharter.BookingDate = DateTime.Now;
                    yachtTourCharter.StatusFid = (int)YachtCharterStatusEnum.Waiting;
                    yachtTourCharter.Processed = true;
                    yachtTourCharter.GotSpecialRequest = false;
                    yachtTourCharter.PaymentExchangeRate = 0;

                    if (tourPrice != null)
                    {
                        yachtTourCharter.CultureCode = tourPrice.CultureCode;
                        yachtTourCharter.CurrencyCode = tourPrice.CurrencyCode;
                    }

                    _yachtDbContext.YachtTourCharters.Add(yachtTourCharter);
                    _yachtDbContext.SaveChanges();

                    //Insert into the table yachttourcharterdetail
                    var yachtTourCharterDetail = new YachtTourCharterDetails();
                    yachtTourCharterDetail.TourCharterFid = yachtTourCharter.Id;

                    //Insert into the table yachttourcharterpaymentlogs
                    var yachtTourCharterPaymentLog = new YachtTourCharterPaymentLogs();
                    yachtTourCharterPaymentLog.TourCharterFid = yachtTourCharter.Id;
                    yachtTourCharterPaymentLog.PaymentDate = DateTime.Now;
                    yachtTourCharterPaymentLog.PaymentAmount = yachtTourCharter.PaymentValue;
                    yachtTourCharterPaymentLog.StatusFid = yachtTourCharter.StatusFid;
                    _yachtDbContext.YachtTourCharterPaymentLogs.Add(yachtTourCharterPaymentLog);
                    _yachtDbContext.SaveChanges();

                    trans.Commit();
                    trans.Dispose();

                    result.UniqueId = yachtTourCharter.UniqueId;
                    result.CultureCode = yachtTourCharter.CultureCode;
                    result.CurrencyCode = yachtTourCharter.CurrencyCode;
                    result.PrepaidValue = yachtTourCharter.PrepaidValue;

                    return result;
                }
                catch
                {
                    trans.Rollback();
                    trans.Dispose();
                    throw;
                }
            }
        }

        public YachtTourCharterViewModel GetYachtTourCharterByUniqueId(string uniqueId)
        {
            var result = new YachtTourCharterViewModel();
            var query = _yachtDbContext.YachtTourCharters.AsNoTracking().Where(x => x.UniqueId == uniqueId).Select(x => _mapper.Map<YachtTourCharterViewModel>(x));
            if (query.Count() > 0)
                result = query.FirstOrDefault();
            return result;
        }

        public bool UpdateYachtTourCharter(YachtTourCharterUpdateModel model)
        {
            using (var trans = _yachtDbContext.Database.BeginTransaction())
            {
                try
                {
                    var tourCharter = _yachtDbContext.YachtTourCharters.AsNoTracking().Where(x => x.UniqueId == model.CharterUniqueId).FirstOrDefault();
                    if (tourCharter == null)
                        return false;

                    tourCharter.StatusFid = model.StatusId;
                    _yachtDbContext.YachtTourCharters.Update(tourCharter);
                    _yachtDbContext.SaveChanges();

                    var tourCharterPaymentLog = _yachtDbContext.YachtTourCharterPaymentLogs.AsNoTracking().Where(x => x.TourCharterFid == tourCharter.Id).FirstOrDefault();
                    if (tourCharterPaymentLog == null)
                        return false;

                    tourCharterPaymentLog.PaymentBy = model.CreatedUser;
                    tourCharterPaymentLog.PaymentRef = model.TransactionId;
                    tourCharterPaymentLog.StatusFid = model.StatusId;
                    _yachtDbContext.YachtTourCharterPaymentLogs.Update(tourCharterPaymentLog);
                    _yachtDbContext.SaveChanges();

                    trans.Commit();
                    trans.Dispose();

                    return true;
                }
                catch
                {
                    trans.Rollback();
                    trans.Dispose();
                    return false;
                }
            }
        }

        #endregion
    }
}
