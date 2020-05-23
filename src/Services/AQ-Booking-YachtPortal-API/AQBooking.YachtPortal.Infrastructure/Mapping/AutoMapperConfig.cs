using AQBooking.YachtPortal.Core.Helpers;
using AQBooking.YachtPortal.Core.Models.YachtAdditionalServices;
using AQBooking.YachtPortal.Core.Models.YachtAttributeValues;
using AQBooking.YachtPortal.Core.Models.YachtCharteringPaymentLogs;
using AQBooking.YachtPortal.Core.Models.YachtCharterings;
using AQBooking.YachtPortal.Core.Models.YachtCounters;
using AQBooking.YachtPortal.Core.Models.YachtFileStreams;
using AQBooking.YachtPortal.Core.Models.YachtInformationDetails;
using AQBooking.YachtPortal.Core.Models.YachtInformations;
using AQBooking.YachtPortal.Core.Models.YachtLanding;
using AQBooking.YachtPortal.Core.Models.YachtMerchantFileStreams;
using AQBooking.YachtPortal.Core.Models.YachtMerchantProductInventories;
using AQBooking.YachtPortal.Core.Models.YachtMerchants;
using AQBooking.YachtPortal.Core.Models.YachtPricingPlanDetails;
using AQBooking.YachtPortal.Core.Models.YachtPricingPlanInfomation;
using AQBooking.YachtPortal.Core.Models.Yachts;
using AQBooking.YachtPortal.Core.Models.YachtTourCharter;
using AQBooking.YachtPortal.Infrastructure.Entities;
using AQBooking.YachtPortal.Infrastructure.Helpers;
using AQEncrypts;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AQBooking.YachtPortal.Infrastructure.Mapping
{
    public class AutoMapperConfig : Profile
    {
        private DateTime _NOW_DATE { get => DateTime.Now; }
        private IMapper _mapper => DependencyInjectionHelper.GetService<IMapper>();
        public AutoMapperConfig()
        {
            #region Yacht Mapper
            CreateMap<Yachts, YachtDetailModel>();
            CreateMap<YachtDetailModel, Yachts>();
            CreateMap<YachtCounters, YachtCounterViewModel>();
            CreateMap<YachtPricingPlanDetails, YachtPricingPlanDetailViewModel>();
            CreateMap<YachtInformations, YachtInformationViewModel>();
            CreateMap<YachtInformationDetails, YachtInformationDetailViewModel>();
            CreateMap<YachtAdditionalServices, YachtAdditionalPackageViewModel>();
            CreateMap<YachtMerchantFileStreams, YachtMerchantFileStreamsViewModel>();
            CreateMap<YachtCharteringDetails, YachtCharteringDetailViewModel>();
            CreateMap<YachtTourCharters, YachtTourCharterViewModel>();
          //  CreateMap<YachtMerchantProductInventories, YachtMerchantProductInventoriesViewModel>();

            CreateMap<Yachts, YachtSingleViewModel>()
                .ForMember(k => k.Id, opt => opt.MapFrom(src => Terminator.Encrypt(src.Id.ToString())))
                .ForMember(k => k.MerchantFid, opt => opt.MapFrom(src => Terminator.Encrypt(src.MerchantFid.ToString())));


            CreateMap<YachtMerchantProductInventories, YachtMerchantProductInventoriesViewModel>()
                .ForMember(k => k.MerchantFid, opt => opt.MapFrom(src => Terminator.Encrypt(src.MerchantFid!=null? src.MerchantFid.ToString():"0")));

            CreateMap<YachtCharteringPaymentLogs, YachtCharteringPaymentLogViewModel>()
               .ForMember(k => k.Id, opt => opt.MapFrom(src => Terminator.Encrypt(src.Id.ToString())))
               .ForMember(k => k.CharteringFid, opt => opt.MapFrom(src => Terminator.Encrypt(src.CharteringFid.ToString())));

            CreateMap<YachtPricingPlanInformations, YachtPricingPlanInfomationViewModel>();
            CreateMap<YachtCharterings, YachtCharteringViewModel>();
            CreateMap<YachtCharteringViewModel, YachtCharterings>();

            #region Map YachtAttributeValues
            CreateMap<List<YachtAttributeValues>, List<YachtAttributeValueViewModel>>()
                .ConvertUsing(e => e.Where(k => !k.EffectiveDate.HasValue || k.EffectiveDate.Value.Date <= _NOW_DATE)
                                   .Select(k => _mapper.Map<YachtAttributeValues, YachtAttributeValueViewModel>(k))
                                   .ToList());

            #endregion Map YachtAttributeValues

            #region Map YachtFileStreams
            //=> Map YachtFileStreams
            CreateMap<YachtFileStreams, YachtFileStreamViewModel>();
            CreateMap<List<YachtFileStreams>, List<YachtFileStreamViewModel>>()
                .ConvertUsing(e => e.Where(k => !k.Deleted && k.ActivatedDate.Date <= _NOW_DATE)
                                   .Select(k => _mapper.Map<YachtFileStreams, YachtFileStreamViewModel>(k)
                                   )
                                   .ToList()
                             );
            #endregion Map YachtFileStreams

            #region YachtTourCharter
            CreateMap<YachtTourCharterCreateModel, YachtTourCharters>()
                .ForMember(x => x.Id, x => x.Ignore());
            CreateMap<YachtTourCharters, YachtTourCharterCreateModel>();
            CreateMap<YachtTourCharterPaymentLogCreateModel, YachtTourCharterPaymentLogs>()
                .ForMember(x => x.Id, x => x.Ignore());
            CreateMap<YachtTourCharterDetailCreateModel, YachtTourCharterDetails>();
            #endregion

            #region YachtLanding
            //YachtMerchants roles = null;
            CreateMap<Yachts, YachtLandingViewModel>();

            CreateMap<YachtLandingViewModel, Yachts>();
            CreateMap<(Yachts, YachtMerchants), YachtLandingViewModel>()
                .ForMember(x => x.MerchantName, k => k.MapFrom(y => y.Item2.MerchantName))
                .ForMember(x => x.MerchantCountry, k => k.MapFrom(y => y.Item2.Country))
                .ForMember(x => x.MerchantCity, k => k.MapFrom(y => y.Item2.City))
                .ForMember(x => x.MerchantEmail1, k => k.MapFrom(y => y.Item2.EmailAddress1))
                .ForMember(x => x.MerchantContactNum1, k => k.MapFrom(y => y.Item2.ContactNumber1))                
                .ForMember(x => x.MerchantFIDEncypt, k => k.MapFrom(y => Terminator.Encrypt(y.Item2.Id.ToString())))
                .ForMember(x => x.Address1, k => k.MapFrom(y => y.Item2.Address1))
                .ForMember(x => x.LandingPageOptionFid, k => k.MapFrom(y => y.Item2.LandingPageOptionFid))
                .ForMember(x => x.MerchantFid, k => k.MapFrom(y => y.Item2.Id))
                .ForMember(x => x.IdEncrypt, k => k.MapFrom(y => Terminator.Encrypt(y.Item1.Id.ToString())))
                .ForMember(x => x.Name, k => k.MapFrom(y => y.Item1.Name))
                .ForMember(x => x.City, k => k.MapFrom(y => y.Item1.City))
                .ForMember(x => x.Country, k => k.MapFrom(y => y.Item1.Country))
                .ForMember(x => x.ID, k => k.MapFrom(y => y.Item1.Id))                
                .ForMember(x => x.Cabins, k => k.MapFrom(y => y.Item1.Cabins))
                .ForMember(x => x.EngineGenerators, k => k.MapFrom(y => y.Item1.EngineGenerators))
                .ForMember(x => x.LengthMeters, k => k.MapFrom(y => y.Item1.LengthMeters))
                .ForMember(x => x.CharterCategoryFid, k => k.MapFrom(y => y.Item1.CharterCategoryFid))
                .ForMember(x => x.CharterTypeFid, k => k.MapFrom(y => y.Item1.CharterTypeFid))
                .ForMember(x => x.CharterTypeReskey, k => k.MapFrom(y => y.Item1.CharterTypeResKey));


            //CreateMap<YachtMerchants, YachtMerchantViewModel>();
            //CreateMap<YachtMerchantViewModel, YachtMerchants>();
            #endregion

            #endregion Yacht Mapper         
        }
    }
}
