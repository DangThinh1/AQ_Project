using APIHelpers.Response;
using System;
using System.Collections.Generic;
using System.Text;
using YachtMerchant.Core.Models.YachtCharteringPaymentLogs;

namespace YachtMerchant.Infrastructure.Interfaces
{
    public interface IYachtCharteringPaymentLogsService
    {
        /// <summary>
        /// This is method use get all payment log of Chartering By CharteringId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<List<YachtCharteringPaymentLogsViewModel>> GetYachtCharteringPaymentLogsByCharteringId(long id);
    }
}
