using APIHelpers.Response;
using System.Collections.Generic;
using System.Threading.Tasks;
using YachtMerchant.Core.Models.YachtTourCharterSchedules;

namespace YachtMerchant.Infrastructure.Interfaces.YachtTours
{
    public interface IYachtTourCharterSchedulesService
    {

        /// <summary>
        /// This is method use get detail infomation schedules By id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<YachtTourCharterSchedulesViewModel> GetYachtTourCharterSchedulesById(long id);


        /// <summary>
        /// This is method use get all schedules set on Chartering By CharteringId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<List<YachtTourCharterSchedulesViewModel>> GetAllScheduleSetOnCharterSchedulesByCharterId(long id);

        /// <summary>
        /// This is method use get all scdedules of Charter (have join with Yacht get infomation of Yacht)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BaseResponse<List<YachtTourCharterSchedulesViewModel>> GetYachtTourCharterSchedulesByCharterId(long id);

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
        /// This is method use create new Processing Fees for each Charter 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResponse<bool>> CreateYachtTourCharterSchedules(YachtTourCharterSchedulesCreateModel model);

        /// <summary>
        /// This is method use  when want edit schedules of Charter and Yacht
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<BaseResponse<bool>> UpdateYachtTourCharterSchedules(YachtTourCharterSchedulesUpdateModel model);


        /// <summary>
        /// This is method use  when want delete schedules had been set of Charter 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<bool>> DeleteYachtTourCharterSchedules(long id);

        

    }
}
