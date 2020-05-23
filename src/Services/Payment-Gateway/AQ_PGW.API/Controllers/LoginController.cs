using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AQ_PGW.Common.Important;
using AQ_PGW.Core.Interfaces;
using AQ_PGW.Core.Models.DBTables;
using AQ_PGW.Core.Models.Model;
using AQ_PGW.Core.ViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stripe;
using static AQ_PGW.Common.DTO;
//using AES_Encryption;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace AQ_PGW.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private ILoginServiceRepository _ILoginService;

        public LoginController(ILoginServiceRepository ILoginService)
        {
            _ILoginService = ILoginService;
        }
        // GET api/values
        [HttpPost]
        [Route("GetToken")]
        public ActionResult GetToken(string user, string pass)
        {
            APIResponseData responseData = new APIResponseData();
            responseData.StatusCode = 0;
            responseData.Message = "Failed.";
            responseData.Result.Data = "";

            try
            {
                var token = _ILoginService.Login(user, pass);
                if (!string.IsNullOrEmpty(token))
                {
                    responseData.Message = "Successfully.";
                    responseData.StatusCode = 1;
                    responseData.Result.Data = token;
                }                
            }
            catch (Exception ex)
            {
                responseData.Message = "Something went wrong, please try again.";
                responseData.StatusCode = 0;

                EmailHelpers.SendEmail(new Common.DTO.ErrorInfo()
                {
                    Section = "AQ GetToken",
                    Exception = ex
                });
            }

            return Ok(responseData);
        }


        [Authorize]
        [HttpGet]
        [Route("GetTest")]
        public ActionResult GetTest()
        {
            return Ok("success");
        }
    }

}