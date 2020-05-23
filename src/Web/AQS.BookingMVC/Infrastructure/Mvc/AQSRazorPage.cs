using AQConfigurations.Core.Services.Interfaces;
using AQS.BookingMVC.Infrastructure.Helpers;
using AQS.BookingMVC.Services.Interfaces.Common;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AQS.BookingMVC.Infrastructure.Mvc
{
    /// <summary>
    /// Web view page
    /// This class help to use Languge result in the view like @T("RESOURCE KEY")
    /// </summary>
    /// <typeparam name="TModel">Model</typeparam>
    public abstract class AQSRazorPage<TModel> : Microsoft.AspNetCore.Mvc.Razor.RazorPage<TModel>
    {
        private  ILanguageService   _languageService;
        private Localizer _localizer;

        /// <summary>
        /// Get a localized resources
        /// </summary>
        public Localizer R
        {
            get
            {
                if (_languageService == null)
                    _languageService = DependencyInjectionHelper.GetService<ILanguageService>() ;

                if (_localizer == null)
                {
                    _localizer = (format, args) =>
                    {
                        var resFormat = _languageService.GetResource(format);
                        if (string.IsNullOrEmpty(resFormat))
                        {
                            return new LocalizedString(resFormat);
                        }
                        return new LocalizedString((args == null || args.Length == 0)
                            ? resFormat
                            : string.Format(resFormat, args));
                    };
                }
                return _localizer;
            }
        }

       
    }
   
    public abstract class AQSRazorPage : AQSRazorPage<dynamic>
    {
    }
    public delegate LocalizedString Localizer(string text, params object[] args);

    public class LocalizedString : HtmlString
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="localized">Localized value</param>
        public LocalizedString(string localized) : base(localized)
        {
            Text = localized;
        }

        /// <summary>
        /// Text
        /// </summary>
        public string Text { get; }
    }
}
