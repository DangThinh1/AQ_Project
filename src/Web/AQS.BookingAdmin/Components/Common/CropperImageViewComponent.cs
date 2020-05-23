using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YachtMerchant.Web.Components
{
    public class CropperImageViewComponent: ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int fileId = 0)
        {
            return await Task.Run(() => View("CropperImage", fileId));
        }
    }
}
