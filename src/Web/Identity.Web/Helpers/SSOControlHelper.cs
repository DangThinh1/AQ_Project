using Identity.Core.Portal.Conts;
using Identity.Core.Portal.Models.SigninControls;
using Identity.Web.AppConfig;
using System.Collections.Generic;
using System.Linq;

namespace Identity.Web.Helpers
{
    public class SSOControlHelper
    {
        public static List<SigninFlowModel> SigninFlows
        {
            get
            {
                var portalControl = ApiUrlHelper.SigninControls;
                if (portalControl == null)
                    return new List<SigninFlowModel>();
                var flows = portalControl.SignFlow;
                var portals = portalControl.Portals;
                var signinFlows = new List<SigninFlowModel>();
                foreach (var currentFlow in flows)
                {
                    var fromPortal = portals.FirstOrDefault(k => k.DomainId == currentFlow.FromDomainId);
                    var toPortal = portals.FirstOrDefault(k => k.DomainId == currentFlow.ToDomainId);
                    if (fromPortal != null && toPortal != null)
                    {
                        var toPortalRedirectUrl = $"{toPortal.Host}{toPortal.LoginNextPath}";
                        if(toPortal.DomainId == SSOConts.SSO_PORTAL_DOMAIN_ID)
                        {
                            toPortalRedirectUrl = $"{toPortal.Host}{toPortal.LoginEndPath}";
                        }
                        signinFlows.Add(new SigninFlowModel()
                        {
                            DomainId = fromPortal.DomainId,
                            RedirectUrl = toPortalRedirectUrl
                        });
                    }
                }
                return signinFlows;
            }
        }
        public static List<SigninFlowModel> SignoutFlows
        {
            get
            {
                var portalControl = ApiUrlHelper.SigninControls;
                if (portalControl == null)
                    return new List<SigninFlowModel>();
                var flows = portalControl.SignFlow;
                var portals = portalControl.Portals;
                var signinFlows = new List<SigninFlowModel>();
                foreach (var currentFlow in flows)
                {
                    var fromPortal = portals.FirstOrDefault(k => k.DomainId == currentFlow.FromDomainId);
                    var toPortal = portals.FirstOrDefault(k => k.DomainId == currentFlow.ToDomainId);
                    if (fromPortal != null && toPortal != null)
                    {
                        var toPortalRedirectUrl = $"{toPortal.Host}{toPortal.LogoutNextPath}";
                        if (toPortal.DomainId == SSOConts.SSO_PORTAL_DOMAIN_ID)
                        {
                            toPortalRedirectUrl = $"{toPortal.Host}{toPortal.LogoutEndPath}";
                        }
                        signinFlows.Add(new SigninFlowModel()
                        {
                            DomainId = fromPortal.DomainId,
                            RedirectUrl = toPortalRedirectUrl
                        });
                    }
                }
                return signinFlows;
            }
        }
    }
}