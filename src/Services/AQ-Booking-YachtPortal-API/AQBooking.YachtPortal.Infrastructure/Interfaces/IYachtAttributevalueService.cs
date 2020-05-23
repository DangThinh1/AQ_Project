using APIHelpers.Response;
using AQBooking.YachtPortal.Core.Models.YachtAttributeValues;
using System;
using System.Collections.Generic;
using System.Text;

namespace AQBooking.YachtPortal.Infrastructure.Interfaces
{
    public interface IYachtAttributevalueService
    {
        BaseResponse<List<YachtAttributeValueViewModel>> GetAttributesCharterPrivate(string yachtFId, int categoryFId, bool isInclude, List<string> attributeName);
        BaseResponse<YachtAttributeValueCharterPrivateGeneralViewModel> GetAttributesCharterPrivateGeneral(string yachtFId, int categoryFId, bool isInclude, List<string> attributeName);
        BaseResponse<YachtAttributeValueCharterPrivateGeneralViewModel> GetAttributesCharterPrivateGeneral2(string yachtFId, int categoryFId, bool isInclude, List<string> attributeName);
    }
}
