using AccommodationMerchant.Core.Helpers;
using AccommodationMerchant.Core.Models.HotelAttributes;
using AccommodationMerchant.Core.Models.HotelFileStreamModel;
using AccommodationMerchant.Core.Models.HotelInformationDetails;
using AccommodationMerchant.Core.Models.HotelInformations;
using AccommodationMerchant.Core.Models.HotelInventories;
using AccommodationMerchant.Core.Models.HotelMerchantUsers;
using AccommodationMerchant.Core.Models.HotelReservationPaymentLogs;
using AccommodationMerchant.Core.Models.HotelReservationProcessingFees;
using AccommodationMerchant.Core.Models.HotelReservations;
using AccommodationMerchant.Core.Models.Hotels;
using AccommodationMerchant.Infrastructure.Databases.Entities;
using AQEncrypts;
using AutoMapper;
using ExtendedUtility;
using HotelInformationDetailViewModel = AccommodationMerchant.Core.Models.HotelInformations.HotelInformationDetailViewModel;

namespace AccommodationMerchant.Infrastructure.Mappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //HotelAttributes
            CreateMap<HotelAttributes, HotelAttributeViewModel>();

            // HotelMerchantUsers
            CreateMap<HotelMerchantUsers, HotelMerchantUserViewModel>();

            // HotelReservationPaymentLogs
            CreateMap<HotelReservationPaymentLogs, HotelReservationPaymentLogViewModel>();

            // HotelReservationProcessingFees
            CreateMap<HotelReservationProcessingFees, HotelReservationProcessingFeeViewModel>();

            // HotelReservations
            CreateMap<HotelReservations, HotelReservationShowDashBoardModel>();
            CreateMap<HotelReservations, HotelReservationViewModel>();

            //HotelInventoryAttributes
            CreateMap<HotelInventoryAttributes, HotelInventoryAttributeViewModel>();

            //Hotels
            CreateMap<HotelCreateModel, Hotels>();
            CreateMap<Hotels, HotelViewModel>()
                .ForMember(viewModel => viewModel.InventoriesCount, entity => entity.MapFrom(k => k.Inventories.Count))
                .ForMember(viewModel => viewModel.Id, entity => entity.MapFrom(k => Terminator.Encrypt(k.Id.ToString())))
                .ForMember(viewModel => viewModel.MerchantFid, entity => entity.MapFrom(k => Terminator.Encrypt(k.MerchantFid.ToString())));

            //HotelInformationDetails
            CreateMap<HotelInformationDetailCreateModel, HotelInformationDetails>();

            CreateMap<HotelInformationDetails, HotelInformationDetailViewModel>();
             //.ForMember(viewModel => viewModel.ActivatedDate, entity => entity.MapFrom(k => DateTimeHelper.ToString(k.ActivatedDate)));

            //HotelInformations
            CreateMap<HotelInformationCreateModel, HotelInformations>()
             .ForMember(viewModel => viewModel.HotelFid, entity => entity.MapFrom(k => Terminator.Decrypt(k.HotelFid.ToString())))
            .ForMember(entity => entity.ActivatedDate, createModel => createModel.MapFrom(k => k.ActivatedDate.ToDateTime()));
            //CreateMap<HotelInformations, HotelInformationViewModel>()
            //    .ForMember(viewModel => viewModel.ActivatedDate, entity => entity.MapFrom(k => DateTimeHelper.ToString(k.ActivatedDate)))
            //    .ForMember(viewModel => viewModel.HotelFid, entity => entity.MapFrom(k => Terminator.Encrypt(k.HotelFid.ToString())))
            //    .ForMember(viewModel => viewModel.Id, entity => entity.MapFrom(k => Terminator.Encrypt(k.Id.ToString())));

            //Hotel Inventories
            CreateMap<HotelInventoryCreateModel, HotelInventories>();
            CreateMap<HotelInventories, HotelInventoryViewModel>()
                .ForMember(viewModel => viewModel.HotelFid, entity => entity.MapFrom(k => Terminator.Encrypt(k.HotelFid.ToString())));

            #region HotelFileStreams

            CreateMap<HotelFileStreamCreateUpdateModel, HotelFileStreams>();
            CreateMap<HotelFileStreams, HotelFileStreamViewModel>();

            #endregion HotelFileStreams
        }
    }
}