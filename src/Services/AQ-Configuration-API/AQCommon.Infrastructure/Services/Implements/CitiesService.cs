using System;
using AutoMapper;
using System.Linq;
using APIHelpers.Response;
using System.Threading.Tasks;
using AQConfigurations.Core.Models.Cities;
using AQConfigurations.Infrastructure.Databases;
using AQConfigurations.Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace AQConfigurations.Infrastructure.Services.Implements
{
    public class CitiesService : ICitiesService
    {
        private readonly AQConfigurationsDbContext _commonContext;
        private readonly IMapper _mapper;
        public CitiesService(AQConfigurationsDbContext commonContext, IMapper mapper)
        {
            _commonContext = commonContext;
            _mapper = mapper;
        }

        public BaseResponse<List<CityViewModel>> GetAllCities()
        {
            try
            {
                var listCities = _commonContext.Cities
                    .AsNoTracking()
                    .Select(x => _mapper.Map<CityViewModel>(x))
                    .ToList();
                return listCities != null
                    ? BaseResponse<List<CityViewModel>>.Success(listCities)
                    : BaseResponse<List<CityViewModel>>.NoContent();
            }
            catch(Exception ex)
            {
                return BaseResponse<List<CityViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<CityViewModel>> GetByCountryCode(int countryCode)
        {
            try
            {
                var listCities = _commonContext.Cities
                    .AsNoTracking()
                    .Where(x => x.CountryCode == countryCode)
                    .Select(x => _mapper.Map<CityViewModel>(x))
                    .ToList();
                return listCities != null
                    ? BaseResponse<List<CityViewModel>>.Success(listCities)
                    : BaseResponse<List<CityViewModel>>.NoContent();
            }
            catch (Exception ex)
            {
                return BaseResponse<List<CityViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<CityViewModel>> GetByCountryName(string countryName)
        {
            try
            {
                var country = _commonContext.Countries.FirstOrDefault(k=> k.CountryName.ToUpper().Equals(countryName.ToUpper()));
                if (country == null && country.CountryCode != 0)
                    return BaseResponse<List<CityViewModel>>.BadRequest();
                var listCities = _commonContext.Cities
                    .AsNoTracking()
                    .Where(x => x.CountryCode == country.CountryCode)
                    .Select(x => _mapper.Map<CityViewModel>(x))
                    .ToList();
                return listCities != null
                    ? BaseResponse<List<CityViewModel>>.Success(listCities)
                    : BaseResponse<List<CityViewModel>>.NoContent();
            }
            catch (Exception ex)
            {
                return BaseResponse<List<CityViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<CityViewModel> FindByCityName(string cityName)
        {
            try
            {
                var city = _commonContext.Cities
                    .AsNoTracking()
                    .Select(x => _mapper.Map<CityViewModel>(x))
                    .FirstOrDefault(x => x.CityName.ToUpper().Contains(cityName.ToUpper()));
                return city != null
                    ? BaseResponse<CityViewModel>.Success(city)
                    : BaseResponse<CityViewModel>.NoContent();
            }
            catch (Exception ex)
            {
                return BaseResponse<CityViewModel>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<CityViewModel>> GetByListCityNames(List<string> cities)
        {
            try
            {
                var listCities = _commonContext.Cities
                    .AsNoTracking()
                    .Where(x => cities.Contains(x.CityName))
                    .Select(x => _mapper.Map<CityViewModel>(x))
                    .ToList();
                return listCities != null
                    ? BaseResponse<List<CityViewModel>>.Success(listCities)
                    : BaseResponse<List<CityViewModel>>.NoContent();
            }
            catch (Exception ex)
            {
                return BaseResponse<List<CityViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        #region .
        public async Task<List<CityViewModel>> GetAllCityAsync()
            => await _commonContext.Cities.AsNoTracking().Select(x => _mapper.Map<CityViewModel>(x)).ToListAsync();

        public async Task<List<CityViewModel>> GetCityByCodeAsync(int countryCode)
            => await _commonContext.Cities.AsNoTracking().Where(x=>x.CountryCode == countryCode).Select(x => _mapper.Map<CityViewModel>(x)).ToListAsync();

        public async Task<List<CityViewModel>> GetCityByCountryNameAsync(string countryName)
            => await _commonContext.Countries.AsNoTracking().Where(x=>x.CountryName == countryName).Select(x => _mapper.Map<CityViewModel>(x)).ToListAsync();

        public async Task<CityViewModel> GetCityByNameAsync(string name)
            => await _commonContext.Cities.AsNoTracking().Where(x => x.CityName.Contains(name)).Select(x => _mapper.Map<CityViewModel>(x)).FirstOrDefaultAsync();

        public async Task<List<CityViewModel>> GetCityDistrictLstByCityLstAsync(List<string> cityLst = null)
            => await _commonContext.Cities.AsNoTracking().Where(x => cityLst.Contains(x.CityName)).Select(x => _mapper.Map<CityViewModel>(x)).ToListAsync();
        #endregion
    }
}
