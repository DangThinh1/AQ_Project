using APIHelpers.Response;
using AQBooking.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using YachtMerchant.Core.Models.YachtCharteringSchedules;

namespace YachtMerchant.Infrastructure.Interfaces
{
    public interface IYachtCharteringSchedulesService
    {

        /// <summary>
        /// This is method use get detail infomation schedules By id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<YachtCharteringSchedulesViewModel> GetYachtCharteringSchedulesById(long id);


        /// <summary>
        /// This is method use get all schedules set on Chartering By CharteringId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<List<YachtCharteringSchedulesViewModel>> GetAllScheduleSetOnCharteringSchedulesByCharteringId(long id);

        /// <summary>
        /// This is method use get all scdedules of charteringId (have join with Yacht get infomation of Yacht)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<List<YachtCharteringSchedulesViewModel>> GetYachtCharteringSchedulesByCharteringId(long id);

        /// <summary>
        /// Check exist user set in schedules for reservation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse<bool> CheckExistUserSetInSchedules(CheckDuplicateUserSchedulesModel model);

        /// <summary>
        /// Check exist role set in schedules for reservation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse<bool> CheckExistRoleSetInSchedules(CheckDuplicateRoleSchedulesModel model);


        /// <summary>
        /// Check exist schedule set user & role for reservation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        BaseResponse<bool> CheckExistUserRoleInSchedules(CheckDuplicateSchedulesModel model);

        /// <summary>
        /// This is method use create new Processing Fees for each Yacht Chartering 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResponse<bool>> CreateYachtCharteringSchedules(YachtCharteringSchedulesCreateModel model);

        /// <summary>
        /// This is method use  when want edit schedules of Chartering and Yacht
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResponse<bool>> UpdateYachtCharteringSchedules(YachtCharteringSchedulesUpdateModel model);


        /// <summary>
        /// This is method use  when want delete schedules had been set of Chartering 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<bool>> DeleteYachtCharteringSchedules(long id);

        

    }
}
