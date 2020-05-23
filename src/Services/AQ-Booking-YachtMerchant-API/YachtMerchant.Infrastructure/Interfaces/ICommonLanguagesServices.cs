using YachtMerchant.Core.Models.CommonLanguagues;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YachtMerchant.Infrastructure.Interfaces
{
    public interface ICommonLanguagesServices
    {
        void InitController(ControllerBase controller);
        string GetLanguageCommonValue(int languageId);
        Task<List<CommonLanguagesViewModel>> GetAllAsync();
    }
}
