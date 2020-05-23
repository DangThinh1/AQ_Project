using APIHelpers.Response;
using AQBooking.Admin.Core.Models.AuthModel;
using AQS.BookingAdmin.Infrastructure.ConfigModel;
using AQS.BookingAdmin.Infrastructure.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AQS.BookingAdmin.Infrastructure.Extensions
{

  public static class WebExtensions
  {
    static ApiServer _apiServer;
    private static ApiServer ApiServer
    {
      get => _apiServer ?? (_apiServer = DependencyInjectionHelper.GetService<IOptions<ApiServer>>().Value);
    }
    /// <summary>
    /// Check and get data from BaseResponse<T>
    /// </summary>
    /// <typeparam name="T">type of Object data</typeparam>
    /// <param name="baseResponse">Base response model return from api</param>
    /// <returns>Object Data</returns>
    public static T GetDataResponse<T>(this BaseResponse<T> baseResponse) where T : class, new()
    {
      if (baseResponse == null || !baseResponse.IsSuccessStatusCode || baseResponse.ResponseData == null)
        return new T();
      return baseResponse.ResponseData;
    }

    public static string GetCurrentServer(this Server server, int? serverId = null)
    {
      serverId = serverId ?? ApiServer.Server;
      switch (serverId)
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
    public static string FullName(this UserInfoModel user)
    {
      return $"{user.FirstName} {user.LastName}";
    }

  }
}
