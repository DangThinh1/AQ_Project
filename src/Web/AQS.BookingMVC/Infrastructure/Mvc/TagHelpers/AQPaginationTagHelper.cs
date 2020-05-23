using AQS.BookingMVC.Infrastructure.AQPagination;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Web;

namespace AQS.BookingMVC.Infrastructure.Mvc.TagHelpers
{
    [HtmlTargetElement("aq-pagination")]
    public class AQPaginationTagHelper : TagHelper
    {
        public PaginatedModel Info { get; set; }
        public string PrevPageText { get; set; } = "<";
        public string NextPageText { get; set; } = ">";
        public string Url { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Method { get; set; } = "POST";
        public object Model { get; set; }
        public string ParamName { get; set; }
        public string UpdateTargetId { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            BuildParent(output);
            AddPrevPage(output);
            AddPageNode(output);
            AddNextPage(output);
        }

        private static void BuildParent(TagHelperOutput output)
        {
            output.TagName = "ul";
            output.Attributes.Add("class", "pagination justify-content-center");
            output.Attributes.Add("id", "aesPagination");
        }

        private void AddPrevPage(TagHelperOutput output)
        {
            if (Info.Pages.Count > 1)
            {
                var html = string.Empty;
                if (Info.PrevPage.Display)
                {
                    html = $@"<li class=""page-item prev"" id=""btnPrev""><button data-model=""{HttpUtility.HtmlEncode(Model)}"" data-page=""{Info.PrevPage.PageNumber}"" data-link=""{Controller}/{Action}"" data-url=""{Url}"" data-param=""{ParamName}"" update-target-id=""{UpdateTargetId}"" method=""{Method}"" class=""page-link button"" >&lt;</button></li>";
                    output.Content.SetHtmlContent(output.Content.GetContent() + html);
                }
                else
                {
                    html = $@"<li class=""page-item prev"" id=""btnPrev""><button class=""page-link button"" disabled>&lt;</button></li>";
                    output.Content.SetHtmlContent(output.Content.GetContent() + html);
                }
            }
        }

        private void AddNextPage(TagHelperOutput output)
        {
            if (Info.Pages.Count > 1)
            {
                var html = string.Empty;
                if (Info.NextPage.Display)
                {
                    html = $@"<li class=""page-item next"" id=""btnNext""><button data-model=""{HttpUtility.HtmlEncode(Model)}"" data-page=""{Info.NextPage.PageNumber}"" data-link=""{Controller}/{Action}"" data-url=""{Url}"" data-param=""{ParamName}"" update-target-id=""{UpdateTargetId}"" method=""{Method}"" class=""page-link button"">&gt;</button></li>";
                    output.Content.SetHtmlContent(output.Content.GetContent() + html);
                }
                else
                {
                    html = $@"<li class=""page-item next"" id=""btnNext""><button class=""page-link button"" disabled>&gt;</button></li>";
                    output.Content.SetHtmlContent(output.Content.GetContent() + html);
                }
            }
        }

        private void AddPageNode(TagHelperOutput output)
        {
            if (Info.Pages.Count > 1)
            {
                foreach (var infoPage in Info.Pages)
                {
                    string html;
                    if (infoPage.IsCurrent)
                    {
                        html = $@"<li class=""page-item""><button data-model=""{HttpUtility.HtmlEncode(Model)}"" data-page=""{infoPage.PageNumber}"" data-link=""{Controller}/{Action}"" data-url=""{Url}"" data-param=""{ParamName}"" data-active=""True"" update-target-id=""{UpdateTargetId}"" method=""{Method}"" class=""page-link button"" style=""font-weight: bold"">{infoPage.PageNumber}</button></li>";
                        output.Content.SetHtmlContent(output.Content.GetContent() + html);
                        continue;
                    }
                    html = $@"<li class=""page-item""><button data-model=""{HttpUtility.HtmlEncode(Model)}"" data-page=""{infoPage.PageNumber}"" data-link=""{Controller}/{Action}"" data-url=""{Url}"" data-param=""{ParamName}"" update-target-id=""{UpdateTargetId}"" method=""{Method}"" class=""page-link button"">{infoPage.PageNumber}</button></li>";
                    output.Content.SetHtmlContent(output.Content.GetContent() + html);
                }
            }
        }
    }
}
