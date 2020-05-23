using APIHelpers.Response;
using AutoMapper;
using AQDiningPortal.Core.Models.RestaurantVenue;
using AQDiningPortal.Infrastructure.Database;
using AQDiningPortal.Infrastructure.Database.Entities;
using AQDiningPortal.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AQDiningPortal.Infrastructure.Services
{
    public class RestaurantVenueService : IRestaurantVenueService
    {
        private readonly DiningSearchContext _searchContext;
        private readonly DinningVenueContext _venueContext;
        private readonly IMapper _mapper;
        public RestaurantVenueService(DiningSearchContext searchContext,
            IMapper mapper,
            DinningVenueContext venueContext)
        {
            _searchContext = searchContext;
            _mapper = mapper;
            _venueContext = venueContext;
        }
        public BaseResponse<List<RestaurantVenueViewModel>> GetVenueByRestauranID(int restaurantId)
        {
            var result = _venueContext.RestaurantVenues.AsNoTracking()
                          .Where(x => x.Deleted == false && x.RestaurantFid == restaurantId)
                          .Select(x => new RestaurantVenueViewModel
                          {
                              Id = x.Id,
                              DefaultName = x.DefaultName,
                              ActiveForOperation = x.ActiveForOperation,
                              Capacity = x.Capacity,
                              CreatedBy = x.CreatedBy,
                              RestaurantFid = x.RestaurantFid,
                              SeatedCapacity = x.SeatedCapacity,
                              SizeAreaMeter = x.SizeAreaMeter,
                              SizeAreaSqft = x.SizeAreaSqft,
                              StandingCapacity = x.StandingCapacity,
                              OrderBy = x.OrderBy,
                              venueInfo = _venueContext.RestaurantVenueInfoDetails.Where(y => y.Deleted == false && y.VenueFid == x.Id).Select(y => _mapper.Map<RestaurantVenueInfoDetails, RestaurantVenueInfoViewModel>(y)).ToList(),
                              venuePricing = _venueContext.RestaurantVenuePricings.Where(z => z.Deleted == false && z.VenueFid == x.Id).Select(z => _mapper.Map<RestaurantVenuePricings, RestaurantVenuePricingViewModel>(z)).ToList(),
                          }).ToList();

            if (result != null)
                return BaseResponse<List<RestaurantVenueViewModel>>.Success(result);
            return BaseResponse<List<RestaurantVenueViewModel>>.NotFound(result);
        }
    }
}
