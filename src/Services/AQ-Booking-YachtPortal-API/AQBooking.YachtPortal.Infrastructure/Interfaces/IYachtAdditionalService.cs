using APIHelpers.Response;
using AQBooking.YachtPortal.Core.Models.YachtAdditionalServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Interfaces
{
    public interface IYachtAdditionalService
    {
        BaseResponse<List<YachtAdditionalPackageViewModel>> GetYachtAddictionalPackageByYachtId(string yachtFId);
    }
}
