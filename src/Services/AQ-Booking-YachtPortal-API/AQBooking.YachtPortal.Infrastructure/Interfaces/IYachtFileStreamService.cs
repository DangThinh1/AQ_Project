using APIHelpers.Response;
using AQBooking.YachtPortal.Core.Models.YachtFileStreams;
using AQBooking.YachtPortal.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Interfaces
{
    public interface IYachtFileStreamService
    {
        BaseResponse<List<YachtFileStreamViewModel>> GetFileStream(string yachtFId, int categoryFId);
        BaseResponse<string> Encrypt(int yachtFId);
        BaseResponse<PagedList<YachtFileStreamViewModel>> GetFileStreamPaging(YachtFileStreamSearchModel searchModel);
    }
}
