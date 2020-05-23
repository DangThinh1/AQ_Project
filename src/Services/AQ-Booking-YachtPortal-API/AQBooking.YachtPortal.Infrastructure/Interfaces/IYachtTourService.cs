using AQBooking.YachtPortal.Core.Models.YachtTourCharter;
using AQBooking.YachtPortal.Core.Models.YachtTours;
using AQBooking.YachtPortal.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Interfaces
{
    public interface IYachtTourService
    {
        IPagedList<YachtTourViewModel> SearchYachtTour(YachtTourSearchModel searchModel);
        YachtTourDetailModel GetYachtTourDetail(int tourId, int langId);
        List<YachtTourFileStreamModel> GetYachtTourFileStream(int tourId, int fileCategoryFid);
        List<YachtTourAttributeModel> GetYachtTourAttribute(int tourId);
        YachtTourYachtModel GetYachtTourYacht(int yachtId, int langId);
        YachtTourPriceResultModel GetYachtTourPrice(int tourId, int yachtId, int pax);
        YachtTourCharterViewModel GetYachtTourCharterByUniqueId(string uniqueId);
        YachtTourCharterResultModel CreateYachtTourCharter(YachtTourCharterCreateModel model);
        bool UpdateYachtTourCharter(YachtTourCharterUpdateModel model);
    }    
}
