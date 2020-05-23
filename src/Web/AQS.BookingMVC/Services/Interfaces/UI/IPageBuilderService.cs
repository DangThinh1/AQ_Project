using AQS.BookingMVC.Infrastructure.Enum;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Services.Interfaces.UI
{
    public interface IPageBuilderService
    {
        #region Scripts
        /// <summary>
        /// Append javascript file to head or footer
        /// </summary>
        /// <param name="resourceLocation">resource location</param>
        /// <param name="scriptParts">list of script file path</param>
        void AppendScripts(ResourceLocation resourceLocation, params string[] scriptParts);
        /// <summary>
        /// Append inline script
        /// </summary>
        /// <param name="scriptParts">List of inline scripts</param>
        void AppendInlineScript(params string[] scriptParts);
        /// <summary>
        /// Generate scripts appended to view
        /// </summary>
        /// <param name="location">location head or footer</param>
        /// <returns>
        /// combine script tag 
        /// </returns>
        string GenerateScript(ResourceLocation location);
        #endregion
        #region Css
        /// <summary>
        ///  Append css file to head or footer
        /// </summary>
        /// <param name="resourceLocation">resource location</param>
        /// <param name="csParts">list of css file path</param>
        void AppendCss(ResourceLocation resourceLocation, params string[] csParts);
        /// <summary>
        /// Generate css appended to view
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        string GenerateCss(ResourceLocation location);
        /// <summary>
        ///  Generate inline js appended to view
        /// </summary>
        /// <param name="location">resource location</param>
        /// <returns></returns>
        string GenerateInlineScripts(ResourceLocation location);
        #endregion
    }
}
