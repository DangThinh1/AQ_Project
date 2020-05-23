using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using YachtMerchant.Infrastructure.Interfaces.YachtTours;
using YachtMerchant.Core.Models.YachtTourFileStream;
using System.Collections.Generic;
using AQEncrypts;
using Identity.Core.Conts;
using Microsoft.AspNetCore.Cors;
using System;

namespace YachtMerchant.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class YachtTourFileStreamController : ControllerBase
    {
        private readonly IYachtTourFileStreamService _yachtTourFileStreamService;
        public YachtTourFileStreamController(IYachtTourFileStreamService yachtTourFileStreamService)
        {
            _yachtTourFileStreamService = yachtTourFileStreamService;
        }

        [HttpPost("YachtTourFileStreams/YachtTourEncryptedId/{yachtTourEncryptedId}")]
        public IActionResult Create([FromBody]List<YachtTourFileStreamCreateModel> models, string yachtTourEncryptedId)
        {
            var id = DecryptValue(yachtTourEncryptedId);
            if(models == null || models.Count == 0 || id == 0)
                return BadRequest();

            var baseresponse = _yachtTourFileStreamService.Create(models, id);
            if (baseresponse.IsSuccessStatusCode)
                return Ok(baseresponse);
            return BadRequest();
        }
       
        [HttpGet("YachtTourFileStreams/{id}")]
        public IActionResult FindByYachtTourEncryptedId(long id)
        {
            var baseresponse = _yachtTourFileStreamService.GetFileStreamById(id);
            if (baseresponse.IsSuccessStatusCode)
                return Ok(baseresponse);
            return BadRequest();
        }

        [HttpGet("YachtTourFileStreams")]
        public IActionResult FindByYachtTourEncryptedId([FromQuery] YachtTourFileStreamSearchModel searchModel)
        {
            var id = DecryptValue(searchModel.YachtTourEncryptedId);
            if (id == 0)
                return BadRequest();
            var baseresponse = _yachtTourFileStreamService.GetFileStreamsByTourId(id, searchModel);
            if (baseresponse.IsSuccessStatusCode)
                return Ok(baseresponse);
            return BadRequest();
        }

        [HttpPost("YachtTourFileStreams/Update/{fileId}")]
        public IActionResult UpdateYachtTour(YachtTourFileStreamUpdateModel model,int fileId)
        {
            
            var rs = _yachtTourFileStreamService.Update(model, fileId);
            if (rs.IsSuccessStatusCode)
                return Ok(rs);
            return BadRequest();
           
        }

        [HttpDelete("YachtTourFileStreams/Delete/{id}")]
        public IActionResult DeleteYachtTour(int id)
        {
            
            var rs = _yachtTourFileStreamService.Delete(id);
            if (rs.IsSuccessStatusCode)
                return Ok(rs);
            return BadRequest();
            
        }
        private int DecryptValue(string encryptedString)
        {
            try
            {
                var decryptedValue = Terminator.Decrypt(encryptedString);
                var intValue = int.Parse(decryptedValue);
                return intValue;
            }
            catch
            {
                return 0;
            }
        }
    }
}