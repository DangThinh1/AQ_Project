using Identity.Core.Common;
using Identity.Core.Enums;
using Identity.Core.Portal.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Identity.Web.Helpers
{
    public static class DecryptCookiesAuthenticationHelper
    {
        private static IDataProtector _dataProtector;
        private readonly static IDataProtectionProvider _provider = DependencyInjectionHelper.GetService<IDataProtectionProvider>();
        public static ClaimsIdentity DecryptAQCookiesAuthentication(HttpContext context, DomainTypePortalEnum domainType, string cookiesAuthenticationKeyName = null)
        {
            string cookiesKey = cookiesAuthenticationKeyName;
            if (string.IsNullOrEmpty(cookiesKey))
            {
                switch (domainType)
                {
                    case DomainTypePortalEnum.AQSSOPortal:
                        cookiesKey = CookiesAuthenticationDomainConstant.AQSSOPortal;
                        break;
                    case DomainTypePortalEnum.AQYachtPortal:
                        cookiesKey = CookiesAuthenticationDomainConstant.AQYachtPortal;
                        break;
                    case DomainTypePortalEnum.AQDiningPortal:
                        cookiesKey = CookiesAuthenticationDomainConstant.AQDiningPortal;
                        break;
                    case DomainTypePortalEnum.AQEVisaPortal:
                        cookiesKey = CookiesAuthenticationDomainConstant.AQEVisaPortal;
                        break;
                    case DomainTypePortalEnum.AQHolidayPackagePortal:
                        cookiesKey = CookiesAuthenticationDomainConstant.AQHolidayPackagePortal;
                        break;
                    case DomainTypePortalEnum.AQAccommodationPortal:
                        cookiesKey = CookiesAuthenticationDomainConstant.AQAccommodationPortal;
                        break;
                    case DomainTypePortalEnum.AQFlightPortal:
                        cookiesKey = CookiesAuthenticationDomainConstant.AQFlightPortal;
                        break;
                    case DomainTypePortalEnum.AQCarRentalPortal:
                        cookiesKey = CookiesAuthenticationDomainConstant.AQCarRentalPortal;
                        break;
                    default:
                        cookiesKey = string.Empty;
                        break;
                }
            }

            //Get the encrypted cookie value
            var cookiesValue = context.Request.Cookies[cookiesKey];
            if (cookiesValue == null)
                return null;
            
            // Instantiate the data protection system at this folder
            _dataProtector = _provider?.CreateProtector(SecurityConstant.AQSecurityMasterProtector);
            //Get teh decrypted cookies as a Authentication Ticket
            TicketDataFormat ticketDataFormat = new TicketDataFormat(_dataProtector);
            AuthenticationTicket ticket = ticketDataFormat?.Unprotect(cookiesValue);

            return (ClaimsIdentity)ticket?.Principal?.Identity??null;
           
        }
    }
}
