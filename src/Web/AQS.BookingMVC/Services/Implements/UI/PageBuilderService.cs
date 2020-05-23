using AQS.BookingMVC.Infrastructure.Enum;
using AQS.BookingMVC.Services.Interfaces.UI;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Services.Implements.UI
{
    public class PageBuilderService : IPageBuilderService
    {
        private Dictionary<ResourceLocation,List<string>> _scriptParts;
        private List<string> _inlineScripts;
        private Dictionary<ResourceLocation,List<string>> _cssParts;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IActionContextAccessor _actionContextAccessor;
        public PageBuilderService(IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor
            )
        {
            _scriptParts = new Dictionary<ResourceLocation, List<string>>();
            _inlineScripts = new List<string>();
            _cssParts = new Dictionary<ResourceLocation, List<string>>();
            _urlHelperFactory = urlHelperFactory;
            _actionContextAccessor = actionContextAccessor;
        }
      

        public void AppendCss(ResourceLocation resourceLocation, params string[] cssParts)
        {
            if (!_cssParts.ContainsKey(resourceLocation))
                _cssParts.Add(resourceLocation, cssParts.ToList());
            else
                _cssParts[resourceLocation].AddRange(cssParts.ToList());
        }

        public void AppendScripts(ResourceLocation resourceLocation, params string[] scriptParts)
        {
            if (!_scriptParts.ContainsKey(resourceLocation))
                _scriptParts.Add(resourceLocation, scriptParts.ToList());
            else
            {
              
                _scriptParts[resourceLocation].AddRange(scriptParts.ToList());
            }
               
        }
        public void AppendInlineScript(params string[] scriptParts)
        {
            _inlineScripts.AddRange(scriptParts.ToList());
        }
        public string GenerateCss(ResourceLocation location)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);
            if (!_cssParts.ContainsKey(location)||!_cssParts[location].Any())
                return string.Empty;
            StringBuilder result = new StringBuilder();

            foreach(var css in _cssParts[location])
            {
                result.AppendFormat("<link href=\"{0}\" rel=\"stylesheet\" type=\"{1}\" />", urlHelper.Content(css),"text/css");
                result.Append(Environment.NewLine);
            }
            return result.ToString();
        }
        public string GenerateScript(ResourceLocation location)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);
            StringBuilder result = new StringBuilder();
            if (_scriptParts.ContainsKey(location) && _scriptParts[location].Any())
            {
               

                foreach (var script in _scriptParts[location])
                {
                    result.AppendFormat("<script src=\"{0}\"></script>", urlHelper.Content(script));
                    result.Append(Environment.NewLine);
                }
            }         
            return result.ToString();
        }
        public string GenerateInlineScripts(ResourceLocation location)
        {
            if (_inlineScripts==null)
                return "";

            if (!_inlineScripts.Any())
                return "";

            var result = new StringBuilder();
            foreach (var item in _inlineScripts)
            {
                result.Append(item);
                result.Append(Environment.NewLine);
            }
            return result.ToString();
        }
    }
}
