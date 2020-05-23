using AQS.BookingMVC.Infrastructure.ConfigModel;
using AQS.BookingMVC.Models.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using static AQS.BookingMVC.Infrastructure.ConfigModel.RedisCacheSrv;

namespace AQS.BookingMVC.Infrastructure.Helpers
{
    public class ApiUrlHelper
    {
        internal static void Init(IConfiguration configuration)
        {
            ApiServer = configuration.GetSection("ApiServer").Get<ApiServer>();
        }

        public static ApiServer ApiServer { get; private set; }
        public static int CurrentServer => ApiServer.Server;
        public static string PermissionKey => ApiServer.PermissionKey ?? string.Empty;
        public static string ProtectedEnviroment => ApiServer.ProtectedEnviroment ?? string.Empty;

        public static YachtPortalApiUrl GetListYachtPortalApi()
        {
            return DependencyInjectionHelper.GetService<IOptions<YachtPortalApiUrl>>().Value;
        }
        public static string CertificateSercurityKey
        {
            get
            {
                var value = ApiServer.CertificateSercurityKey;
                if (string.IsNullOrEmpty(value))
                    throw new Exception("CertificateSercurityKey not provide");
                return value;
            }
        }

        public static string CertificatePFX
        {
            get
            {
                var server = ApiServer.CertificatePFX;
                switch (CurrentServer)
                {
                    case 0:
                        return server.Server_LOCAL;
                    case 1:
                        return server.Server_VN;
                    case 2:
                        return server.Server_BETA;
                    case 3:
                        return server.Server_LIVE;
                    default: throw new Exception("Please check environment variable in appsetting.json");
                }
            }
        }
        public static string AQBaseDomainPortal
        {
            get
            {
                var server = ApiServer.AQBaseDomainPortal;
                switch (CurrentServer)
                {
                    case 0:
                        return server.Server_LOCAL;
                    case 1:
                        return server.Server_VN;
                    case 2:
                        return server.Server_BETA;
                    case 3:
                        return server.Server_LIVE;
                    default: throw new Exception("Please check environment variable in appsetting.json");
                }
            }
        }

      
        public static string SSOPortal
        {
            get
            {
                var server = ApiServer.SSOPortal;
                switch (CurrentServer)
                {
                    case 0:
                        return server.Server_LOCAL;
                    case 1:
                        return server.Server_VN;
                    case 2:
                        return server.Server_BETA;
                    case 3:
                        return server.Server_LIVE;
                    default: throw new Exception("Please check environment variable in appsetting.json");
                }
            }
        }

        public static RedisCache RedisCacheSrv
        {
            get
            {
                var server = ApiServer.RedisCacheSrv;
                switch (CurrentServer)
                {
                    case 0:
                        return server.Server_LOCAL;
                    case 1:
                        return server.Server_VN;
                    case 2:
                        return server.Server_BETA;
                    case 3:
                        return server.Server_LIVE;
                    default: throw new Exception("Please check environment variable in appsetting.json");
                }
            }
        }

        public static string YachtPortal
        {
            get
            {
                var server = ApiServer.YachtPortal;
                switch (CurrentServer)
                {
                    case 0:
                        return server.Server_LOCAL;
                    case 1:
                        return server.Server_VN;
                    case 2:
                        return server.Server_BETA;
                    case 3:
                        return server.Server_LIVE;
                    default: throw new Exception("Please check environment variable in appsetting.json");
                }
            }
        }

        public static string DiningPortal
        {
            get
            {
                var server = ApiServer.DiningPortal;
                switch (CurrentServer)
                {
                    case 0:
                        return server.Server_LOCAL;
                    case 1:
                        return server.Server_VN;
                    case 2:
                        return server.Server_BETA;
                    case 3:
                        return server.Server_LIVE;
                    default: throw new Exception("Please check environment variable in appsetting.json");
                }
            }
        }



