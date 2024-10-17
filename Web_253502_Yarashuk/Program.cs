using Web_253502_Yarashuk.UI.Extensions;
using Web_253502_Yarashuk.UI.Services.ProductService;
using Web_253502_Yarashuk.UI.Services.CategoryService;
using Web_253502_Yarashuk.UI.Settings;

namespace Web_253502_Yarashuk;
//проблема в том что у меня создаются в юд 2 типа файлов, ззачем это нужно

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllersWithViews();

        builder.Services.AddRazorPages();
        builder.RegisterCustomServices();

        var UriData = builder.Configuration.GetSection("UriData").Get<UriData>();

        builder.Services.AddHttpClient<IProductService, ApiProductService>(opt =>
            opt.BaseAddress = new Uri(UriData.ApiUri));

        builder.Services.AddHttpClient<ICategoryService, ApiCategoryService>(opt =>
            opt.BaseAddress = new Uri(UriData.ApiUri));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Product}/{action=Index}/{category?}");

    


        app.MapRazorPages();

        app.Run();
    }
}
