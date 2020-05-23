using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using YachtMerchant.Infrastructure.Database.Entities;

namespace YachtMerchant.Infrastructure.Interfaces
{
    public interface ICommonResourcesPortalServices 
    {
        void InitController(ControllerBase controller);

        Task<List<CommonResources>> GetAllResource(int languageID, List<string> type = null);
    }
}
