using APIHelpers.Response;
using System.Collections.Generic;
using AQConfigurations.Core.Models.Cities;

namespace AQConfigurations.Core.Services.Interfaces
{
    public interface IZoneDistrictRequestService
    {
        BaseResponse<List<StateViewModel>> GetAllStates(string actionUrl = "");
        BaseResponse<List<StateViewModel>> GetStatesByCityCode(int cityCode, string actionUrl = "");
        BaseResponse<List<StateViewModel>> GetStatesByZoneDistrictName(string zoneDistrictName, string actionUrl = "");
    }
}
