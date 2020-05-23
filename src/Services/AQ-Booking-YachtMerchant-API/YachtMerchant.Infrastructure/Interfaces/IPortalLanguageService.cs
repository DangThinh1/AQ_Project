using YachtMerchant.Core.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace YachtMerchant.Infrastructure.Interfaces
{
    public interface IPortalLanguageService
    {
        void InitController(ControllerBase controller);
        List<DTODropdownItem> GetAllLanguageSupportByPortalId(string PortalId);
    }
}
