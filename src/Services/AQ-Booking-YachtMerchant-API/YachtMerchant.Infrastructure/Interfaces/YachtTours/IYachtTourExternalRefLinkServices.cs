using APIHelpers.Response;
using AQBooking.Core.Helpers;
using System.Collections.Generic;
using YachtMerchant.Core.Models.YachtExternalRefLinks;
using YachtMerchant.Core.Models.YachtTourExternalRefLinks;

namespace YachtMerchant.Infrastructure.Interfaces.YachtTours
{
    public interface IYachtTourExternalRefLinkServices
    {
        BaseResponse<bool> Create(YachtTourExternalRefLinkModel model);
        BaseResponse<bool> Update(YachtTourExternalRefLinkModel model);
        BaseResponse<bool> Delete(long id);
        BaseResponse<YachtTourExternalRefLinkModel> FindById(long id);
        BaseResponse<List<YachtTourExternalRefLinkModel>> GetAll();
        BaseResponse<PagedList<YachtTourExternalRefLinkModel>> GetByYachtTourId(int id, YachtTourExternalRefLinkSearchModel searchModel);
    }
}
