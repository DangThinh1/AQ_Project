using System;
using AutoMapper;
using System.Linq;
using APIHelpers.Response;
using System.Collections.Generic;
using AQConfigurations.Infrastructure.Databases;
using AQConfigurations.Infrastructure.Databases.Entities;
using AQConfigurations.Infrastructure.Services.Interfaces;
using AQConfigurations.Core.Models.PortalLocationControls;

namespace AQConfigurations.Infrastructure.Services.Implements
{
    public class PortalLocationControlService : IPortalLocationControlService
    {
        private readonly AQConfigurationsDbContext _commonContext;
        private readonly IMapper _mapper;
        public PortalLocationControlService(AQConfigurationsDbContext commonContext, IMapper mapper)
        {
            _commonContext = commonContext;
            _mapper = mapper;
        }

        public BaseResponse<List<PortalLocationControlViewModel>> GetLocationsByPortalUniqueId(string uniqueId)
        {
            try
            {
                if (string.IsNullOrEmpty(uniqueId))
                    return BaseResponse<List<PortalLocationControlViewModel>>.BadRequest();
                var locations = _commonContext.PortalLocationControls
                    .Where(k => k.Deleted == false && k.IsActive == true && k.PortalUniqueId == uniqueId)
                    .OrderByDescending(k => k.IsExclusive)
                    .ThenBy(k => k.OrderBy)
                    .Select(k => _mapper.Map<PortalLocationControls, PortalLocationControlViewModel>(k)).ToList();
                if (locations == null || locations.Count == 0)
                    return BaseResponse<List<PortalLocationControlViewModel>>.NoContent();

                return BaseResponse<List<PortalLocationControlViewModel>>.Success(locations);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<PortalLocationControlViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<PortalLocationControlViewModel>> GetLocationsByPortalUniqueIdAndCountryCode(string uniqueId, int countryCode)
        {
            try
            {
                if (string.IsNullOrEmpty(uniqueId))
                    return BaseResponse<List<PortalLocationControlViewModel>>.BadRequest();
                var locations = _commonContext.PortalLocationControls
                    .Where(k => k.Deleted == false && k.IsActive == true && k.PortalUniqueId == uniqueId && k.CountryCode == countryCode)
                    .OrderByDescending(k => k.IsExclusive)
                    .ThenBy(k => k.OrderBy)
                    .Select(k => _mapper.Map<PortalLocationControls, PortalLocationControlViewModel>(k)).ToList();
                if (locations == null || locations.Count == 0)
                    return BaseResponse<List<PortalLocationControlViewModel>>.NoContent();

                return BaseResponse<List<PortalLocationControlViewModel>>.Success(locations);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<PortalLocationControlViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<PortalLocationControlViewModel>> GetLocationsByPortalUniqueIdAndCountryName(string uniqueId, string countryName)
        {
            try
            {
                if (string.IsNullOrEmpty(countryName) || string.IsNullOrEmpty(uniqueId))
                    return BaseResponse<List<PortalLocationControlViewModel>>.BadRequest();
                var locations = _commonContext.PortalLocationControls
                    .Where(k => k.Deleted == false && k.IsActive == true && k.PortalUniqueId == uniqueId && countryName.Equals(k.CountryName))
                    .OrderByDescending(k => k.IsExclusive)
                    .ThenBy(k => k.OrderBy)
                    .Select(k => _mapper.Map<PortalLocationControls, PortalLocationControlViewModel>(k)).ToList();
                if (locations == null || locations.Count == 0)
                    return BaseResponse<List<PortalLocationControlViewModel>>.NoContent();

                return BaseResponse<List<PortalLocationControlViewModel>>.Success(locations);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<PortalLocationControlViewModel>>.InternalServerError(message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
    }
}
