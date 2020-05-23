using AQS.BookingMVC.Infrastructure.Enum;
using AQS.BookingMVC.Infrastructure.Helpers;
using AQS.BookingMVC.Services.Interfaces.UI;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Infrastructure.Extensions
{
    public static class LayoutExtentions
    {
        #region JS / CSS
        public static string RenderHtmlContent(this IHtmlContent htmlContent)
        {
            using var writer = new StringWriter();
            htmlContent.WriteTo(writer, HtmlEncoder.Default);
            var htmlOutput = writer.ToString();
            return htmlOutput;
        }

        public static void AppendScriptParts(this IHtmlHelper html, ResourceLocation location, params string[] parts)
        {
            var pageBuilderService = DependencyInjectionHelper.GetService<IPageBuilderService>();
            pageBuilderService.AppendScripts(location,parts);
        }
        public static void AppendCssParts(this IHtmlHelper html, ResourceLocation location, params string[] parts)
        {
            var pageBuilderService = DependencyInjectionHelper.GetService<IPageBuilderService>();
            pageBuilderService.AppendCss(location, parts);
        }
        public static void AppendInlineScripts(this IHtmlHelper html, ResourceLocation location, params string[] parts)
        {
            var pageBuilderService = DependencyInjectionHelper.GetService<IPageBuilderService>();
            pageBuilderService.AppendInlineScript(parts);
        }
        public static void AddInlineScriptParts(this IHtmlHelper html,string inlineScripts)
        {
            var pageBuilderService = DependencyInjectionHelper.GetService<IPageBuilderService>();
            pageBuilderService.AppendInlineScript(inlineScripts);
        }
        public static IHtmlContent GenerateInlineScript(this IHtmlHelper html)
        {
            var pageBuilderService = DependencyInjectionHelper.GetService<IPageBuilderService>();
            return new HtmlString(pageBuilderService.GenerateInlineScripts(ResourceLocation.Footer));
        }
        public static IHtmlContent GenerateScripts(this IHtmlHelper html, ResourceLocation location)
        {
            var pageBuilderService = DependencyInjectionHelper.GetService<IPageBuilderService>();
            return new HtmlString(pageBuilderService.GenerateScript(location));
        }
        public static IHtmlContent GenerateCss(this IHtmlHelper html, ResourceLocation location)
        {
            var pageBuilderService = DependencyInjectionHelper.GetService<IPageBuilderService>();
            return new HtmlString(pageBuilderService.GenerateCss(location));
        }
        #endregion
    }

}
