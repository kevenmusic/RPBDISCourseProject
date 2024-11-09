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
            output.TagName = "nav"; // Добавляем тег nav для навигации

            // Создаем ul для пагинации
            TagBuilder tag = new("ul");
            tag.AddCssClass("pagination");

            // Создаем ссылку на предыдущую страницу, если она есть
            if (PageModel.HasPreviousPage)
            {
                TagBuilder prevItem = CreateTag(PageModel.PageNumber - 1, urlHelper);
                prevItem.AddCssClass("page-item"); // Добавляем класс для элемента списка
                tag.InnerHtml.AppendHtml(prevItem);
            }

            // Создаем текущую страницу
            TagBuilder currentItem = CreateTag(PageModel.PageNumber, urlHelper);
            currentItem.AddCssClass("page-item active"); // Добавляем класс active для текущей страницы
            tag.InnerHtml.AppendHtml(currentItem);

            // Создаем ссылку на следующую страницу, если она есть
            if (PageModel.HasNextPage)
            {
                TagBuilder nextItem = CreateTag(PageModel.PageNumber + 1, urlHelper);
                nextItem.AddCssClass("page-item"); // Добавляем класс для элемента списка
                tag.InnerHtml.AppendHtml(nextItem);
            }

            output.Content.AppendHtml(tag);
        }

        TagBuilder CreateTag(int pageNumber, IUrlHelper urlHelper)
        {
            TagBuilder item = new("li");
            item.AddCssClass("page-item"); // Добавляем класс для элемента списка
            TagBuilder link = new("a");
            link.AddCssClass("page-link"); // Класс для ссылки

            if (pageNumber == this.PageModel.PageNumber)
            {
                link.Attributes["href"] = "#"; // Текущая страница не должна быть ссылкой
                link.Attributes["aria-current"] = "page"; // Указываем, что это текущая страница
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
