using System;
using System.Linq;
using APIHelpers.Response;
using YachtMerchant.Core.Models;
using Microsoft.EntityFrameworkCore;
using YachtMerchant.Infrastructure.Helpers;
using YachtMerchant.Infrastructure.Database;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Infrastructure.Services
{
    public class CheckHealthService : ICheckHealthService
    {
        private readonly YachtOperatorDbContext _db;
        public CheckHealthService(YachtOperatorDbContext db)
        {
            _db = db;
        }
        
        public BaseResponse<bool> IsGoodHealth()
        {
            try
            {
                return ServerInfo().IsSuccessStatusCode ? BaseResponse<bool>.Success(true) : BaseResponse<bool>.BadRequest();
            }
            catch
            {
                return BaseResponse<bool>.InternalServerError();
            }
        }

        public BaseResponse<CheckHealthModel> ServerInfo()
        {
            var model = new CheckHealthModel();
            try
            {
                //Info
                model.ServerName = ApiUrlHelper.ServerName;
                model.Enviroment = ApiUrlHelper.Server;
                model.IdentityApi = ApiUrlHelper.IdentityApiUrl;
                model.FileStreamApi = ApiUrlHelper.FileStreamApiUrl;
                model.AdminApi = ApiUrlHelper.AdminApiUrl;
                model.ConfigurationApi = ApiUrlHelper.ConfigurationApi;
                model.ConnectionString = GetConnectionStringPublicInfo(ApiUrlHelper.ConnectionString);

                //Test Connection
                var testConnect_Database = TestConnectDatabase();
                model.TestConnect_Database = testConnect_Database ? "Connect database successful" : "Connect database failed";
                if (testConnect_Database)
                {
                    model.IsGoodHealth = true;
                    return BaseResponse<CheckHealthModel>.Success(model);
                }
                    
                return BaseResponse<CheckHealthModel>.BadRequest(model);
            }
            catch (Exception ex)
            {
                return BaseResponse<CheckHealthModel>.InternalServerError(model);
            }
        }
        private string GetConnectionStringPublicInfo(string connectionString)
        {
            var connectionStringArr = connectionString.Split(";");
            var result = $"{connectionStringArr[0]};{connectionStringArr[1]}";
            return result;
        }

        private bool TestConnectDatabase()
        {
            try
            {
                var test = _db.YachtTourAttributes.AsNoTracking().FirstOrDefault();
                return test != null ? true : false;
            }
            catch
            {
                return false;
            }
        }
    }
}