using System.Threading.Tasks;
using AQBooking.YachtPortal.Core.Enum;

namespace AQS.BookingMVC.Services.Implements.Common
{
    public interface IFileStreamService
    {
        ///// <summary>
        ///// Inject file id and get url of image
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //Task<string> GetFileById(int id);

        /// <summary>
        /// Inject file id and list url by ratio
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ratio"></param>
        /// <returns></returns>
        Task<string> GetFileById(int id, ThumbRatioEnum ratio);

        /// <summary>
        /// Get File By Id And Type
        /// </summary>
        /// <param name="id">File id</param>
        /// <param name="typeId">File type id</param>
        /// <returns>File url</returns>
        Task<string> GetFileByIdAndType(int id, int typeId);
    }
}
