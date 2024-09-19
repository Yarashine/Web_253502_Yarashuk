using Web_253502_Yarashuk.UI.Services.CategoryService;
using Web_253502_Yarashuk.UI.Services.ProductService;

namespace Web_253502_Yarashuk.UI.Extensions;

public static class HostingExtensions
{
    public static void RegisterCustomServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ICategoryService, ApiCategoryService>();
        builder.Services.AddScoped<IProductService, ApiProductService>();

    }
}
