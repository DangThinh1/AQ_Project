using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using YachtMerchant.Core.Models.YachtFileStreams;
using YachtMerchant.Core.Models.YachtTourPreview;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Database.Entities;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Infrastructure.Services
{
    public class PreviewYachtTourDetail : IPreviewYachtTourDetail
    {
        private readonly IMapper _mapper;
        private YachtOperatorDbContext _yachtDbContext;

        public PreviewYachtTourDetail(YachtOperatorDbContext db, IMapper mapper)
        {
            _yachtDbContext = db;
            _mapper = mapper;
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
                .Where(x => x.Deleted == false && x.YachtTourFid == tourId && x.FileCategoryFid == fileCategoryFid)
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
                            ShortDescription = yid.ShortDescriptions,
                            Size = y.LengthMeters.ToString(),
                            Cabins = y.Cabins,
                            CrewMembers = y.CrewMembers,
                            EngineGenerators = y.EngineGenerators,
                            MaxPassengers = y.MaxPassengers,
                            ListYachtFileStream = _yachtDbContext.YachtFileStreams.AsNoTracking().Where(x => x.Deleted == false && x.YachtFid == y.Id)
                                                  .Select(x => new YachtFileStreamViewModel
                                                  {
                                                      FileStreamFid = x.FileStreamFid
                                                  }).ToList()
                        };

            if (query.Count() > 0)
                yacht = query.FirstOrDefault();
            return yacht;
        }

        public string GetYachtTourPrice(int tourId = 0, int yachtId = 0, int pax = 0)
        {
            string result = string.Empty;
            var query = _yachtDbContext.YachtTourPricings.AsNoTracking()
                .Where(x => x.Deleted == false && x.TourFid == tourId && x.YachtFid == yachtId && x.EffectiveDate.Date <= DateTime.Now.Date);
            var tourPrice = new YachtTourPricings();
            if (query.Count() > 0)
            {
                tourPrice = query.OrderBy("EffectiveDate DESC").FirstOrDefault();
                result = TourPriceEstimate((TourPricingTypeEnum)tourPrice.TourPricingTypeFid, tourPrice.TourFee, tourPrice.MinimumPaxToGo, pax).ToString();
                return result + "," + tourPrice.CultureCode;
            }

            return result;
        }

        #region Utilities

        public double TourPriceEstimate(TourPricingTypeEnum pricingType, double tourFee, int maximumPaxToGo, int passenger)
        {
            if (pricingType == TourPricingTypeEnum.FlatFee)
            {
                return Math.Round(tourFee / passenger);
            }
            if (pricingType == TourPricingTypeEnum.BasePersion)
            {
                return tourFee;
            }
            return tourFee;
        }

        #endregion Utilities

        public enum TourPricingTypeEnum
        {
            FlatFee = 1,
            BasePersion = 2
        }
    }
}