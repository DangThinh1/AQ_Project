using APIHelpers.Response;
using AQBooking.Core.Helpers;
using AccommodationMerchant.Core.Models.HotelInformationDetails;
using AccommodationMerchant.Infrastructure.Databases;

namespace AccommodationMerchant.Infrastructure.Services.Interfaces
{
    public interface IHotelInformationDetailService
    {
        //View
        BaseResponse<HotelInformationDetailViewModel> Find(long id);
        BaseResponse<PagedList<HotelInformationDetailViewModel>> Search(HotelInformationDetailSearchModel model);

        //Modify
        BaseResponse<bool> Create(HotelInformationDetailCreateModel model);
        BaseResponse<bool> Update(HotelInformationDetailUpdateModel model);
        BaseResponse<bool> IsActivated(long id, bool value);
        BaseResponse<bool> Delete(long id);
    }
}