using System;
using APIHelpers.Response;
using APIHelpers.ServerHost;
using AQConfigurations.Core.Enums;
using AQConfigurations.Core.Services.Interfaces;

namespace AQConfigurations.Core.Services.Implements
{
    public class FileStreamRequestService : ConfigurationsRequestServiceBase, IFileStreamRequestService
    {
        private const string GET_IMAGE_URL_BY_ID_RATIO = "api/File/{0}/{1}";

        private readonly string _fileStreamHost;
        public FileStreamRequestService(string user = null, string pasword = null) : base()
        {
            _fileStreamHost= ServerHostHelper.GetServerHostByName("FileStreamApi");
            UseBasicAuthorization(user ?? "AESAPI", pasword ?? "Sysadmin@2019$$");
        }
        public BaseResponse<string> GetImageUrlByIdAndRatio(int id, int ratio, string actionUrl = "")
        {
            try
            {
                if (!Enum.IsDefined(typeof(ThumbRatioEnum), ratio))
                    return BaseResponse<string>.BadRequest("Please check the ratio value");
                var thumbRatioEnum = (ThumbRatioEnum)ratio;
                return GetImageUrlByIdAndRatio(id, thumbRatioEnum, actionUrl);
            }
            catch (Exception ex)
            {
                return BaseResponse<string>.InternalServerError(ex: ex);
            }
        }
        public BaseResponse<string> GetImageUrlByIdAndRatio(int id, ThumbRatioEnum ratio, string actionUrl = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(actionUrl))
                    actionUrl = string.Format(GET_IMAGE_URL_BY_ID_RATIO, id, (int)ratio);
                string url = _fileStreamHost + actionUrl;
                var response = Get<string>(url);
                return response;
            }
            catch(Exception ex)
            {
                return BaseResponse<string>.InternalServerError(ex: ex);
            }
        }
    }
}