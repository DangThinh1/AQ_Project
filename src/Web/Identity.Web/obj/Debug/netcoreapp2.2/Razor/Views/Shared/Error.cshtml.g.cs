#pragma checksum "C:\Users\ThinkPro\Source\Repos\AQBooking_Solutions\src\Web\Identity.Web\Views\Shared\Error.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2c1e3c8d11cc9a501f42db8410b63255b50a9335"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_Error), @"mvc.1.0.view", @"/Views/Shared/Error.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Shared/Error.cshtml", typeof(AspNetCore.Views_Shared_Error))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Users\ThinkPro\Source\Repos\AQBooking_Solutions\src\Web\Identity.Web\Views\_ViewImports.cshtml"
using Identity.Web;

#line default
#line hidden
#line 2 "C:\Users\ThinkPro\Source\Repos\AQBooking_Solutions\src\Web\Identity.Web\Views\_ViewImports.cshtml"
using Identity.Web.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2c1e3c8d11cc9a501f42db8410b63255b50a9335", @"/Views/Shared/Error.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"96d57be4b86ec9a433a069589ac0a862aab8f6ee", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_Error : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ErrorViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 2 "C:\Users\ThinkPro\Source\Repos\AQBooking_Solutions\src\Web\Identity.Web\Views\Shared\Error.cshtml"
  
    ViewData["Title"] = "Error";

#line default
#line hidden
            BeginContext(64, 120, true);
            WriteLiteral("\r\n<h1 class=\"text-danger\">Error.</h1>\r\n<h2 class=\"text-danger\">An error occurred while processing your request.</h2>\r\n\r\n");
            EndContext();
#line 9 "C:\Users\ThinkPro\Source\Repos\AQBooking_Solutions\src\Web\Identity.Web\Views\Shared\Error.cshtml"
 if (Model.ShowRequestId)
{

#line default
#line hidden
            BeginContext(214, 52, true);
            WriteLiteral("    <p>\r\n        <strong>Request ID:</strong> <code>");
            EndContext();
            BeginContext(267, 15, false);
#line 12 "C:\Users\ThinkPro\Source\Repos\AQBooking_Solutions\src\Web\Identity.Web\Views\Shared\Error.cshtml"
                                      Write(Model.RequestId);

#line default
#line hidden
            EndContext();
            BeginContext(282, 19, true);
            WriteLiteral("</code>\r\n    </p>\r\n");
            EndContext();
#line 14 "C:\Users\ThinkPro\Source\Repos\AQBooking_Solutions\src\Web\Identity.Web\Views\Shared\Error.cshtml"
}

#line default
#line hidden
            BeginContext(304, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 16 "C:\Users\ThinkPro\Source\Repos\AQBooking_Solutions\src\Web\Identity.Web\Views\Shared\Error.cshtml"
 if (Model.ShowMessage)
{

#line default
#line hidden
            BeginContext(334, 26, true);
            WriteLiteral("    <h2>\r\n        <strong>");
            EndContext();
            BeginContext(361, 13, false);
#line 19 "C:\Users\ThinkPro\Source\Repos\AQBooking_Solutions\src\Web\Identity.Web\Views\Shared\Error.cshtml"
           Write(Model.Message);

#line default
#line hidden
            EndContext();
            BeginContext(374, 22, true);
            WriteLiteral("</strong>\r\n    </h2>\r\n");
            EndContext();
#line 21 "C:\Users\ThinkPro\Source\Repos\AQBooking_Solutions\src\Web\Identity.Web\Views\Shared\Error.cshtml"
}
else
{

#line default
#line hidden
            BeginContext(408, 615, true);
            WriteLiteral(@"    <h3>Development Mode</h3>
    <p>
        Swapping to <strong>Development</strong> environment will display more detailed information about the error that occurred.
    </p>
    <p>
        <strong>The Development environment shouldn't be enabled for deployed applications.</strong>
        It can result in displaying sensitive information from exceptions to end users.
        For local debugging, enable the <strong>Development</strong> environment by setting the <strong>ASPNETCORE_ENVIRONMENT</strong> environment variable to <strong>Development</strong>
        and restarting the app.
    </p>
");
            EndContext();
#line 34 "C:\Users\ThinkPro\Source\Repos\AQBooking_Solutions\src\Web\Identity.Web\Views\Shared\Error.cshtml"
}

#line default
#line hidden
            BeginContext(1026, 6, true);
            WriteLiteral("\r\n\r\n\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ErrorViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
