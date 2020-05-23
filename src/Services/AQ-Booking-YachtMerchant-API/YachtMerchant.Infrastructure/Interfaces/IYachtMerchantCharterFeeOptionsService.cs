using APIHelpers.Response;
using System;
using System.Collections.Generic;
using System.Text;
using YachtMerchant.Core.Models.YachtMerchantCharterFeeOptions;

namespace YachtMerchant.Infrastructure.Interfaces
{
    public interface IYachtMerchantCharterFeeOptionsService
    {
        /// <summary>
        /// This is method use get all infomation charter fee option of Merchant By MerchantId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<List<YachtMerchantCharterFeeOptionsViewModel>> GetAllCharterFeeOptionOfMerchantByMerchantId(int merchantId);
    }
}
