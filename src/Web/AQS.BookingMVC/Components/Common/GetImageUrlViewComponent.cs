using AQBooking.YachtPortal.Core.Enum;
using AQS.BookingMVC.Services.Implements.Common;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Components.Common
{
    public class GetImageUrlViewComponent : ViewComponent
    {
        #region Fields
        private readonly IFileStreamService _fileStreamService;
        #endregion

        #region Contructor
        public GetImageUrlViewComponent(IFileStreamService fileStreamService)
        {
            _fileStreamService = fileStreamService;
        }
        #endregion

        #region Methods
        public async Task<IViewComponentResult> InvokeAsync(int fileId = 0, ThumbRatioEnum ratio = ThumbRatioEnum.full)
        {
            var result = await _fileStreamService.GetFileById(fileId, ratio);
            return Content(result);
        }
        #endregion
    }
}
