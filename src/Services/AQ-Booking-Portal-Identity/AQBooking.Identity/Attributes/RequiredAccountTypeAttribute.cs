using System.Linq;
using APIHelpers.Response;
using Identity.Core.Enums;
using Identity.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Identity.Api.Attributes
{
    public class RequiredAccountTypeAttribute : ActionFilterAttribute
    {
        private List<string> _accountTypeAsStringFormat;
        public RequiredAccountTypeAttribute(AccountTypeEnum accountType = AccountTypeEnum.Admin)
        {
            _accountTypeAsStringFormat = new List<string>()
            {
                ((int)accountType).ToString()
            };
        }

        public RequiredAccountTypeAttribute(AccountTypeEnum[] accountTypes)
        {
            _accountTypeAsStringFormat = accountTypes.Select(k=> ((int)k).ToString()).ToList();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var controller = (ControllerBase)context.Controller;
            try
            {
                var profile = JwtTokenHelper.GetSignInProfile(controller.User);
                if (profile == null || string.IsNullOrEmpty(profile.AccountTypeFid) || !_accountTypeAsStringFormat.Contains(profile.AccountTypeFid))
                    context.Result = controller.Ok(BaseResponse<bool>.BadRequest());
            }
            catch
            {
                context.Result = controller.Ok(BaseResponse<bool>.BadRequest());
            }
        }
    }
}
