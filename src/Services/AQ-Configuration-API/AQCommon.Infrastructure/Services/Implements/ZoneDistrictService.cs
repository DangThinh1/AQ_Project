using APIHelpers.Response;
using AQConfigurations.Core.Models.Cities;
using AQConfigurations.Infrastructure.Databases;
using AQConfigurations.Infrastructure.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace AQConfigurations.Infrastructure.Services.Implements
{
    public class ZoneDistrictService : IZoneDistrictService
    {
        private readonly AQConfigurationsDbContext _commonContext;
        private readonly IMapper _mapper;
        public ZoneDistrictService(AQConfigurationsDbContext commonContext, IMapper mapper)
        {
            _commonContext = commonContext;
            _mapper = mapper;
        }

        #region BaseResponse Format
        public BaseResponse<List<StateViewModel>> GetAllStates()
        {
            try
            {
                var listStates = _commonContext.ZoneDistricts
                    .AsNoTracking()
                    .Select(x => _mapper.Map<StateViewModel>(x))
                    .ToList();
                return listStates != null
                    ? BaseResponse<List<StateViewModel>>.Success(listStates)
                    : BaseResponse<List<StateViewModel>>.NoContent();
            }
            catch (Exception ex)
            {
                return BaseResponse<List<StateViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<StateViewModel>> GetStatesByCityCode(int cityCode)
        {
            try
            {
                var listStates = _commonContext.ZoneDistricts
                                 .AsNoTracking()
                                 .Where(x => x.CityCode == cityCode)
                                 .Select(x => _mapper.Map<StateViewModel>(x))
                                 .ToList();
                return listStates != null
                    ? BaseResponse<List<StateViewModel>>.Success(listStates)
                    : BaseResponse<List<StateViewModel>>.NoContent();
            }
            catch (Exception ex)
            {
                return BaseResponse<List<StateViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<StateViewModel>> GetStatesByZoneDistrictName(string zoneDistrictName)
        {
            try
            {
                var listStates = _commonContext
                    .ZoneDistricts.AsNoTracking()
                    .Where(x => x.ZoneDistrictName.ToUpper().Contains(zoneDistrictName.ToUpper()))
                    .Select(x => _mapper.Map<StateViewModel>(x))
                    .ToList();
                return listStates != null
                    ? BaseResponse<List<StateViewModel>>.Success(listStates)
                    : BaseResponse<List<StateViewModel>>.NoContent();
            }
            catch (Exception ex)
            {
                return BaseResponse<List<StateViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        #endregion BaseResponse Format

        public async Task<List<StateViewModel>> GetAllStatesAsync()
            => await _commonContext.ZoneDistricts.AsNoTracking().Select(x => _mapper.Map<StateViewModel>(x)).ToListAsync();

        public async Task<List<StateViewModel>> GetStatesByCityAsync(int countryCode)  
            => await _commonContext.ZoneDistricts.AsNoTracking().Where(x=>x.CityCode == countryCode).Select(x => _mapper.Map<StateViewModel>(x)).ToListAsync();

        public async Task<List<StateViewModel>> GetStatesByCityNameAsync(string countryName)
            => await _commonContext.ZoneDistricts.AsNoTracking().Where(x => x.ZoneDistrictName.Contains(countryName)).Select(x => _mapper.Map<StateViewModel>(x)).ToListAsync();
    }
}
