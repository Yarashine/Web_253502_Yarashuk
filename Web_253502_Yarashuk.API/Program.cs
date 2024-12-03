
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Web_253502_Yarashuk.API.Data;
using Web_253502_Yarashuk.API.Services.CategoryService;
using Web_253502_Yarashuk.API.Services.ProductService;
using Web_253502_Yarashuk.API.Models;
using Web_253502_Yarashuk.Data;
using Web_253502_Yarashuk.Domain.Entities;
using static System.Net.WebRequestMethods;

namespace Web_253502_Yarashuk.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.



            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IProductService, ProductService>();

            var connectionString = builder.Configuration.GetConnectionString("Default");

            // Регистрация контекста базы данных
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connectionString));

            var authServer = builder.Configuration.GetSection("AuthServer").Get<AuthServerData>();
            //var a = $"{authServer.Host}/realms/{authServer.Realm}/.well-known/openid-configuration";

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
                {
                    // Адрес метаданных конфигурации OpenID
                    o.MetadataAddress = $"{authServer.Host}/realms/{authServer.Realm}/.well-known/openid-configuration";

                    // Authority сервера аутентификации
                    o.Authority = $"http://localhost:8080/realms/Yarashuk";
                    // Audience для токена JWT
                    o.Audience = "account";
                    // Запретить HTTPS для использования локальной версии Keycloak
                    // В рабочем проекте должно быть true
                    o.RequireHttpsMetadata = false;
                });

            builder.Services.AddAuthorization(opt =>
            {
                opt.AddPolicy("admin", p => p.RequireRole("POWER-USER"));
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    policy => policy.WithOrigins("https://localhost:7193")
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
            });

            var app = builder.Build();

            app.UseCors("AllowSpecificOrigin");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            //DbInitializer.SeedData(app);

            app.MapControllers();

            app.Run();


        }
    }
}
