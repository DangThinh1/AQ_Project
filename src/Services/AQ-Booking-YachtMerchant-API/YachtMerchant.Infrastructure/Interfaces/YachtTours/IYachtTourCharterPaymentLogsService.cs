using APIHelpers.Response;
using System.Collections.Generic;
using YachtMerchant.Core.Models.YachtTourCharterPaymentLogs;

namespace YachtMerchant.Infrastructure.Interfaces.YachtTours
{
    public interface IYachtTourCharterPaymentLogsService
    {
        /// <summary>
        /// This is method use get all payment log of charter By TourCharterFid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<List<YachtTourCharterPaymentLogsViewModel>> GetYachtTourCharterPaymentLogsByTourCharterId(long id);
    }
}
