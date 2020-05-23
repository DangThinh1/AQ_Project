using APIHelpers;
using AQS.BookingAdmin.Infrastructure.ConfigModel;
using AQS.BookingAdmin.Infrastructure.Constants;
using AQS.BookingAdmin.Infrastructure.Extensions;
using AQS.BookingAdmin.Infrastructure.Helpers;
using AQS.BookingAdmin.Interfaces.User;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AQS.BookingAdmin.Services.Implements
{
    public abstract class BaseService
    {
        #region Field
        protected APIExcute _aPIExcute;
        protected readonly ApiServer _apiServer;
        protected readonly string _basicAPIToken;
        protected readonly string _baseAdminAPIUrl;
        protected readonly AdminPortalApiUrl _adminPortalApiUrl;
        protected readonly string _baseConfigurationAPIUrl;
        private IWorkContext _workcontext = null;
        protected IWorkContext WorkContext { get => _workcontext ?? (_workcontext = DependencyInjectionHelper.GetService<IWorkContext>()); }
        protected string Token => WorkContext.UserToken;

        #endregion

        #region Ctor
        public BaseService()
        {
            _aPIExcute = new APIExcute(AuthenticationType.Bearer);
            _basicAPIToken = _aPIExcute.GetBasicAuthToken(BasicAccountConstant.UserName, BasicAccountConstant.Password);
            _apiServer = DependencyInjectionHelper.GetService<IOptions<ApiServer>>().Value;
            _adminPortalApiUrl = DependencyInjectionHelper.GetService<IOptions<AdminPortalApiUrl>>().Value;
            _baseAdminAPIUrl = _apiServer.AQAdminApi.GetCurrentServer();
            _baseConfigurationAPIUrl = _apiServer.AQConfigurationApi.GetCurrentServer();

        }
        #endregion

        #region Helper Methods

        protected string ConvertToUrlParameter(object param, string paramName = "")
        {
            try
            {
                if (param == null)
                    return string.Empty;
                string urlParameter = string.Empty;

                var paramType = param.GetType();
                var typeInt = typeof(List<int>);
                var typeLong = typeof(List<long>);
                var typeLDouble = typeof(List<double>);
                var typeDecimal = typeof(List<decimal>);
                var typeString = typeof(List<string>);

                //if input is a list of value type
                if (paramType.Equals(typeInt) || paramType.Equals(typeLong) || paramType.Equals(typeLong) || paramType.Equals(typeLDouble) || paramType.Equals(typeDecimal) || paramType.Equals(typeString))
                {
                    urlParameter = ListValueToParam(param, paramName);
                }
                else
                {

                    urlParameter = ModelToParam(param);
                }

                return urlParameter;
            }
            catch
            {
                return string.Empty;
            }
        }

        private string ListValueToParam(object listValue, string paramName = "")
        {
            try
            {
                var list = listValue as IList;
                string urlParam = string.Empty;
                if (!urlParam.Contains("?"))
                    urlParam += "?";
                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        urlParam += string.Format("{0}={1}&", paramName, item);
                    }
                }

                return urlParam.TrimEnd('&');
            }
            catch
            {

                return string.Empty;
            }
        }

        private string ModelToParam(object model)
        {
            var listProperties = model.GetType().GetProperties();
            string urlParam = string.Empty;
            foreach (var prop in listProperties)
            {
                var type = prop.PropertyType;
                var typeInt = typeof(List<int>);
                var typeLong = typeof(List<long>);
                var typeLDouble = typeof(List<double>);
                var typeDecimal = typeof(List<decimal>);
                var typeString = typeof(List<string>);

                if (type.Equals(typeInt) || type.Equals(typeLong) || type.Equals(typeLong) || type.Equals(typeLDouble) || type.Equals(typeDecimal) || type.Equals(typeString))
                {
                    if (!urlParam.Contains("?"))
                        urlParam += "?";
                    var list = prop.GetValue(model, null) as IList;
                    if (list != null && list.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            urlParam += string.Format("{0}={1}&", prop.Name, item);
                        }
                    }
                }
                else
                {
                    if (!urlParam.Contains("?"))
                        urlParam += "?";
                    var value = prop.GetValue(model, null);

                    if (value != null && !value.Equals(DefaultForType(type)))
                    {
                        urlParam += string.Format("{0}={1}&", prop.Name, value);
                    }
                }
            }
            return urlParam.TrimEnd('&');
        }

        private object DefaultForType(Type targetType)
        {
            return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
        }


        #endregion
    }
}
