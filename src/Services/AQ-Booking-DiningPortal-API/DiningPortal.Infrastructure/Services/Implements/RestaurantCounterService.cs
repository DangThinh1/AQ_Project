using APIHelpers.Response;
using AutoMapper;
using AQDiningPortal.Core.Models.RestaurantCounters;
using AQDiningPortal.Infrastructure.Database;
using AQDiningPortal.Infrastructure.Database.Entities;
using AQDiningPortal.Infrastructure.Interfaces;
using System;
using System.Linq;

namespace AQDiningPortal.Infrastructure.Services
{
    public class RestaurantCounterService : IRestaurantCounterService
    {
        private readonly DiningSearchContext _searchContext;
        private readonly IMapper _mapper;
        public RestaurantCounterService(DiningSearchContext searchContext, IMapper mapper)
        {
            _searchContext = searchContext;
            _mapper = mapper;
        }

        public BaseResponse<RestaurantCounters> Create(RestaurantCounterCreateModel createModel)
        {
            try
            {
                if(createModel == null)
                    return BaseResponse<RestaurantCounters>.BadRequest();
                var entity = _mapper.Map<RestaurantCounterCreateModel, RestaurantCounters>(createModel);
                entity.RestaurantUniqueId = string.Empty;
                var restaurant = _searchContext.Restaurants.Find(createModel.RestaurantFid);
                if(restaurant != null)
                    entity.RestaurantUniqueId = restaurant.UniqueId;
                _searchContext.RestaurantCounters.Add(entity);
                var result = _searchContext.SaveChanges();
                if(result != 1)
                    return BaseResponse<RestaurantCounters>.InternalServerError();
                return BaseResponse<RestaurantCounters>.Success(entity);
            }
            catch (Exception ex)
            {
                return BaseResponse<RestaurantCounters>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<RestaurantCounters> TopupCounter(RestaurantCounterTopupModel topupModel)
        {
            try
            {
                if(topupModel == null)
                    return BaseResponse<RestaurantCounters>.NotFound();
                var counterDb = _searchContext.RestaurantCounters.Where(k => k.RestaurantId == topupModel.RestaurantFid).FirstOrDefault();

                if(counterDb == null)
                    return BaseResponse<RestaurantCounters>.NotFound();

                if (topupModel.TotalViews > 0)
                    counterDb.TotalViews += topupModel.TotalViews;
                if (topupModel.TotalReservations > 0)
                    counterDb.TotalReservations += topupModel.TotalReservations;
                if (topupModel.TotalSuccessReservations > 0)
                    counterDb.TotalSuccessReservations += topupModel.TotalSuccessReservations;
                if (topupModel.TotalReviews > 0)
                    counterDb.TotalReviews += topupModel.TotalReviews;
                if (topupModel.TotalRecommendeds > 0)
                    counterDb.TotalRecommendeds += topupModel.TotalRecommendeds;
                if (topupModel.TotalNotRecommendeds > 0)
                    counterDb.TotalNotRecommendeds += topupModel.TotalNotRecommendeds;

                var result =_searchContext.SaveChanges();
                if(result != 1)
                    return BaseResponse<RestaurantCounters>.InternalServerError();

                return BaseResponse<RestaurantCounters>.Success(counterDb);

            }
            catch(Exception ex)
            {
                return BaseResponse<RestaurantCounters>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<RestaurantCounters> IncreaseTotalViews(int restaurantFid)
        {
            var topupModel = new RestaurantCounterTopupModel()
            {
                RestaurantFid = restaurantFid,
                TotalViews = 1
            };
            return TopupCounter(topupModel);
        }

        public BaseResponse<RestaurantCounters> IncreaseTotalReservations(int restaurantFid)
        {
            var topupModel = new RestaurantCounterTopupModel()
            {
                RestaurantFid = restaurantFid,
                TotalReservations = 1
            };
            return TopupCounter(topupModel);
        }

        public BaseResponse<RestaurantCounters> IncreaseTotalSuccessReservations(int restaurantFid)
        {
            var topupModel = new RestaurantCounterTopupModel()
            {
                RestaurantFid = restaurantFid,
                TotalSuccessReservations = 1
            };
            return TopupCounter(topupModel);
        }

        public BaseResponse<RestaurantCounters> IncreaseTotalReviews(int restaurantFid)
        {
            var topupModel = new RestaurantCounterTopupModel()
            {
                RestaurantFid = restaurantFid,
                TotalReviews = 1
            };
            return TopupCounter(topupModel);
        }

        public BaseResponse<RestaurantCounters> IncreaseTotalRecommendeds(int restaurantFid)
        {
            var topupModel = new RestaurantCounterTopupModel()
            {
                RestaurantFid = restaurantFid,
                TotalRecommendeds = 1
            };
            return TopupCounter(topupModel);
        }

        public BaseResponse<RestaurantCounters> IncreaseTotalNotRecommendeds(int restaurantFid)
        {
            var topupModel = new RestaurantCounterTopupModel()
            {
                RestaurantFid = restaurantFid,
                TotalNotRecommendeds = 1
            };
            return TopupCounter(topupModel);
        }

    }
}
