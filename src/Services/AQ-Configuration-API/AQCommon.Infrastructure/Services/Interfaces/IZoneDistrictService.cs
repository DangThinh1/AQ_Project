using APIHelpers.Response;
using System.Threading.Tasks;
using System.Collections.Generic;
using AQConfigurations.Core.Models.Cities;

namespace AQConfigurations.Infrastructure.Services.Interfaces
{
    public interface IZoneDistrictService
    {
        #region BaseResponse Format
        BaseResponse<List<StateViewModel>> GetAllStates();
        BaseResponse<List<StateViewModel>> GetStatesByCityCode(int cityCode);
        BaseResponse<List<StateViewModel>> GetStatesByZoneDistrictName(string zoneDistrictName);
        #endregion BaseResponse Format

        Task<List<StateViewModel>> GetAllStatesAsync();
        Task<List<StateViewModel>> GetStatesByCityAsync(int countryCode);        
        Task<List<StateViewModel>> GetStatesByCityNameAsync(string countryName);
    }
}
