using AccommodationMerchant.Core.Models.HotelInformations;
using APIHelpers.Response;
using AQBooking.Core.Helpers;
using System.Collections.Generic;

namespace AccommodationMerchant.Infrastructure.Services.Interfaces
{
    public interface IHotelInformationService
    {
        BaseResponse<HotelInformationDetailViewModel> Find(int id);
        BaseResponse<PagedList<HotelInformationViewModel>> Search(HotelInformationSearchModel model);

        BaseResponse<bool> Create(HotelInformationCreateModel model);
        BaseResponse<bool> Update(HotelInformationUpdateModel model);
        BaseResponse<bool> IsActivated(int id, bool value);
        BaseResponse<bool> Delete(int id);
        BaseResponse<List<HotelInformationSupportModel>> GetInfoDetailSupportedByInfoId(int infoId);
    }
}
