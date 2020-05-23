using APIHelpers.Response;
using System.Collections.Generic;

namespace AQDiningPortal.Infrastructure.Interfaces
{
    public interface IRestaurantVenueService
    {
        BaseResponse<List<Core.Models.RestaurantVenue.RestaurantVenueViewModel>> GetVenueByRestauranID(int restaurantId);
    }
}
