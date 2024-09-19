
using Microsoft.EntityFrameworkCore;
using Web_253502_Yarashuk.API.Data;
using Web_253502_Yarashuk.API.Services.CategoryService;
using Web_253502_Yarashuk.API.Services.ProductService;
using Web_253502_Yarashuk.Data;

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




            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthorization();

            DbInitializer.SeedData(app);

            app.MapControllers();

            app.Run();
        }
    }
}
