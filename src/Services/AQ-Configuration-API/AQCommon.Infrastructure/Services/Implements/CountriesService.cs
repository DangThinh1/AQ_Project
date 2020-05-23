using APIHelpers.Response;
using AQConfigurations.Core.Models.Countries;
using AQConfigurations.Infrastructure.Databases;
using AQConfigurations.Infrastructure.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQConfigurations.Infrastructure.Services.Implements
{
    public class CountriesService : ICountriesService
    {
        private readonly AQConfigurationsDbContext _commonContext;
        private readonly IMapper _mapper;
        public CountriesService(AQConfigurationsDbContext commonContext, IMapper mapper)
        {
            _commonContext = commonContext;
            _mapper = mapper;
        }

        public BaseResponse<List<CountriesViewModel>> GetAllCountries()
        {
            try
            {
                var listCountries = _commonContext.Countries
                    .AsNoTracking()
                    .Select(x => _mapper.Map<CountriesViewModel>(x))
                    .ToList();
                return listCountries != null 
                    ? BaseResponse<List<CountriesViewModel>>.Success(listCountries) 
                    : BaseResponse<List<CountriesViewModel>>.NoContent();
            }
            catch (Exception ex)
            {
                return BaseResponse<List<CountriesViewModel>>.InternalServerError(message:ex.Message, fullMsg:ex.StackTrace);
            }
        }

        public BaseResponse<CountriesViewModel> FindByCountryCode(int countryCode)
        {
            try
            {
                var country = _commonContext.Countries
                    .AsNoTracking()
                    .Select(x => _mapper.Map<CountriesViewModel>(x))
                    .FirstOrDefault(x => x.CountryCode == countryCode);
                return country != null
                    ? BaseResponse<CountriesViewModel>.Success(country)
                    : BaseResponse<CountriesViewModel>.NoContent();
            }
            catch (Exception ex)
            {
                return BaseResponse<CountriesViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public async Task<List<CountriesViewModel>> GetAllCountryAsync()
            => await _commonContext.Countries.AsNoTracking().Select(x => _mapper.Map<CountriesViewModel>(x)).ToListAsync();

        public async Task<CountriesViewModel> GetCountryByCodeAsync(int countryCode)
            => await _commonContext.Countries.AsNoTracking().Where(x => x.CountryCode == countryCode).Select(x => _mapper.Map<CountriesViewModel>(x)).FirstOrDefaultAsync();
    }
}
