using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIHelpers;
using AQBooking.FileStreamWeb.AppConfig;
using AQBooking.FileStreamWeb.Helpers;
using Microsoft.Extensions.Options;

namespace AQBooking.FileStreamWeb.Services
{
    public class ServiceBase
    {
        #region Fields
        protected readonly APIExcute _aPIExcute;
        protected string _basicToken;
        protected string _baseFileStreamUrl;
        #endregion

        #region Ctor
        public ServiceBase()
        {
            _aPIExcute = new APIExcute(AuthenticationType.Bearer);
            _baseFileStreamUrl = ApiUrlHelper.FileStreamApiUrl;
            _basicToken = _aPIExcute.GetBasicAuthToken("AESAPI", "Sysadmin@2019$$");
        }
        #endregion

        #region Methods

        #endregion
    }
}
