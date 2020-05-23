using AQBooking.Admin.Core.Models.EVisaMerchantAccount;
using AQBooking.Admin.Core.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.Admin.Infrastructure.Interfaces
{
    public interface IEVisaMerchantAccService
    {   
        IPagedList<EVisaMerchantAccViewModel> SearchEVisaMerchantAcc(EVisaMerchantAccSearchModel model);
        EVisaMerchantAccViewModel GetEvisaMerchantAccById(int id);
        bool CreateEvisaMerchantAcc(EVisaMerchantAccCreateUpdateModel model);
        bool UpdateEvisaMerchantAcc(EVisaMerchantAccCreateUpdateModel model);
        bool DeleteEvisaMerchantAcc(int id);
    }
}
