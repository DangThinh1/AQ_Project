#pragma checksum "C:\Users\ThinkPro\Source\Repos\AQBooking_Solutions\src\Web\AQS.BookingAdmin\Views\PostCategory\_CreateUpdateModal.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "ecda42bd7423cdcc2705839a8ad151589b3ffdce"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_PostCategory__CreateUpdateModal), @"mvc.1.0.view", @"/Views/PostCategory/_CreateUpdateModal.cshtml")]
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
#nullable restore
#line 1 "C:\Users\ThinkPro\Source\Repos\AQBooking_Solutions\src\Web\AQS.BookingAdmin\Views\_ViewImports.cshtml"
using AQS.BookingAdmin;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\ThinkPro\Source\Repos\AQBooking_Solutions\src\Web\AQS.BookingAdmin\Views\_ViewImports.cshtml"
using AQS.BookingAdmin.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\ThinkPro\Source\Repos\AQBooking_Solutions\src\Web\AQS.BookingAdmin\Views\_ViewImports.cshtml"
using AQS.BookingAdmin.Infrastructure.Extensions;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\ThinkPro\Source\Repos\AQBooking_Solutions\src\Web\AQS.BookingAdmin\Views\_ViewImports.cshtml"
using AQS.BookingAdmin.Models.Common;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\ThinkPro\Source\Repos\AQBooking_Solutions\src\Web\AQS.BookingAdmin\Views\_ViewImports.cshtml"
using AQS.BookingAdmin.Infrastructure.AQPagination;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "C:\Users\ThinkPro\Source\Repos\AQBooking_Solutions\src\Web\AQS.BookingAdmin\Views\PostCategory\_CreateUpdateModal.cshtml"
using AQBooking.Admin.Core.Models.PostCategories;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ecda42bd7423cdcc2705839a8ad151589b3ffdce", @"/Views/PostCategory/_CreateUpdateModal.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fbc538716caba8d61ece7bb69ee98231840614e7", @"/Views/_ViewImports.cshtml")]
    public class Views_PostCategory__CreateUpdateModal : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<PostCategoriesCreateModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("frmAddUpdatePostCate"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n<div class=\"modal-dialog \">\r\n    <div class=\"modal-content\" id=\"loadContentModal\">\r\n        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "ecda42bd7423cdcc2705839a8ad151589b3ffdce4944", async() => {
                WriteLiteral("\r\n            ");
#nullable restore
#line 7 "C:\Users\ThinkPro\Source\Repos\AQBooking_Solutions\src\Web\AQS.BookingAdmin\Views\PostCategory\_CreateUpdateModal.cshtml"
       Write(Html.HiddenFor(x => x.Id));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n            ");
#nullable restore
#line 8 "C:\Users\ThinkPro\Source\Repos\AQBooking_Solutions\src\Web\AQS.BookingAdmin\Views\PostCategory\_CreateUpdateModal.cshtml"
       Write(Html.HiddenFor(x => x.PostCateDetailId));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n            <div class=\"modal-header\">\r\n                <h4 class=\"modal-title\"> ");
#nullable restore
#line 10 "C:\Users\ThinkPro\Source\Repos\AQBooking_Solutions\src\Web\AQS.BookingAdmin\Views\PostCategory\_CreateUpdateModal.cshtml"
                                     Write(Model.Id == 0 ? "Create new post category" : "Update post category");

