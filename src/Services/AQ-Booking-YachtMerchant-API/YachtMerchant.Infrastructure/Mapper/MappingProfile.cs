using AQEncrypts;
using AutoMapper;
using ExtendedUtility;
using YachtMerchant.Core.Models.PortLocation;
using YachtMerchant.Core.Models.Yacht;
using YachtMerchant.Core.Models.YachtAttribute;
using YachtMerchant.Core.Models.YachtCharteringPaymentLogs;
using YachtMerchant.Core.Models.YachtCharteringProcessingFees;
using YachtMerchant.Core.Models.YachtCharterings;
using YachtMerchant.Core.Models.YachtCharteringSchedules;
using YachtMerchant.Core.Models.YachtExternalRefLinks;
using YachtMerchant.Core.Models.YachtFileStreams;
using YachtMerchant.Core.Models.YachtMerchantCharterFeeOptions;
using YachtMerchant.Core.Models.YachtMerchantProductVendors;
using YachtMerchant.Core.Models.YachtMerchantUsers;
using YachtMerchant.Core.Models.YachtPricingPlan;
using YachtMerchant.Core.Models.YachtTourAttributes;
using YachtMerchant.Core.Models.YachtTourAttributeValues;
using YachtMerchant.Core.Models.YachtTourCategory;
using YachtMerchant.Core.Models.YachtTourCharterPaymentLogs;
using YachtMerchant.Core.Models.YachtTourCharterProcessingFees;
using YachtMerchant.Core.Models.YachtTourCharters;
using YachtMerchant.Core.Models.YachtTourCharterSchedules;
using YachtMerchant.Core.Models.YachtTourCounters;
using YachtMerchant.Core.Models.YachtTourFileStream;
using YachtMerchant.Core.Models.YachtTourInformations;
using YachtMerchant.Core.Models.YachtTourNonOperationDays;
using YachtMerchant.Core.Models.YachtTourOperationDetails;
using YachtMerchant.Core.Models.YachtTourPricings;
using YachtMerchant.Core.Models.YachtTours;
using YachtMerchant.Infrastructure.Database.Entities;

