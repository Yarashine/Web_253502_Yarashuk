namespace Web_253502_Yarashuk.UI.TagHelpers;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;

public class PagerTagHelper : TagHelper
{
    private readonly LinkGenerator _linkGenerator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PagerTagHelper(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
    {
        _linkGenerator = linkGenerator;
        _httpContextAccessor = httpContextAccessor;
    }

    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public string? Category { get; set; }
    public bool Admin { get; set; } = false;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "nav";
        output.Attributes.SetAttribute("aria-label", "Page navigation");

        var ul = new TagBuilder("ul");
        ul.AddCssClass("pagination");

        // Previous Button
        ul.InnerHtml.AppendHtml(CreatePageItem(CurrentPage - 1, "Previous", CurrentPage == 1, false));

        // Page Numbers
        for (int i = 1; i <= TotalPages; i++)
        {
            ul.InnerHtml.AppendHtml(CreatePageItem(i, i.ToString(), false, i == CurrentPage));
        }

        // Next Button
        ul.InnerHtml.AppendHtml(CreatePageItem(CurrentPage + 1, "Next", CurrentPage == TotalPages, false));

        output.Content.AppendHtml(ul);
    }

    private TagBuilder CreatePageItem(int pageNo, string text, bool isDisabled, bool isActive)
    {
        var li = new TagBuilder("li");
        li.AddCssClass("page-item");
        if (isDisabled) li.AddCssClass("disabled");
        if (isActive) li.AddCssClass("active");

        var link = new TagBuilder("a");
        link.AddCssClass("page-link");

        if (!isDisabled)
        {
            string? href = null;

            if (Admin)
            {
                // Генерация ссылки для страницы администратора
                href = _linkGenerator.GetPathByRouteValues(
                    httpContext: _httpContextAccessor.HttpContext,
                    routeName: null, // Используем стандартный маршрут
                    values: new { area = "Admin", pageNo }); // Указываем область и номер страницы
            }
            else
            {
                // Генерация ссылки для обычной страницы
                href = _linkGenerator.GetPathByPage(
                    httpContext: _httpContextAccessor.HttpContext,
                    page: null,
                    values: new { pageNo, category = Category });
            }

            link.Attributes["href"] = href ?? "#";
        }
        else
        {
            link.Attributes["href"] = "#";
        }

        link.InnerHtml.Append(text);
        li.InnerHtml.AppendHtml(link);
        return li;
    }
}
