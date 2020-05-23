using AutoMapper;
using System;
using System.Linq;
using AQDiningPortal.Core.Models.RestaurantAttributeValues;
using AQDiningPortal.Core.Models.RestaurantBusinessDayOperations;
using AQDiningPortal.Core.Models.RestaurantBusinessDays;
using AQDiningPortal.Core.Models.RestaurantCounters;
using AQDiningPortal.Core.Models.RestaurantFileStreams;
using AQDiningPortal.Core.Models.RestaurantInformationDetails;
using AQDiningPortal.Core.Models.RestaurantInformations;
using AQDiningPortal.Core.Models.RestaurantMenuInfoDetails;
using AQDiningPortal.Core.Models.RestaurantMenuPricings;
using AQDiningPortal.Core.Models.RestaurantMenus;
using AQDiningPortal.Core.Models.RestaurantNonBusinessDays;
using AQDiningPortal.Core.Models.RestaurantOtherInformations;
using AQDiningPortal.Core.Models.RestaurantReservationFees;
using AQDiningPortal.Core.Models.Restaurants;
using AQDiningPortal.Core.Models.RestaurantVenue;
using AQDiningPortal.Infrastructure.Database.Entities;


namespace AQDiningPortal.Infrastructure.Helpers
{
    public class AutoMapperProfile : Profile
    {
        private DateTime _NOW_DATE { get => DateTime.Now; }

        private  IMapper _mapper => DependencyInjectionHelper.GetService<IMapper>();

        public AutoMapperProfile()
        {
            // Add as many of these lines as you need to map your objects
            #region Dining Mapper
            CreateMap<RestaurantReservationFees, RestaurantReservationFeeViewModel>();
            CreateMap<Restaurants, RestaurantViewModel>();
            CreateMap<RestaurantInformations, RestaurantInformationViewModel>();
            CreateMap<RestaurantInformationDetails, RestaurantInformationDetailViewModel>();
            CreateMap<RestaurantOtherInformations, RestaurantOtherInformationViewModel>();
            CreateMap<RestaurantNonBusinessDays, RestaurantNonBusinessDayViewModel>();
            CreateMap<RestaurantFileStreams, RestaurantFileStreamViewModel>();
            CreateMap<RestaurantCounters, RestaurantCounterViewModel>();
            CreateMap<RestaurantMenuPricings, RestaurantMenuPricingViewModel>();
            CreateMap<RestaurantMenuInfoDetails, RestaurantMenuInfoDetailViewModel>();
            CreateMap<RestaurantBusinessDayOperations, RestaurantBusinessDayOperationModel>();
            CreateMap<RestaurantMenus, RestaurantMenuViewModel>()
                .ForMember(k=>k.InfoDetails, opt=>opt.MapFrom(src=> _mapper.Map<RestaurantMenuInfoDetails, RestaurantMenuInfoDetailViewModel>(src.InfoDetails.Where(k=>k.EffectiveDate.Date <= DateTime.Now.Date).OrderByDescending(k=>k.EffectiveDate).FirstOrDefault())))
                .ForMember(k => k.Pricings, opt => opt.MapFrom(src => _mapper.Map<RestaurantMenuPricings, RestaurantMenuPricingViewModel>(src.Pricings.Where(k => k.EffectiveDate.Date <= DateTime.Now.Date).OrderByDescending(k => k.EffectiveDate).FirstOrDefault())));
            CreateMap<RestaurantBusinessDays, RestaurantBusinessDayViewModel>()
                .ForMember(a=> a.BusinessDayOperation, b=>b.MapFrom(c=> _mapper.Map<RestaurantBusinessDayOperations, RestaurantBusinessDayOperationModel>(c.BusinessDayOperations.Where(d => d.EffectiveDate.Date <= DateTime.Now.Date).OrderByDescending(e => e.EffectiveDate).FirstOrDefault())));
            //2=>1
            CreateMap<(RestaurantAttributeValues, RestaurantAttributes), RestaurantAttributeValueViewModel>()
                .ForMember(a=>a.AttributeCategoryFid, b=>b.MapFrom(c=>c.Item1.AttributeCategoryFid))
                .ForMember(a => a.AttributeFid, b => b.MapFrom(c => c.Item1.AttributeFid))
                .ForMember(a => a.AttributeValue, b => b.MapFrom(c => c.Item1.AttributeValue))
                .ForMember(a => a.ResourceKey, b => b.MapFrom(c => c.Item2.ResourceKey));

            CreateMap<RestaurantVenuePricings, RestaurantVenuePricingViewModel>();
            CreateMap<RestaurantVenuePricingViewModel, RestaurantVenuePricings>();

            CreateMap<RestaurantVenueInfoDetails, RestaurantVenueInfoViewModel>();
            CreateMap<RestaurantVenueInfoViewModel, RestaurantVenueInfoDetails>();
            #endregion Dining Mapper
        }
    }
}
