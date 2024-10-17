using Web_253502_Yarashuk.UI.Services.CategoryService;
using Web_253502_Yarashuk.UI.Services.FileService;
using Web_253502_Yarashuk.UI.Services.ProductService;
using Web_253502_Yarashuk.UI.Settings;

namespace Web_253502_Yarashuk.UI.Extensions;

public static class HostingExtensions
{
    public static void RegisterCustomServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ICategoryService, ApiCategoryService>();
        builder.Services.AddScoped<IProductService, ApiProductService>();

        var UriData = builder.Configuration.GetSection("UriData").Get<UriData>();

        builder.Services.AddHttpClient<IFileService, ApiFileService>(opt =>
            opt.BaseAddress = new Uri($"{UriData.ApiUri}Files"));

    }
}
