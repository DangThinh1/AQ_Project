using AQEncrypts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using YachtMerchant.Core.Models.YachtExternalRefLinks;
using YachtMerchant.Infrastructure.Interfaces.YachtTours;
using Identity.Core.Conts;
using YachtMerchant.Core.Models.YachtTourExternalRefLinks;

namespace YachtMerchant.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [EnableCors(AQCorsPolicy.DefaultScheme)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class YachtTourExternalRefLinksController : ControllerBase
    {
        private readonly IYachtTourExternalRefLinkServices _yachtExternalRefLinkServices;

        public YachtTourExternalRefLinksController(IYachtTourExternalRefLinkServices yachtExternalRefLinkServices)
        {
            _yachtExternalRefLinkServices = yachtExternalRefLinkServices;
        }

        [HttpGet]
        [Route("YachtTourExternalRefLinks")]
        public IActionResult GetAll()
        {
            var result = _yachtExternalRefLinkServices.GetAll();
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpGet]
        [Route("YachtTourExternalRefLinks/{id}")]
        public IActionResult Detail(long id)
        {
            var result = _yachtExternalRefLinkServices.FindById(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpPost]
        [Route("YachtTourExternalRefLinks")]
        public IActionResult Create(YachtTourExternalRefLinkModel model)
        {
            var result = _yachtExternalRefLinkServices.Create(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpPut]
        [Route("YachtTourExternalRefLinks")]
        public IActionResult Update(YachtTourExternalRefLinkModel model)
        {
            var result = _yachtExternalRefLinkServices.Update(model);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }

        [HttpDelete]
        [Route("YachtTourExternalRefLinks/{id}")]
        public IActionResult Delelte(long id)
        {
            var result = _yachtExternalRefLinkServices.Delete(id);
            if (result.IsSuccessStatusCode)
                return Ok(result);
            return BadRequest();
        }
        [HttpGet]
        [Route("YachtTourExternalRefLinks/YachTour")]
        public IActionResult GetByYachtTour([FromQuery] YachtTourExternalRefLinkSearchModel searchModel)
        {
            int tourId = DecryptValue(searchModel.YachtIdEncrypted);
            var result = _yachtExternalRefLinkServices.GetByYachtTourId(tourId, searchModel);
            if (result.IsSuccessStatusCode)
                return Ok(result);
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