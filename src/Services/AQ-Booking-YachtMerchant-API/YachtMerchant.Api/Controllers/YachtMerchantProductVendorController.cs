using AQBooking.Core.Helpers;
using Identity.Core.Conts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using YachtMerchant.Core.Models.YachtMerchantProductInventories;
using YachtMerchant.Core.Models.YachtMerchantProductVendors;
using YachtMerchant.Infrastructure.Interfaces;

namespace YachtMerchant.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    public class YachtMerchantProductVendorController : ControllerBase
    {
        private readonly IYachtMerchantProductVendorServices _yachtMerchantProductVendorServices;

        public YachtMerchantProductVendorController(IYachtMerchantProductVendorServices yachtMerchantProductVendorServices)
        {
            _yachtMerchantProductVendorServices = yachtMerchantProductVendorServices;
        }

        [HttpGet]
        [Route("YachtMerchantProductVendor")]
        public IActionResult Search([FromQuery]YachtMerchantProductVendorSearchModel model)
        {
            var result = _yachtMerchantProductVendorServices.Search(model);
            if(result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpGet]
        [Route("YachtMerchantProductVendor/GetSelectListVendor/{id}/{categoryId}")]
        public IActionResult GetSelectListVendorByMerchantId(int id, int categoryId)
        {
            var res = _yachtMerchantProductVendorServices.GetAllProductVendorByMerchantId(id, categoryId).Result;
            if(res.IsSuccessStatusCode)
                return Ok(res);
            return BadRequest();
        }

        [HttpGet]
        [Route("YachtMerchantProductVendor/{id}")]
        public IActionResult GetYachtMerchantProductVendorById(int id)
        {
            var res = _yachtMerchantProductVendorServices.GetYachtMerchantProductVendorById(id);
            if(res.IsSuccessStatusCode)
                return Ok(res);
            return BadRequest();
        }

        [HttpGet]
        [Route("YachtMerchantProductVendor/GetProductSupplierByVendorId/{id}")]
        public IActionResult GetProductSuppliserByVendorId(int id)
        {
            var result = _yachtMerchantProductVendorServices.GetProductSupplierByVendorId(id);
            if(result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpPost]
        [Route("YachtMerchantProductVendor")]
        public IActionResult CreateYachtMerchantProductVendor(YachtMerchantProductVendorCreateModel model)
        {
            var res = _yachtMerchantProductVendorServices.CreateYachtMerchantProductVendor(model).Result;
            if( res.IsSuccessStatusCode)
                return Ok(res);
            return BadRequest();
        }

        [HttpPut]
        [Route("YachtMerchantProductVendor")]
        public IActionResult UpdateYachtMerchantProductVendor(YachtMerchantProductVendorUpdateModel model)
        {
            var res = _yachtMerchantProductVendorServices.UpdateYachtMerchantProductVendor(model).Result;
            if (res.IsSuccessStatusCode)
                return Ok(res);
            return BadRequest();
        }

        [HttpDelete]
        [Route("YachtMerchantProductVendor/{id}")]
        public IActionResult DeleteYachtMerchantProductVendor(int id)
        {
            var res = _yachtMerchantProductVendorServices.DeleteYachtMerchantProductVendor(id).Result;
            if(res.IsSuccessStatusCode)
                return Ok(res);
            return BadRequest();
        }

        [HttpPost]
        [Route("YachtMerchantProductVendor/ProductSupplierByVendor")]
        public IActionResult CreateProductSupplier(YachtMerchantProductSupplierViewModel model)
        {
            var result = _yachtMerchantProductVendorServices.CreateProductSupplier(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpDelete]
        [Route("YachtMerchantProductVendor/DeleteProductByVendor/{vendorId}/{productId}")]
        public IActionResult DeleteProductByVendor(int vendorId, int productid)
        {
            var result = _yachtMerchantProductVendorServices.DeleteProductByVendor(vendorId, productid);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }
    }
}