namespace YachtMerchant.Infrastructure.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Yacht

            CreateMap<Yachts, YachtCreateModel>();
            CreateMap<YachtCreateModel, Yachts>();
            CreateMap<YachtUpdateModel, Yachts>();

            #endregion Yacht

            #region YachtAttributeValue

            CreateMap<YachtAttributeValues, YachtAttributeCreateModel>();
            CreateMap<YachtAttributeCreateModel, YachtAttributeValues>();

            #endregion YachtAttributeValue

            #region YachtChartering

            CreateMap<YachtCharterings, YachtCharteringsViewModel>();
            CreateMap<YachtCharterings, YachtCharteringsDetailModel>();

            #endregion YachtChartering

            #region YachtCharteringProcessingFees

            CreateMap<YachtCharteringProcessingFees, YachtCharteringProcessingFeesViewModel>();

            #endregion YachtCharteringProcessingFees

            #region YachtCharteringPaymentLogs

            CreateMap<YachtCharteringPaymentLogs, YachtCharteringPaymentLogsViewModel>();

            #endregion YachtCharteringPaymentLogs

            #region YachtCharteringSchedules

            CreateMap<YachtCharteringSchedules, YachtCharteringSchedulesViewModel>();

            #endregion YachtCharteringSchedules

            #region YachtMerchantCharterFeeOptions

            CreateMap<YachtMerchantCharterFeeOptions, YachtMerchantCharterFeeOptionsViewModel>();

            #endregion YachtMerchantCharterFeeOptions

            #region YachtMerchantUsers

            CreateMap<YachtMerchantUsers, YachtMerchantUsersViewModel>();

            #endregion YachtMerchantUsers

            #region YachtCalendar

            //CreateMap<YachtCharterings, YachtCalendar>()
            //    .ForMember(model => model.Id, entity => entity.MapFrom(k => k.Id))
            //    .ForMember(model => model.YachtId, entity => entity.MapFrom(k => k.YachtFid))
            //    .ForMember(model => model.Start, entity => entity.MapFrom(k => k.CharterDateFrom))
            //    .ForMember(model => model.End, entity => entity.MapFrom(k => k.CharterDateTo))
            //    .ForMember(model => model.Status, entity => entity.MapFrom(k => 1));

            //CreateMap<YachtNonOperationDays, YachtCalendar>()
            //    .ForMember(model => model.Id, entity => entity.MapFrom(k => k.Id))
            //    .ForMember(model => model.YachtId, entity => entity.MapFrom(k =>k.YachtFid))
            //    .ForMember(model => model.Start, entity => entity.MapFrom(k => k.StartDate))
            //    .ForMember(model => model.End, entity => entity.MapFrom(k => k.EndDate))
            //    .ForMember(model => model.Status, entity => entity.MapFrom(k => 2));

            #endregion YachtCalendar

            #region YachtFileStream

            CreateMap<YachtFileStreams, YachtFileStreamViewModel>();
            CreateMap<YachtFileStreamCreateModel, YachtFileStreams>();
            CreateMap<YachtFileStreamUpdateModel, YachtFileStreams>();

            #endregion YachtFileStream

            #region YachtPricingPlans

            CreateMap<YachtPricingPlans, YachtPricingPlanCreateModel>();
            CreateMap<YachtPricingPlanCreateModel, YachtPricingPlans>();

            CreateMap<YachtPricingPlanDetails, YachtPricingPlanCreateModel>();
            CreateMap<YachtPricingPlanCreateModel, YachtPricingPlanDetails>();

            CreateMap<YachtPricingPlanInformations, YachtPricingPlanCreateModel>();
            CreateMap<YachtPricingPlanCreateModel, YachtPricingPlanInformations>();

            CreateMap<YachtPricingPlans, YachtPricingPlanViewModel>();
            CreateMap<YachtPricingPlanViewModel, YachtPricingPlans>();

            CreateMap<YachtPricingPlanDetails, YachtPricingPlanViewModel>();
            CreateMap<YachtPricingPlanViewModel, YachtPricingPlanDetails>();

            CreateMap<YachtPricingPlanInformations, YachtPricingPlanViewModel>();
            CreateMap<YachtPricingPlanViewModel, YachtPricingPlanInformations>();

            CreateMap<YachtPricingPlanDetailCreateModel, YachtPricingPlanDetails>();
            CreateMap<YachtPricingPlanDetails, YachtPricingPlanDetailCreateModel>();

            CreateMap<YachtMerchantProductVendors, YachtMerchantProductVendorViewModel>();
            CreateMap<YachtMerchantProductVendorCreateModel, YachtMerchantProductVendors>();
            CreateMap<YachtMerchantProductVendorUpdateModel, YachtMerchantProductVendors>();

            CreateMap<YachtPricingPlanInformationCreateModel, YachtPricingPlanInformations>();
            CreateMap<YachtPricingPlanInformations, YachtPricingPlanInformationCreateModel>();

            #endregion YachtPricingPlans

            #region YachtPort

            CreateMap<PortLocations, PortLocationViewModel>();

            #endregion YachtPort

            #region YachtTour

            CreateMap<YachtTours, YachTourViewModel>()
                .ForMember(model => model.Id, entity => entity.MapFrom(k => Terminator.Encrypt(k.Id.ToString())))
                .ForMember(model => model.MerchantFid, entity => entity.MapFrom(k => Terminator.Encrypt(k.MerchantFid.ToString())));
            CreateMap<YachtTourCreateModel, YachtTours>();
            CreateMap<YachtTourUpdateModel, YachtTours>();

            CreateMap<YachtTourCategories, YachtTourCategoryViewModel>();

            #region YachtTourFileStream Models

            CreateMap<YachtTourFileStreamCreateModel, YachtTourFileStreams>();
            CreateMap<YachtTourFileStreams, YachtTourFileStreamViewModel>()
                .ForMember(model => model.YachtTourFid, entity => entity.MapFrom(k => Terminator.Encrypt(k.YachtTourFid.ToString())));

            #endregion YachtTourFileStream Models

            CreateMap<YachtTourExternalRefLinks, YachtTourExternalRefLinkModel>()
                .ForMember(model => model.YachtTourFid, entity => entity.MapFrom(k => Terminator.Encrypt(k.YachtTourFid.ToString())));

            #region YachtTourNonOperationDay Models

            CreateMap<YachtTourNonOperationDayCreateModel, YachtTourNonOperationDays>()
                .ForMember(model => model.YachtTourFid, entity => entity.MapFrom(k => Terminator.Decrypt(k.YachtTourFid).ToInt32()))
                .ForMember(model => model.YachtFid, entity => entity.MapFrom(k => Terminator.Decrypt(k.YachtFid).ToInt32()));
            CreateMap<YachtTourNonOperationDays, YachtTourNonOperationDayViewModel>()
                .ForMember(model => model.YachtTourFid, entity => entity.MapFrom(k => Terminator.Encrypt(k.YachtTourFid.ToString())))
                .ForMember(model => model.YachtFid, entity => entity.MapFrom(k => Terminator.Encrypt(k.YachtFid.ToString())));

            #endregion YachtTourNonOperationDay Models

            #region YachtTourCounter Models

            CreateMap<YachtTourCounters, YachtTourCounterViewModel>()
                .ForMember(model => model.YachtTourId, entity => entity.MapFrom(k => Terminator.Encrypt(k.YachtTourId.ToString())));
            CreateMap<YachtTourCounterCreateModel, YachtTourCounters>()
                .ForMember(model => model.YachtTourId, entity => entity.MapFrom(k => Terminator.Decrypt(k.YachtTourId).ToInt32()));

            #endregion YachtTourCounter Models

            #region YachtTourOperationDetail Models

            CreateMap<YachtTourOperationDetails, YachtTourOperationDetailViewModel>()
                .ForMember(model => model.TourFid, entity => entity.MapFrom(k => Terminator.Encrypt(k.TourFid.ToString())))
                .ForMember(model => model.YachtFid, entity => entity.MapFrom(k => Terminator.Encrypt(k.YachtFid.ToString())))
                .ForMember(model => model.MerchantFid, entity => entity.MapFrom(k => Terminator.Encrypt(k.MerchantFid.ToString())));
            CreateMap<YachtTourOperationDetailCreateModel, YachtTourOperationDetails>()
                .ForMember(model => model.TourFid, entity => entity.MapFrom(k => Terminator.Decrypt(k.TourFid).ToInt32()))
                .ForMember(model => model.YachtFid, entity => entity.MapFrom(k => Terminator.Decrypt(k.YachtFid).ToInt32()))
                .ForMember(model => model.MerchantFid, entity => entity.MapFrom(k => Terminator.Decrypt(k.MerchantFid).ToInt32()));

            #endregion YachtTourOperationDetail Models

            #region YachtTourAttributes Models

            CreateMap<YachtTourAttributes, YachtTourAttributeViewModel>();

            #endregion YachtTourAttributes Models

            #region YachtTourAttributeValue Models

            CreateMap<YachtTourAttributeValues, YachtTourAttributeValueViewModel>()
                 .ForMember(model => model.YachtTourFid, entity => entity.MapFrom(k => Terminator.Encrypt(k.YachtTourFid.ToString())));
            CreateMap<YachtTourAttributeValueCreateModel, YachtTourAttributeValues>()
                .ForMember(model => model.YachtTourFid, entity => entity.MapFrom(k => Terminator.Decrypt(k.YachtTourFid).ToInt32()));

            #endregion YachtTourAttributeValue Models

            #region YachtTourPricings Models

            CreateMap<YachtTourPricings, YachtTourPricingViewModel>()
               .ForMember(model => model.TourFid, entity => entity.MapFrom(k => Terminator.Encrypt(k.TourFid.ToString())))
               .ForMember(model => model.YachtFid, entity => entity.MapFrom(k => Terminator.Encrypt(k.YachtFid.ToString())));

            #endregion YachtTourPricings Models
 
            #region YachtTourInformations

            CreateMap<YachtTourInformationDetails, YachtTourInformationUpdateDetailModel>();

            #endregion YachtTourInformations

            #endregion YachtTour

            #region YachtTourCharterPaymentLogs
            CreateMap<YachtTourCharterPaymentLogs, YachtTourCharterPaymentLogsViewModel>();
            #endregion

            #region YachtTourCharterProcessingFees

            CreateMap<YachtTourCharterProcessingFees, YachtTourCharterProcessingFeesViewModel>();
            #endregion

            # region YachtTourCharterSchedules

            CreateMap<YachtTourCharterSchedules, YachtTourCharterSchedulesViewModel>();
            #endregion

            #region YachtTourCharters
            CreateMap<YachtTourCharters, YachtTourCharterViewModel>();
            CreateMap<YachtTourCharters, YachtTourCharterDetailModel>();
            #endregion
        }
    }
}