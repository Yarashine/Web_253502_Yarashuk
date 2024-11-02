using Web_253502_Yarashuk.UI.Extensions;
using Web_253502_Yarashuk.UI.Services.ProductService;
using Web_253502_Yarashuk.UI.Services.CategoryService;
using Web_253502_Yarashuk.UI.Settings;
using Web_253502_Yarashuk.UI.HelperClasses;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Web_253502_Yarashuk.UI.Services.Authentication;
using Web_253502_Yarashuk.UI.Services.Authorization;

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

        builder.Services
            .Configure<KeycloakData>(builder.Configuration.GetSection("Keycloak"));

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddHttpClient<ITokenAccessor, KeycloakTokenAccessor>();
        builder.Services.AddHttpClient<IAuthService, KeycloakAuthService>();
        var keycloakData =
        builder.Configuration.GetSection("Keycloak").Get<KeycloakData>();
        builder.Services
        .AddAuthentication(options => 
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
        .AddCookie()
        .AddJwtBearer()
        .AddOpenIdConnect(options =>
        {
            options.Authority = $"{keycloakData.Host}/auth/realms/{keycloakData.Realm}";
            options.ClientId = keycloakData.ClientId;
            options.ClientSecret = keycloakData.ClientSecret;
            options.ResponseType = OpenIdConnectResponseType.Code;
            options.Scope.Add("openid"); // Customize scopes as needed
            options.SaveTokens = true;
            options.RequireHttpsMetadata = false; // позволяет обращаться к     локальному Keycloak по http
            options.MetadataAddress = $"{keycloakData.Host}/realms/{keycloakData.Realm}/.well-known/openid-configuration";
        });



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
