using Accommodation.Core.Models.HotelInformationDetails;
using Accommodation.Core.Models.HotelInformations;
using Accommodation.Core.Models.HotelReservationDetails;
using Accommodation.Core.Models.HotelReservations;
using Accommodation.Core.Models.Hotels;
using Accommodation.Infrastructure.Databases.Entities;
using AutoMapper;

namespace Accommodation.Infrastructure.Mappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Hotel
            CreateMap<Hotels, HotelViewModel>();
            CreateMap<HotelViewModel, Hotels>();

            CreateMap<HotelInformations, HotelInformationViewModel>();
            CreateMap<HotelInformationViewModel, HotelInformations>();

            CreateMap<HotelReservations, HotelReservationViewModel>();
            CreateMap<HotelReservationViewModel, HotelReservations>();

            CreateMap<HotelInformationDetails, HotelInformationDetailViewModel>();
            CreateMap<HotelInformationDetailViewModel, HotelInformationDetails>();

            CreateMap<HotelReservationDetails, HotelReservationDetailViewModel>();
            CreateMap<HotelReservationDetailViewModel, HotelReservationDetails>();
            #endregion
        }
    }
}
