using AutoMapper;
using AQConfigurations.Core.Models.CommonResources;
using AQConfigurations.Infrastructure.Databases.Entities;
using AQConfigurations.Core.Models.PortalLocationControls;
using AQConfigurations.Core.Models.CommonValues;
using AQConfigurations.Core.Models.Currencies;
using AQConfigurations.Core.Models.Cities;
using AQConfigurations.Core.Models.Countries;
using AQConfigurations.Core.Models.CommonLanguages;
using AQConfigurations.Core.Models.PortalLanguages;

namespace AQConfigurations.Infrastructure.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CommonValueCreateModel, CommonValues>();
            CreateMap<CommonValues, CommonValueViewModel>();
            CreateMap<CommonResources, CommonResourceViewModel>();
            CreateMap<CommonLanguages, CommonLanguagesViewModel>();
            CreateMap<PortalLanguageControls, PortalLanguageViewModel>()
                .ForMember(model => model.LanguageName, entity => entity.MapFrom(k => k.Language.LanguageName));

            CreateMap<PortalLocationControls, PortalLocationControlViewModel>();
            CreateMap<Currencies, CurrencyViewModel>();

            CreateMap<Cities, CityViewModel>();
            CreateMap<CityViewModel, Cities>();

            CreateMap<Countries, CountriesViewModel>();
            CreateMap<CountriesViewModel, Countries>();

            CreateMap<StateViewModel, ZoneDistricts>();
            CreateMap<ZoneDistricts, StateViewModel>();
        }
    }
}