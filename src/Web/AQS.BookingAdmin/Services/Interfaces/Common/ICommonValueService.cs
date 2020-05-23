using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.Admin.Core.Models.CommonValue;
using AQS.BookingAdmin.Models.Paging;

namespace AQS.BookingAdmin.Services.Interfaces.Common
{
    public interface ICommonValueService
    {
        /// <summary>
        /// Get common value
        /// </summary>
        /// <param name="Id">Identity</param>
        /// <returns>Commonvalue</returns>
        Task<CommonValueViewModel> GetById(int Id);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<List<CommonValueViewModel>> GetAllCommonValues();

        /// <summary>
        ///
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        Task<List<CommonValueViewModel>> GetCommonValuesByGroupName(string groupName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<PagedListClientModel<CommonValueViewModel>> GetAllCommonValuesPaging(CommonValueSearchModel model);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetValueGroupStr();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<object> CreateNewCommonValues(CommonValueCreateModel model);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
        Task<object> UpdateCommonValues(CommonValueUpdateModel model);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<object> DeleteCommonValues(int Id);
       
       
        

    }
}
