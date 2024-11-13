using MarriageAgency.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;

namespace MarriageAgency.TagHelpers
{
    public class PageLinkTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory urlHelperFactory;

        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public PageViewModel PageModel { get; set; }
        public string PageAction { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>();

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            output.TagName = "nav";

            // Создаем ul для пагинации
            TagBuilder tag = new("ul");
            tag.AddCssClass("pagination");

            // Кнопка "Prev"
            if (PageModel.HasPreviousPage)
            {
                TagBuilder prevItem = new("li");
                prevItem.AddCssClass("page-item");
                TagBuilder prevLink = new("a");
                prevLink.AddCssClass("page-link");
                PageUrlValues["page"] = PageModel.PageNumber - 1;
                prevLink.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
                prevLink.InnerHtml.Append("Prev");
                prevItem.InnerHtml.AppendHtml(prevLink);
                tag.InnerHtml.AppendHtml(prevItem);
            }

            // Ссылка на первую страницу
            if (PageModel.PageNumber > 1)
            {
                TagBuilder firstItem = CreateTag(1, urlHelper);
                firstItem.AddCssClass("page-item");
                tag.InnerHtml.AppendHtml(firstItem);
            }

            // Ссылка на предыдущую страницу, если она есть
            if (PageModel.HasPreviousPage)
            {
                TagBuilder prevItem = CreateTag(PageModel.PageNumber - 1, urlHelper);
                prevItem.AddCssClass("page-item");
                tag.InnerHtml.AppendHtml(prevItem);
            }

            // Текущая страница
            TagBuilder currentItem = CreateTag(PageModel.PageNumber, urlHelper);
            currentItem.AddCssClass("page-item active");
            tag.InnerHtml.AppendHtml(currentItem);

            // Ссылка на следующую страницу, если она есть
            if (PageModel.HasNextPage)
            {
                TagBuilder nextItem = CreateTag(PageModel.PageNumber + 1, urlHelper);
                nextItem.AddCssClass("page-item");
                tag.InnerHtml.AppendHtml(nextItem);
            }

            // Ссылка на последнюю страницу
            if (PageModel.PageNumber < PageModel.TotalPages)
            {
                TagBuilder lastItem = CreateTag(PageModel.TotalPages, urlHelper);
                lastItem.AddCssClass("page-item");
                tag.InnerHtml.AppendHtml(lastItem);
            }

            // Кнопка "Next"
            if (PageModel.HasNextPage)
            {
                TagBuilder nextItem = new("li");
                nextItem.AddCssClass("page-item");
                TagBuilder nextLink = new("a");
                nextLink.AddCssClass("page-link");
                PageUrlValues["page"] = PageModel.PageNumber + 1;
                nextLink.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
                nextLink.InnerHtml.Append("Next");
                nextItem.InnerHtml.AppendHtml(nextLink);
                tag.InnerHtml.AppendHtml(nextItem);
            }

            output.Content.AppendHtml(tag);
        }

        TagBuilder CreateTag(int pageNumber, IUrlHelper urlHelper)
        {
            TagBuilder item = new("li");
            item.AddCssClass("page-item");
            TagBuilder link = new("a");
            link.AddCssClass("page-link");

            if (pageNumber == this.PageModel.PageNumber)
            {
                link.Attributes["href"] = "#";
                link.Attributes["aria-current"] = "page";
                link.InnerHtml.Append(pageNumber.ToString());
            }
            else
            {
                PageUrlValues["page"] = pageNumber;
                link.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
                link.InnerHtml.Append(pageNumber.ToString());
            }

            item.InnerHtml.AppendHtml(link);
            return item;
        }
    }
}
