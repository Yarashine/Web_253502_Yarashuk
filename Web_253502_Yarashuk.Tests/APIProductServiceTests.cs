using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Web_253502_Yarashuk.API.Services.ProductService;
using Web_253502_Yarashuk.Data;
using Web_253502_Yarashuk.Domain.Entities;
using Web_253502_Yarashuk.Domain.Models;

namespace Web_253502_Yarashuk.Tests
{
    public class ProductServiceTests
    {
        private AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite("DataSource=:memory:") // Используем SQLite in-memory
                .Options;

            var context = new AppDbContext(options);
            context.Database.OpenConnection(); // Открываем соединение
            context.Database.EnsureCreated(); // Создаем схему базы данных
            return context;
        }

        [Fact]
        public void ServiceReturnsFirstPageOfThreeItems()
        {
            // Arrange
            using var context = CreateContext();
            var service = new ProductService(context);

            // Добавляем категорию один раз
            var category = new Category { Name = "category1", NormalizedName = "category1" };

            // Добавляем продукты с одной категорией
            context.Products.AddRange(
                new Product { Name = "Product1", Description = "Description1", Category = category },
                new Product { Name = "Product2", Description = "Description2", Category = category },
                new Product { Name = "Product3", Description = "Description3", Category = category },
                new Product { Name = "Product4", Description = "Description4", Category = category }
            );

            // Сохраняем изменения
            context.SaveChanges();

            // Act
            var result = service.GetProductListAsync(null).Result;

            // Assert
            Assert.IsType<ResponseData<ListModel<Product>>>(result);
            Assert.True(result.Successfull);
            Assert.Equal(1, result.Data.CurrentPage);
            Assert.Equal(3, result.Data.Items.Count);
            Assert.Equal(2, result.Data.TotalPages);
            Assert.Equal("Product1", result.Data.Items[0].Name);
        }


        [Fact]
        public void ServiceReturnsCorrectPageNumber()
        {
            // Arrange
            using var context = CreateContext();
            var service = new ProductService(context);

            var category = new Category { Name = "category1", NormalizedName = "category1" };

            // Добавляем продукты с одной категорией
            context.Products.AddRange(
                new Product { Name = "Product1", Description = "Description1", Category = category },
                new Product { Name = "Product2", Description = "Description2", Category = category },
                new Product { Name = "Product3", Description = "Description3", Category = category },
                new Product { Name = "Product4", Description = "Description4", Category = category }
            );
            context.SaveChanges();

            // Act
            var result = service.GetProductListAsync(null, 2).Result;

            // Assert
            Assert.IsType<ResponseData<ListModel<Product>>>(result);
            Assert.Equal(2, result.Data.CurrentPage);
            Assert.Equal(1, result.Data.Items.Count);
            Assert.Equal("Product4", result.Data.Items[0].Name);
        }

        [Fact]
        public void ServiceFiltersByCategory()
        {
            // Arrange
            using var context = CreateContext();
            var service = new ProductService(context);

            var category = new Category { Name = "category1", NormalizedName = "category1" };
            var category2 = new Category { Name = "category2", NormalizedName = "category2" };

            // Добавляем продукты с одной категорией
            context.Products.AddRange(
                new Product { Name = "Product1", Description = "Description1", Category = category },
                new Product { Name = "Product2", Description = "Description2", Category = category },
                new Product { Name = "Product3", Description = "Description3", Category = category2 },
                new Product { Name = "Product4", Description = "Description4", Category = category2 }
            );
            context.SaveChanges();

            // Act
            var result = service.GetProductListAsync("category1").Result;

            // Assert
            Assert.IsType<ResponseData<ListModel<Product>>>(result);
            Assert.Equal(2, result.Data.Items.Count);
            Assert.Equal("Product1", result.Data.Items[0].Name);
            Assert.Equal("Product2", result.Data.Items[1].Name);
        }

        [Fact]
        public void ServiceDoesNotAllowPageSizeGreaterThanMax()
        {
            // Arrange
            using var context = CreateContext();
            var service = new ProductService(context);

            var category = new Category { Name = "category1", NormalizedName = "category1" };


            // Добавляем тестовые данные с обязательными полями
            context.Products.AddRange(
                new Product { Name = "Product1", Description = "Description1", Category = category },
                new Product { Name = "Product2", Description = "Description2", Category = category },
                new Product { Name = "Product3", Description = "Description3", Category = category },
                new Product { Name = "Product4", Description = "Description4", Category = category },
                new Product { Name = "Product5", Description = "Description1", Category = category },
                new Product { Name = "Product6", Description = "Description2", Category = category },
                new Product { Name = "Product7", Description = "Description3", Category = category },
                new Product { Name = "Product8", Description = "Description4", Category = category },
                new Product { Name = "Product9", Description = "Description1", Category = category },
                new Product { Name = "Product10", Description = "Description2", Category = category },
                new Product { Name = "Product11", Description = "Description3", Category = category },
                new Product { Name = "Product12", Description = "Description4", Category = category },
                new Product { Name = "Product13", Description = "Description1", Category = category },
                new Product { Name = "Product14", Description = "Description2", Category = category },
                new Product { Name = "Product15", Description = "Description3", Category = category },
                new Product { Name = "Product16", Description = "Description4", Category = category }, 
                new Product { Name = "Product17", Description = "Description1", Category = category },
                new Product { Name = "Product18", Description = "Description2", Category = category },
                new Product { Name = "Product19", Description = "Description3", Category = category },
                new Product { Name = "Product20", Description = "Description4", Category = category },
                new Product { Name = "Product21", Description = "Description1", Category = category },
                new Product { Name = "Product22", Description = "Description2", Category = category },
                new Product { Name = "Product23", Description = "Description3", Category = category },
                new Product { Name = "Product24", Description = "Description4", Category = category }
            );
            context.SaveChanges();

            // Act
            var result = service.GetProductListAsync(null, 1, 50).Result; // Страница больше максимального размера

            // Assert
            Assert.IsType<ResponseData<ListModel<Product>>>(result);
            Assert.Equal(20, result.Data.Items.Count); // Три — это максимальное количество элементов на странице
        }

        [Fact]
        public void ServiceReturnsErrorWhenPageExceedsTotalPages()
        {
            // Arrange
            using var context = CreateContext();
            var service = new ProductService(context);

            // Добавляем тестовые данные с обязательными полями
            var category = new Category { Name = "category1", NormalizedName = "category1" };

            // Добавляем продукты с одной категорией
            context.Products.AddRange(
                new Product { Name = "Product1", Description = "Description1", Category = category },
                new Product { Name = "Product2", Description = "Description2", Category = category }
            );
            context.SaveChanges();

            // Act
            var result = service.GetProductListAsync(null, 3).Result; // Страница больше общего количества страниц

            // Assert
            Assert.IsType<ResponseData<ListModel<Product>>>(result);
            Assert.False(result.Successfull);
            Assert.Equal("No such page", result.ErrorMessage);
        }
    }
}