        public static string PaymentApi
        {
            get
            {
                var server = ApiServer.PaymentApi;
                switch (CurrentServer)
                {
                    case 0:
                        return server.Server_LOCAL;
                    case 1:
                        return server.Server_VN;
                    case 2:
                        return server.Server_BETA;
                    case 3:
                        return server.Server_LIVE;
                    default: throw new Exception("Please check environment variable in appsetting.json");
                }
            }
        }
        public static string ConfigurationApi
        {
            get
            {
                var server = ApiServer.ConfigurationApi;
                switch (CurrentServer)
                {
                    case 0:
                        return server.Server_LOCAL;
                    case 1:
                        return server.Server_VN;
                    case 2:
                        return server.Server_BETA;
                    case 3:
                        return server.Server_LIVE;
                    default: throw new Exception("Please check environment variable in appsetting.json");
                }
            }
        }

        public static string DiningPortalApi
        {
            get
            {
                var server = ApiServer.DiningPortalApi;
                switch (CurrentServer)
                {
                    case 0:
                        return server.Server_LOCAL;
                    case 1:
                        return server.Server_VN;
                    case 2:
                        return server.Server_BETA;
                    case 3:
                        return server.Server_LIVE;
                    default: throw new Exception("Please check environment variable in appsetting.json");
                }
            }
        }

        public static string YachtPortalApi
        {
            get
            {
                var server = ApiServer.YachtPortalApi;
                switch (CurrentServer)
                {
                    case 0:
                        return server.Server_LOCAL;
                    case 1:
                        return server.Server_VN;
                    case 2:
                        return server.Server_BETA;
                    case 3:
                        return server.Server_LIVE;
                    default: throw new Exception("Please check environment variable in appsetting.json");
                }
            }
        }

        public static string IdentityApiUrl
        {
            get
            {
                var server = ApiServer.IdentityApi;
                switch (CurrentServer)
                {
                    case 0:
                        return server.Server_LOCAL;
                    case 1:
                        return server.Server_VN;
                    case 2:
                        return server.Server_BETA;
                    case 3:
                        return server.Server_LIVE;
                    default: throw new Exception("Please check environment variable in appsetting.json");
                }
            }
        }

        public static string FileStreamApiUrl
        {
            get
            {
                var server = ApiServer.FileStreamApi;
                switch (CurrentServer)
                {
                    case 0:
                        return server.Server_LOCAL;
                    case 1:
                        return server.Server_VN;
                    case 2:
                        return server.Server_BETA;
                    case 3:
                        return server.Server_LIVE;
                    default: throw new Exception("Please check environment variable in appsetting.json");
                }
            }
        }

        public static string AdminApiUrl
        {
            get
            {
                var server = ApiServer.AdminApi;
                switch (CurrentServer)
                {
                    case 0:
                        return server.Server_LOCAL;
                    case 1:
                        return server.Server_VN;
                    case 2:
                        return server.Server_BETA;
                    case 3:
                        return server.Server_LIVE;
                    default: throw new Exception("Please check environment variable in appsetting.json");
                }
            }
        }

        //public static string LoggingApiUrl
        //{
        //    get
        //    {
        //        var server = ApiServer.LoggingApi;
        //        switch (CurrentServer)
        //        {
        //            case 0:
        //                return server.Server_LOCAL;
        //            case 1:
        //                return server.Server_VN;
        //            case 2:
        //                return server.Server_BETA;
        //            case 3:
        //                return server.Server_LIVE;
        //            default: throw new Exception("Please check environment variable in appsetting.json");
        //        }
        //    }
        //}

        public static string SearchModelToUrlParameter(object searchModel)
        {
            try
            {
                if (searchModel == null)
                    return string.Empty;
                var listProperties = searchModel.GetType().GetProperties();
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
                        var list = prop.GetValue(searchModel, null) as IList;
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
                        var value = prop.GetValue(searchModel, null);

                        if (value != null && !value.Equals(DefaultForType(type)))
                        {
                            urlParam += string.Format("{0}={1}&", prop.Name, value);
                        }
                    }
                }

                return urlParam.TrimEnd('&');
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static object DefaultForType(Type targetType)
        {
            return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
        }
    }
}
