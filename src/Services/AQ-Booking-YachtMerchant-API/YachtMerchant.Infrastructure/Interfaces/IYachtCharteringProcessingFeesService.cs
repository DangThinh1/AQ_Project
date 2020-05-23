using APIHelpers.Response;
using AQBooking.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YachtMerchant.Core.Models.YachtCharteringProcessingFees;

namespace YachtMerchant.Infrastructure.Interfaces
{
    public interface IYachtCharteringProcessingFeesService
    {
        /// <summary>
        /// This is method use create new Processing Fees for each Yacht Reservation 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResponse<bool>> CreateYachtCharteringProcessingFees(YachtCharteringProcessingFeesCreateModel model);

        /// <summary>
        /// This is method create YachtCharteringProcessingFees and also Update change status Reservation in YachtCharterings has control by transactions
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResponse<bool>> CreateYachtCharteringProcessingFeesAndChangeStatusReservationTransaction(YachtCharteringProcessingFeeWithChangeStatusReservationCreateModel model);

        /// <summary>
        /// This is method use  when want edit infomation Processing Fee
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResponse<bool>> UpdateYachtCharteringProcessingFees(YachtCharteringProcessingFeesUpdateModel model);

        
        /// <summary>
        /// This is method use get infomation of ProcessingFee By CharteringId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<YachtCharteringProcessingFeesViewModel> GetYachtCharteringProcessingFeesByCharteringId(long id);

    }
}
