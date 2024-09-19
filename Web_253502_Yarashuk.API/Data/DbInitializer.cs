using Web_253502_Yarashuk.Data;
using Web_253502_Yarashuk.Domain.Entities;
using Web_253502_Yarashuk.API.Services.CategoryService;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Web_253502_Yarashuk.API.Data;

public static class DbInitializer
{
    public static async Task SeedData(WebApplication app)
    {
        // Получение контекста БД
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        // Выполнение миграций
        await context.Database.MigrateAsync();
        // Получение конфигурации
        var configuration = app.Configuration;
        var baseUrl = configuration["BaseUrl"];
        var imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");

        // Создание папки для изображений, если ее нет
        if (!Directory.Exists(imagesPath))
        {
            Directory.CreateDirectory(imagesPath);
        }

        var categories = new List<Category>
            {
                new Category {Name = "Одежда", NormalizedName = "clothing" },
                new Category {Name = "Обувь", NormalizedName = "footwear" },
                new Category {Name = "Еда", NormalizedName = "food" }
            };

        if (!context.Categories.Any())
        {
            categories = new List<Category>
            {
                new Category {Name = "Одежда", NormalizedName = "clothing" },
                new Category {Name = "Обувь", NormalizedName = "footwear" },
                new Category {Name = "Еда", NormalizedName = "food" }
            };
            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();
        }


        // Заполнение данных
        if (!context.Products.Any())
        {
            var httpClient = new HttpClient();

            // Получение списка категорий
            //var categoryService = scope.ServiceProvider.GetRequiredService<ICategoryService>();
            //categories = (await categoryService.GetCategoryListAsync()).Data;

            var products = new List<Product>
            {
                // Продукты для категории "Одежда"

                new Product { Name = "TShirt", Description = "Comfortable cotton t-shirt", CategoryId = 1, Price = 500 },
                new Product { Name = "Jeans", Description = "Fashionable jeans for everyday wear", CategoryId = 1, Price = 1200 },
                new Product { Name = "Jacket", Description = "Warm jacket for winter season", CategoryId = 1, Price = 2500 },
                new Product { Name = "Shirt", Description = "Classic shirt made of cotton", CategoryId = 1, Price = 700 },

                // Products for the "Shoes" category
                new Product { Name = "Sneakers", Description = "Comfortable sneakers for running", CategoryId = 2, Price = 3000 },
                new Product { Name = "Boots", Description = "Classic boots made of natural leather", CategoryId = 2, Price = 4000 },
                new Product { Name = "Sandals", Description = "Light sandals for summer season", CategoryId = 2, Price = 1500 },

                // Products for the "Food" category
                new Product { Name = "Pizza", Description = "Tasty pizza with tomato sauce", CategoryId = 3, Price = 800 },
                new Product { Name = "Burger", Description = "Juicy burger with beef and fresh vegetables", CategoryId = 3, Price = 600 },
                new Product { Name = "CaesarSalad", Description = "Fresh salad with chicken and Caesar sauce", CategoryId = 3, Price = 500 },
            };

            foreach (var product in products)
            {
                // Сохранение изображений
                var imageFileName = $"{product.Name}.jfif";
                var imagePath = Path.Combine(imagesPath, imageFileName);


                // Установка пути изображения
                product.ImagePath = $"Images/{imageFileName}";
                product.Category = categories.FirstOrDefault(c => c.Id == product.CategoryId);
            }

            context.Products.AddRange(products);
            await context.SaveChangesAsync();
        }
    }
}