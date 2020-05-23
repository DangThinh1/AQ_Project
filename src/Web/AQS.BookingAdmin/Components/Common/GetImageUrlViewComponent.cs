using AQBooking.Admin.Core.Enums;
using AQS.BookingAdmin.Services.Interfaces.Common;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AQS.BookingAdmin.Components
{
    public class GetImageUrlViewComponent : ViewComponent
    {
        #region Fields
        private readonly IFileStreamService _fileStreamService;
        #endregion

        #region Ctor
        public GetImageUrlViewComponent(IFileStreamService fileStreamService)
        {
            _fileStreamService = fileStreamService;
        }
        #endregion

        #region Methods
        public async Task<IViewComponentResult> InvokeAsync(int fileId, ThumbRatioEnum ratio = ThumbRatioEnum.full)
        {
            var result = await _fileStreamService.GetFileById(fileId, ratio) ?? string.Empty;
            return Content(result);
        }
        #endregion
    }
}
