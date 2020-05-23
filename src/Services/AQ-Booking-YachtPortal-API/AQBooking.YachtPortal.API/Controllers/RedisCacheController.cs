using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.YachtPortal.API.Helpers;
using AQBooking.YachtPortal.Core.Models.RedisCaches;
using AQBooking.YachtPortal.Infrastructure.Interfaces;
using Identity.Core.Conts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AQBooking.YachtPortal.API.Controllers
{
    [Route("api")]
    [ApiController]
    [LogHelper]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class RedisCacheController : Controller
    {
        // GET: /<controller>/
        private readonly IRedisCacheService _redisCacheService;
        public RedisCacheController(IRedisCacheService redisCacheService)
        {
            _redisCacheService = redisCacheService;
        }
        [HttpPost]
        [Route("RedisCache/RedisCacheYacht/HashKey/CopyLocalStorage/Model/Set")]
        public IActionResult CopyLocalStorageToRedisCache(RedisCachesModel requestModel)
        {
            var result = _redisCacheService.CopyLocalStorageToRedisCache(requestModel);
            return Ok(result);
        }
        [HttpPost]
        [Route("RedisCache/RedisCacheYacht/HashKey/Model/Set")]
        public IActionResult SetRedisCacheYachtModel([FromBody]RedisCachesYachtModel requestModel)
        {
            var result = _redisCacheService.SetRedisCacheYacht(requestModel);
            return Ok(result);
        }     

        [HttpGet]
        [Route("RedisCache/RedisCacheYacht/HashKey/Get/{hashKey}/{key}/{yachtFId}")]
        public IActionResult GetRedisCacheYacht(string hashKey, string key, string yachtFId)
        {
            var result = _redisCacheService.GetRedisCacheYacht( hashKey, key, yachtFId);
            return Ok(result);
        }
        [HttpPost]
        [Route("RedisCache/RedisCacheYacht/HashKey/Remove")]
        public IActionResult RemoveRedisCacheYacht(RedisCachesYachRemoveModel requestModel)
        {
            var result = _redisCacheService.RemoveRedisCacheYacht( requestModel);
            return Ok(result);
        }

       
        [HttpGet]
        [Route("RedisCache/RedisCache/Haskey/GetAll/{key}")]
        public IActionResult GetRedisCacheUser(string key)
        {
            var result = _redisCacheService.GetRedisCacheUser(key);
            return Ok(result);
        }

        [HttpPost]
        [Route("RedisCache/RedisCache/SimpleKey/Model/Set")]
        public IActionResult SetSimpleKeyModel( RedisCachesModel entireData)
        {
            var result = _redisCacheService.SetSimpleKeyModel(entireData);
            return Ok(result);
        }
        [HttpGet]
        [Route("RedisCache/RedisCache/SimpleKey/Set/{key}/{value}")]
        public IActionResult SetSimpleKey(string key, string value)
        {
            var result = _redisCacheService.SetSimpleKey(key, value);
            return Ok(result);
        }
        [HttpGet]
        [Route("RedisCache/RedisCache/SimpleKey/Get/{key}")]
        public IActionResult GetSimpleKey(string key)
        {
            var result = _redisCacheService.GetSimpleKey(key);
            return Ok(result);
        }
        [HttpGet]
        [Route("RedisCache/RedisCache/SimpleKey/Remove/{key}")]
        public IActionResult RemoveSimpleKey(string key)
        {
            var result = _redisCacheService.RemoveSimpleKey(key);
            return Ok(result);
        }
       
    }
}
