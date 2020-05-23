using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AQBooking.Admin.Core.Models.Subscriber;
using AQBooking.Admin.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AQBooking.Admin.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class SubscriberController : BaseApiController
    {
        #region Fields
        private readonly ISubscriberService _subscriberService;
        #endregion

        #region Ctor

        public SubscriberController(ISubscriberService subscriberService)
        {
            _subscriberService = subscriberService;
        }
        #endregion

        #region Methods
        [Route("Subscriber")]
        [HttpPost]
        public async Task<IActionResult> CreateNewSubscriber([FromBody]SubscriberCreateModel model)
        {
            var res = await _subscriberService.CreateNewSubscriber(model);
            return Ok(res);
        }

        [Route("SubscriberSearch")]
        [HttpGet]
        public IActionResult SearchPost([FromQuery]SubscriberSearchModel model)
        {
            try
            {
                var res = _subscriberService.SearchSubscriber(model);
                if (res != null)
                    return OkBaseResponse(res);
                return ErrorBaseResponse(System.Net.HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return ErrorBaseResponse(ex);
            }
        }
        [Route("SubcriberLstExcel")]
        [HttpPost]
        public IActionResult GetLstToExport([FromBody]SubscriberSearchModel model)
        {
            try
            {
                var res = _subscriberService.GetListSubToExport(model);
                if (res != null)
                    return OkBaseResponse(res);
                return ErrorBaseResponse(System.Net.HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return ErrorBaseResponse(ex);
            }
        }
        #endregion
    }
}