#line default
#line hidden
#nullable disable
                WriteLiteral(@"</h4>
                <button type=""button"" class=""close"" data-dismiss=""modal"" aria-label=""Close"">
                    <span aria-hidden=""true"">×</span>
                </button>
            </div>
            <div class=""modal-body"">
                <div");
                BeginWriteAttribute("class", " class=\"", 720, "\"", 728, 0);
                EndWriteAttribute();
                WriteLiteral(">\r\n                    <div class=\"form-group\">\r\n                        <label for=\"ParentFid\" class=\"required\">Parent category</label>\r\n                        ");
#nullable restore
#line 19 "C:\Users\ThinkPro\Source\Repos\AQBooking_Solutions\src\Web\AQS.BookingAdmin\Views\PostCategory\_CreateUpdateModal.cshtml"
                   Write(Html.DropDownList("ParentFid", ViewBag.lstParent, "Please select", new { @class = "form-control" }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                    </div>\r\n                    <div class=\"form-group\">\r\n                        <label for=\"DefaultName\" class=\"required\">Default Name</label>\r\n                        ");
#nullable restore
#line 23 "C:\Users\ThinkPro\Source\Repos\AQBooking_Solutions\src\Web\AQS.BookingAdmin\Views\PostCategory\_CreateUpdateModal.cshtml"
                   Write(Html.TextBoxFor(x => x.DefaultName, new { @class = "form-control", @required = "required" }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                    </div>\r\n                    <div class=\"form-group\">\r\n                        <label class=\"required\">Languages</label>\r\n                        ");
#nullable restore
#line 27 "C:\Users\ThinkPro\Source\Repos\AQBooking_Solutions\src\Web\AQS.BookingAdmin\Views\PostCategory\_CreateUpdateModal.cshtml"
                   Write(Html.DropDownList("LanguageFid", ViewBag.lstLang, "Please select", new { @class = "form-control", @required = "required" }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                    </div>\r\n                    <div class=\"form-group\">\r\n                        <label for=\"Name\" class=\"required\">Name</label>\r\n                        ");
#nullable restore
#line 31 "C:\Users\ThinkPro\Source\Repos\AQBooking_Solutions\src\Web\AQS.BookingAdmin\Views\PostCategory\_CreateUpdateModal.cshtml"
                   Write(Html.TextBoxFor(x => x.Name, new { @class = "form-control", @required = "required", @maxlength = "255" }));

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n                    </div>\r\n                    <div class=\"form-group\">\r\n                        <label");
                BeginWriteAttribute("class", " class=\"", 1948, "\"", 1956, 0);
                EndWriteAttribute();
                WriteLiteral(">Order by</label>\r\n                        <select required class=\"form-control\"");
                BeginWriteAttribute("id", " id=\"", 2037, "\"", 2056, 1);
#nullable restore
#line 35 "C:\Users\ThinkPro\Source\Repos\AQBooking_Solutions\src\Web\AQS.BookingAdmin\Views\PostCategory\_CreateUpdateModal.cshtml"
WriteAttributeValue("", 2042, Model.OrderBy, 2042, 14, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(">\r\n                            ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "ecda42bd7423cdcc2705839a8ad151589b3ffdce9692", async() => {
                    WriteLiteral("Top");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n                            ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "ecda42bd7423cdcc2705839a8ad151589b3ffdce10731", async() => {
                    WriteLiteral("Bottom");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n                        </select>\r\n                    </div>\r\n                    <div class=\"form-group\">\r\n                        <div class=\"custom-control custom-checkbox\">\r\n                            ");
#nullable restore
#line 42 "C:\Users\ThinkPro\Source\Repos\AQBooking_Solutions\src\Web\AQS.BookingAdmin\Views\PostCategory\_CreateUpdateModal.cshtml"
                       Write(Html.CheckBoxFor(x => x.IsActivated, new { @class = "custom-control-input", @required = "required" }));

#line default
#line hidden
#nullable disable
                WriteLiteral(@"
                            <label class=""custom-control-label"" for=""IsActivated"">Active</label>
                        </div>
                    </div>
                </div>
            </div>
            <div class=""modal-footer justify-content-between"">
                <button type=""button"" class=""btn btn-default"" data-dismiss=""modal"">Close</button>
                <button type=""submit"" class=""btn btn-primary"" id=""btnSavePostCate"">Save changes</button>
            </div>
        ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n    </div>\r\n    <!-- /.modal-content -->\r\n</div>\r\n<!-- /.modal-dialog -->");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public AQS.BookingAdmin.Interfaces.User.IWorkContext WorkContext { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<PostCategoriesCreateModel> Html { get; private set; }
    }
}
#pragma warning restore 1591