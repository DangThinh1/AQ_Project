using APIHelpers.Response;
using AQBooking.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YachtMerchant.Core.Models.YachtNonBusinessDay;

namespace YachtMerchant.Infrastructure.Interfaces
{
    public interface IYachtNonBusinessDaysService
    {
        Task<BaseResponse<bool>> CreateAsync(YachtNonBusinessDayCreateModel model);
        BaseResponse<List<YachtNonBusinessDayViewModel>> GetNonBusinessDaysByYachtID(int yachtId);
        Task<BaseResponse<bool>> CreateRangeAsync(YachtNonBusinessDayCreateRangeModel createModel);
        Task<BaseResponse<bool>> DeleteNonBusinessDayByIdAsync(int id);
        BaseResponse<bool> Update(YachtNonBusinessDayUpdateModel model);
        BaseResponse<YachtNonBusinessDayViewModel> GetYactNonBusinessDayById(int id);
    }
}
