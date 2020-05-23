using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AQS.BookingAdmin.Services.Interfaces.Common
{
    public interface ISelectListService
    {
        List<SelectListItem> GetLstByParentId();

        List<SelectListItem> PreparingLanguageList();

        List<SelectListItem> PreparingCategoryList();

        Task<List<SelectListItem>> GetUserSelectList(List<int> listRoleId=null);
    }
}
