using APIHelpers.Response;
using System.Threading.Tasks;
using YachtMerchant.Core.Models.YachtTourCharterProcessingFees;

namespace YachtMerchant.Infrastructure.Interfaces.YachtTours
{
    public interface IYachtTourCharterProcessingFeesService
    {
        /// <summary>
        /// This is method use create new Processing Fees for each reservation 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResponse<bool>> CreateYachtTourCharterProcessingFees(YachtTourCharterProcessingFeesCreateModel model);

        /// <summary>
        /// This is method create YachtTourCharterProcessingFees and also update change status reservation in YachtTourCharter has control by transactions
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResponse<bool>> CreateYachtTourCharterProcessingFeesAndChangeStatusReservationTransaction(YachtTourCharterProcessingFeeWithChangeStatusReservationCreateModel model);

        /// <summary>
        /// This is method use  when want edit infomation Processing Fee
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResponse<bool>> UpdateYachtTourCharterProcessingFees(YachtTourCharterProcessingFeesUpdateModel model);

        
        /// <summary>
        /// This is method use get infomation of ProcessingFee By YachtTourCharterFid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<YachtTourCharterProcessingFeesViewModel> GetYachtTourCharterProcessingFeesByCharterId(long id);

    }
}
