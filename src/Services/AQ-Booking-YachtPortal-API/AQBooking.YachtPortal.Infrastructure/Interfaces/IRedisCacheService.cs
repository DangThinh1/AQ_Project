using APIHelpers.Response;
using AQBooking.YachtPortal.Core.Helpers;
using AQBooking.YachtPortal.Core.Models.RedisCaches;
using AQBooking.YachtPortal.Core.Models.Yachts;
using System.Collections.Generic;

namespace AQBooking.YachtPortal.Infrastructure.Interfaces
{
   public interface IRedisCacheService
    {
        BaseResponse<ResponseModel> SetRedisCacheYacht(RedisCachesYachtModel requestModel);
        BaseResponse<List<YachtPackageServiceModel>> GetRedisCacheYacht(string hashKey, string key, string yachtFId);
        BaseResponse<List<RedisStorage>> GetRedisCacheUser(string key);

        BaseResponse<ResponseModel> RemoveRedisCacheYacht(RedisCachesYachRemoveModel requestModel);
        BaseResponse<ResponseModel> CopyLocalStorageToRedisCache(RedisCachesModel requestModel);
        BaseResponse<string> SetSimpleKeyModel(RedisCachesModel entireData);
        BaseResponse<string> SetSimpleKey(string key, string value);
        BaseResponse<string> GetSimpleKey(string key);
        BaseResponse<string> RemoveSimpleKey(string key);
        }
}
        
    

