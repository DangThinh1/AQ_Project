using System;
using System.Collections.Generic;
using System.Linq;
using AQBooking.YachtPortal.Infrastructure.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using AQBooking.YachtPortal.Core.Models.RedisCaches;
using Newtonsoft.Json;
using APIHelpers.Response;
using AQBooking.YachtPortal.Core.Models.Yachts;
using AQBooking.YachtPortal.Core.Helpers;

namespace AQBooking.YachtPortal.Infrastructure.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDistributedCache _distributedCache;
        public RedisCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        
        public BaseResponse<ResponseModel> CopyLocalStorageToRedisCache(RedisCachesModel requestModel)
        {
            ResponseModel resultModel = new ResponseModel();
            resultModel.Id = "";
            resultModel.CodeError = "";

            try
            {
                var value = _distributedCache.GetString(requestModel.Key);
                if (requestModel != null)
                {
                    if (value != null)// Haskey Have been existing Value
                    {
                        var lstRedisStorage = JsonConvert.DeserializeObject<List<RedisStorage>>(value);
                        //requestModel.HashKey ==> Dining or Yacht or other...
                        var RedisStorageModel = lstRedisStorage.FirstOrDefault(k => k.Domain == requestModel.HashKey);
                        #region DOMAIN
                        if (RedisStorageModel != null)// DOMAIN IS  EXIST
                        { 
                           var result = RedisStorageModel.PackageStorage;                         

                            if (requestModel.CartStorage != null)
                            {
                                if (result.Count > 0)//if RedisCache have some Yacht Infomation
                                {
                                    foreach (YachtPackageServiceModel cartItem in requestModel.CartStorage as List<YachtPackageServiceModel>)
                                    {
                                        //get Redis Cache Yacht infomation by Id
                                        var yachtStoraged = result.FirstOrDefault(x => x.YachtId.Trim() == cartItem.YachtId.Trim());

                                        if (yachtStoraged != null)// Yacht was existing
                                        {
                                            yachtStoraged.CheckIn = cartItem.CheckIn;
                                            yachtStoraged.CheckOut = cartItem.CheckOut;
                                            yachtStoraged.Passenger = cartItem.Passenger;
                                            yachtStoraged.ProductPackage = cartItem.ProductPackage;
                                            //    if (yachtStoraged.ProductPackage != null && cartItem.ProductPackage != null)//checking package
                                            //    {
                                            //        if (yachtStoraged.ProductPackage.Count > 0 )//Some Existing package in Yacht
                                            //        {
                                            //            foreach (MerchantProductInventoriesModel packageItem in cartItem.ProductPackage)
                                            //            {
                                            //                var packageStorage = yachtStoraged.ProductPackage.FirstOrDefault(x => x.productInventoryFId.Trim() == packageItem.productInventoryFId.Trim());
                                            //                #region if Package of Yacht have be Existing , just Sum the quatility Field and else add entire 
                                            //                if (packageStorage != null)//Update Package in Existing Yacht
                                            //                {
                                            //                    int newQuality = packageStorage.quantity + packageItem.quantity;
                                            //                    packageStorage.quantity = newQuality;
                                            //                }
                                            //                else //add new Pakage in Yacht
                                            //                {
                                            //                    yachtStoraged.ProductPackage.Add(packageItem);
                                            //                }
                                            //                #endregion
                                            //            }
                                            //        }                                                
                                            //    }
                                            //    else// change Existing Package was Storaged in Redist of Yacht through new package was NULL
                                            //    {
                                            //        yachtStoraged.ProductPackage = cartItem.ProductPackage;
                                       // }
                                        }
                                        else //yacht is unvaliable in list
                                        {
                                            result.Add(cartItem);
                                        }
                                    }
                                }
                                else
                                {
                                    result.AddRange(requestModel.CartStorage);
                                }
                                string strSave = JsonConvert.SerializeObject(lstRedisStorage);
                                _distributedCache.SetString(requestModel.Key, strSave);
                            }

                        }
                        else // DOMAIN IS NOT EXIST
                        {
                            if (requestModel.CartStorage != null)
                            {                               
                                List<RedisStorage> saveCartStorage = new List<RedisStorage>()
                                {
                                    new RedisStorage()
                                    {
                                        Domain=requestModel.HashKey,
                                        PackageStorage=requestModel.CartStorage
                                    }
                                };

                                string strSave = JsonConvert.SerializeObject(saveCartStorage);
                                _distributedCache.SetString(requestModel.Key, strSave);
                            }
                        }
                        #endregion  
                    }
                    else
                    {
                        if (requestModel.CartStorage != null)
                        {
                            List<RedisStorage> saveCartStorage = new List<RedisStorage>()
                                {
                                    new RedisStorage()
                                    {
                                        Domain=requestModel.HashKey,
                                        PackageStorage=requestModel.CartStorage
                                    }
                                };

                            string strSave = JsonConvert.SerializeObject(saveCartStorage);
                            _distributedCache.SetString(requestModel.Key, strSave);
                        }
                    }
                }

                return BaseResponse<ResponseModel>.Success(resultModel);
            }
            catch (Exception ex)
            {
                return BaseResponse<ResponseModel>.InternalServerError(resultModel, message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<ResponseModel> SetRedisCacheYacht(RedisCachesYachtModel requestModel)
        {
            ResponseModel resultModel = new ResponseModel();
            try
            {
                var value = _distributedCache.GetString(requestModel.Key);

                if (value != null)
                {
                    var lstRedisStorage = JsonConvert.DeserializeObject<List<RedisStorage>>(value);

                    //requestModel.HashKey ==> Dining or Yacht or other...
                    var RedisStorageModel = lstRedisStorage.FirstOrDefault(k => k.Domain == requestModel.HashKey);
                    #region DOMAIN
                    if (RedisStorageModel != null)// DOMAIN IS  EXIST
                    {
                        var result = RedisStorageModel.PackageStorage;

                        //get Yacht infomation by Id
                        var yachtStoraged = result.FirstOrDefault(k => k.YachtId == requestModel.YachtId);

                        if (yachtStoraged != null)
                        {
                            if (yachtStoraged.ProductPackage.Count > 0)//Some Existing package in Yacht
                            {
                                var packageStorage = yachtStoraged.ProductPackage.FirstOrDefault(x => x.productInventoryFId.Trim() == requestModel.CartStorage.ProductPackage.productInventoryFId.Trim());
                                if (packageStorage != null)//Update Package in Existing Yacht
                                {
                                    int newQuality = packageStorage.quantity + requestModel.CartStorage.ProductPackage.quantity;
                                    packageStorage.quantity = newQuality;
                                }
                                else //add new Pakage in Yacht
                                {
                                    yachtStoraged.ProductPackage.Add(requestModel.CartStorage.ProductPackage);
                                }
                            }
                            else // no Existing Package in Yacht
                            {
                                yachtStoraged.ProductPackage.Add(requestModel.CartStorage.ProductPackage);
                            }
                        }
                        else //yacht is unvaliable in list
                        {
                            YachtPackageServiceModel newCartStorage = new YachtPackageServiceModel()
                            {
                                YachtId = requestModel.YachtId,
                                ProductPackage = new List<MerchantProductInventoriesModel>()
                                {
                                    new MerchantProductInventoriesModel()
                                    {
                                        categroryFId =requestModel.CartStorage.ProductPackage.categroryFId,
                                        productInventoryFId=requestModel.CartStorage.ProductPackage.productInventoryFId,
                                        productName=requestModel.CartStorage.ProductPackage.productName,
                                        quantity= requestModel.CartStorage.ProductPackage.quantity
                                    }
                                }
                            };
                            result.Add(newCartStorage);
                        }
                    }
                    else// DOMAIN IS NOT EXIST
                    {
                        YachtPackageServiceModel newCartStorage = new YachtPackageServiceModel()
                        {
                            YachtId = requestModel.YachtId,
                            ProductPackage = new List<MerchantProductInventoriesModel>()
                                {
                                    new MerchantProductInventoriesModel()
                                    {
                                        categroryFId =requestModel.CartStorage.ProductPackage.categroryFId,
                                        productInventoryFId=requestModel.CartStorage.ProductPackage.productInventoryFId,
                                        productName=requestModel.CartStorage.ProductPackage.productName,
                                        quantity= requestModel.CartStorage.ProductPackage.quantity
                                    }
                                }
                        };

                        RedisStorage newDomainCartStorage = new RedisStorage();

                        newDomainCartStorage.Domain = requestModel.HashKey;
                        newDomainCartStorage.PackageStorage.Add(newCartStorage);
                        lstRedisStorage.Add(newDomainCartStorage);
                    }

                    string strSave = JsonConvert.SerializeObject(lstRedisStorage);
                    _distributedCache.SetString(requestModel.Key, strSave);
                    #endregion
                }
                else
                {
                    List<YachtPackageServiceModel> newCartStorage = new List<YachtPackageServiceModel>()
                    {
                        new YachtPackageServiceModel()
                        {
                            YachtId = requestModel.YachtId,
                            ProductPackage = new List<MerchantProductInventoriesModel>() {
                                    new MerchantProductInventoriesModel(){
                                        categroryFId =requestModel.CartStorage.ProductPackage.categroryFId,
                                        productInventoryFId=requestModel.CartStorage.ProductPackage.productInventoryFId,
                                        productName=requestModel.CartStorage.ProductPackage.productName,
                                        quantity= requestModel.CartStorage.ProductPackage.quantity
                                    }
                                }
                        }
                    };
                    List<RedisStorage> saveCartStorage = new List<RedisStorage>()
                    {
                        new RedisStorage()
                        {
                            Domain=requestModel.HashKey,
                            PackageStorage=newCartStorage
                        }
                    };

                    string strSave = JsonConvert.SerializeObject(saveCartStorage);
                    _distributedCache.SetString(requestModel.Key, strSave);
                }

                return BaseResponse<ResponseModel>.Success(resultModel);
            }
            catch (Exception ex)
            {
                return BaseResponse<ResponseModel>.InternalServerError(resultModel, message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<YachtPackageServiceModel>> GetRedisCacheYacht(string hashKey, string key, string yachtFId)
        {
            List<YachtPackageServiceModel> lstYacht = new List<YachtPackageServiceModel>();
            try
            {
                var value = _distributedCache.GetString(key);
                if (value != null)
                {

                    var lstRedisStorage = JsonConvert.DeserializeObject<List<RedisStorage>>(value);

                    //requestModel.HashKey ==> Dining or Yacht or other...
                    var RedisStorageModel = lstRedisStorage.FirstOrDefault(k => k.Domain == hashKey);
                    #region DOMAIN
                    if (RedisStorageModel != null)// DOMAIN IS  EXIST
                    {
                        var result = RedisStorageModel.PackageStorage;
                        //get Yacht infomation by Id
                        lstYacht = result.Where(k => k.YachtId == yachtFId).Select(k => k).ToList();
                    }
                    #endregion
                }

                return BaseResponse<List<YachtPackageServiceModel>>.Success(lstYacht);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<YachtPackageServiceModel>>.InternalServerError(lstYacht, message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<List<RedisStorage>> GetRedisCacheUser(string key)
        {
            List<RedisStorage> lstRedisCache = new List<RedisStorage>();
            try
            {
                var value = _distributedCache.GetString(key);
                if (value != null)
                {
                    lstRedisCache = JsonConvert.DeserializeObject<List<RedisStorage>>(value);                  
                    
                }

                return BaseResponse<List<RedisStorage>>.Success(lstRedisCache);
            }
            catch (Exception ex)
            {
                return BaseResponse<List<RedisStorage>>.InternalServerError(lstRedisCache, message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<ResponseModel> RemoveRedisCacheYacht(RedisCachesYachRemoveModel requestModel)
        {
            ResponseModel resultModel = new ResponseModel();
            try
            {

                var value = _distributedCache.GetString(requestModel.Key);
                if (value != null)
                {
                    var result = JsonConvert.DeserializeObject<List<YachtCartStorageViewModel>>(value);
                    //get Yacht infomation by Id

                    if (requestModel.isDeleteEntireYacht == true)//delete entier Yacht
                    {
                        result.RemoveAll(k => k.YachtId == requestModel.YachtId);
                    }
                    if (requestModel.isDeleteEntireYacht == false)//delete Service in Yacht
                    {
                        var yachtStoraged = result.FirstOrDefault(k => k.YachtId == requestModel.YachtId);
                        if (yachtStoraged != null)
                        {
                            if (yachtStoraged.ProductPackage.Count > 0)//Some Existing package in Yacht
                            {
                                yachtStoraged.ProductPackage.RemoveAll(x => x.productInventoryFId.Trim() == requestModel.productInventoryFId.Trim());

                                if (yachtStoraged.ProductPackage.Count == 0)
                                {
                                    result.RemoveAll(x => x.YachtId.Trim() == requestModel.YachtId.Trim());
                                }
                            }
                        }
                    }

                    string strSave = JsonConvert.SerializeObject(result);
                    _distributedCache.SetString(requestModel.Key, strSave);

                }
                return BaseResponse<ResponseModel>.Success(resultModel);
            }
            catch (Exception ex)
            {
                return BaseResponse<ResponseModel>.InternalServerError(resultModel, message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        #region Using Redis with simple key.
        BaseResponse<string> IRedisCacheService.SetSimpleKeyModel(RedisCachesModel entireData)
        {
            try
            {
                if (entireData != null)
                {
                    string key = entireData.Key;
                    string value = "";
                    if (entireData.CartStorage.Count > 0)
                    {
                        value = JsonConvert.SerializeObject(entireData.CartStorage);
                    }
                    _distributedCache.SetString(key, value);
                }

                return BaseResponse<string>.Success("1");
            }
            catch (Exception ex)
            {
                return BaseResponse<string>.InternalServerError("-1", message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<string> SetSimpleKey(string key, string value)
        {
            try
            {
                _distributedCache.SetString(key, value);

                return BaseResponse<string>.Success("1");
            }
            catch (Exception ex)
            {
                return BaseResponse<string>.InternalServerError("-1", message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<string> GetSimpleKey(string key)
        {
            try
            {
                var value = _distributedCache.GetString(key);
                return BaseResponse<string>.Success(value);
            }
            catch (Exception ex)
            {
                return BaseResponse<string>.InternalServerError("-1", message: ex.Message, fullMsg: ex.StackTrace);
            }
        }

        public BaseResponse<string> RemoveSimpleKey(string key)
        {
            try
            {
                _distributedCache.Remove(key);
                return BaseResponse<string>.Success("1");
            }
            catch (Exception ex)
            {
                return BaseResponse<string>.InternalServerError("-1", message: ex.Message, fullMsg: ex.StackTrace);
            }
        }
        #endregion


    }
}
