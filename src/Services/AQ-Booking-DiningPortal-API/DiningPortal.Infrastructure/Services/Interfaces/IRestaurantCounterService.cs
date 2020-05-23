using APIHelpers.Response;
using AQDiningPortal.Core.Models.RestaurantCounters;
using AQDiningPortal.Infrastructure.Database.Entities;

namespace AQDiningPortal.Infrastructure.Interfaces
{
    public interface IRestaurantCounterService
    {
        BaseResponse<RestaurantCounters> TopupCounter(RestaurantCounterTopupModel topupModel);

        BaseResponse<RestaurantCounters> IncreaseTotalViews(int restaurantFid);

        BaseResponse<RestaurantCounters> IncreaseTotalReservations(int restaurantFid);

        BaseResponse<RestaurantCounters> IncreaseTotalSuccessReservations(int restaurantFid);

        BaseResponse<RestaurantCounters> IncreaseTotalReviews(int restaurantFid);

        BaseResponse<RestaurantCounters> IncreaseTotalRecommendeds(int restaurantFid);

        BaseResponse<RestaurantCounters> IncreaseTotalNotRecommendeds(int restaurantFid);
    }
}
