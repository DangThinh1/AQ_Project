using System.Collections.Generic;
using YachtMerchant.Core.Models.YachtTourPreview;

namespace YachtMerchant.Infrastructure.Interfaces
{
    public interface IPreviewYachtTourDetail
    {
        YachtTourDetailModel GetYachtTourDetail(int tourId, int langId);
        List<YachtTourFileStreamModel> GetYachtTourFileStream(int tourId, int fileCategoryFid);
        List<YachtTourAttributeModel> GetYachtTourAttribute(int tourId);
        YachtTourYachtModel GetYachtTourYacht(int yachtId, int langId);
        string GetYachtTourPrice(int tourId, int yachtId, int pax);
    }
}
