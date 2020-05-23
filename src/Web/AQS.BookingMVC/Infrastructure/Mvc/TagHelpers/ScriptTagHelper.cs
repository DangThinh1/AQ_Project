using AQS.BookingMVC.Infrastructure.Enum;
using AQS.BookingMVC.Infrastructure.Extensions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace AQS.BookingMVC.Infrastructure.Mvc.TagHelpers
{
    [HtmlTargetElement("script",Attributes = LOCATION_ATTRIBUTE_NAME)]
    public class ScriptTagHelper: TagHelper
    {
        private const string LOCATION_ATTRIBUTE_NAME = "asp-location";
        private readonly IHtmlHelper _htmlHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        /// <summary>
        /// Indicates where the script should be rendered
        /// </summary>
        [HtmlAttributeName(LOCATION_ATTRIBUTE_NAME)]
        public ResourceLocation Location { set; get; }

        /// <summary>
        /// ViewContext
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }
        public ScriptTagHelper(IHtmlHelper htmlHelper,
            IHttpContextAccessor httpContextAccessor)
        {
            _htmlHelper = htmlHelper;
            _httpContextAccessor = httpContextAccessor;
        }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }           

            //contextualize IHtmlHelper
            var viewContextAware = _htmlHelper as IViewContextAware;
            viewContextAware?.Contextualize(ViewContext);

            //get JavaScript
            var script = output.GetChildContentAsync().Result.GetContent();

            //build script tag
            var scriptTag = new TagBuilder("script");
            scriptTag.InnerHtml.SetHtmlContent(new HtmlString(script));

            //merge attributes
            foreach (var attribute in context.AllAttributes)
                if (!attribute.Name.StartsWith("asp-"))
                    scriptTag.Attributes.Add(attribute.Name, attribute.Value.ToString());

            _htmlHelper.AddInlineScriptParts(scriptTag.RenderHtmlContent());

            //generate nothing
            output.SuppressOutput();
        }

    }
    
}
