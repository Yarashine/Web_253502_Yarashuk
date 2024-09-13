using Web_253502_Yarashuk.Domain.Entities;
using Web_253502_Yarashuk.Domain.Models;
using Web_253502_Yarashuk.UI.Services.CategoryService;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;

namespace Web_253502_Yarashuk.UI.Services.ProductService;

public class MemoryProductService : IProductService
{
    private List<Product> _Productes;
    private List<Category> _categories;
    private readonly int _itemsPerPage;

    public MemoryProductService(IConfiguration config, ICategoryService categoryService)
    {
        _categories = categoryService.GetCategoryListAsync().Result.Data;
        _itemsPerPage = int.Parse(config["ItemsPerPage"]);
        SetupData();
    }

    public Task<ResponseData<Product>> CreateProductAsync(Product product, IFormFile? formFile)
    {
        throw new NotImplementedException();
    }

    public Task DeleteProductAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<Product>> GetProductByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<ListModel<Product>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
    {
        var products = _Productes.AsQueryable();

        if (!string.IsNullOrEmpty(categoryNormalizedName))
        {
            products = products.Where(p => p.Category != null &&
                                            p.Category.NormalizedName.Equals(categoryNormalizedName, StringComparison.OrdinalIgnoreCase));
        }
        if (categoryNormalizedName == "Все")
            products = _Productes.AsQueryable();

        // Количество продуктов для текущей категории
        var totalItems = products.Count();
        var totalPages = (int)Math.Ceiling(totalItems / (double)_itemsPerPage);

        // Выбираем элементы для текущей страницы
        var items = products.Skip((pageNo - 1) * _itemsPerPage).Take(_itemsPerPage).ToList();

        var result = new ListModel<Product>
        {
            Items = items,
            CurrentPage = pageNo,
            TotalPages = totalPages
        };

        return Task.FromResult(ResponseData<ListModel<Product>>.Success(result));
    }



    public Task UpdateProductAsync(int id, Product product, IFormFile? formFile)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Инициализация списков
    /// </summary>
    private void SetupData()
    {
        _Productes = new List<Product>
        {
            // Продукты для категории "Одежда"
            new Product { Id = 1, Name = "Футболка", Description = "Комфортная хлопковая футболка", CategoryId = 1, ImagePath = "Images/TShirt.jfif", Price = 500 },
            new Product { Id = 2, Name = "Джинсы", Description = "Стильные джинсы, подходящие для повседневной носки", CategoryId = 1, ImagePath = "Images/Jeans.jfif", Price = 1200 },
            new Product { Id = 3, Name = "Куртка", Description = "Теплая куртка для зимнего сезона", CategoryId = 1, ImagePath = "Images/Jacket.jfif", Price = 2500 },
            new Product { Id = 4, Name = "Рубашка", Description = "Классическая рубашка из хлопка", CategoryId = 1, ImagePath = "Images/Shirt.jfif", Price = 700 },

            // Продукты для категории "Обувь"
            new Product { Id = 5, Name = "Кроссовки", Description = "Удобные кроссовки для бега", CategoryId = 2, ImagePath = "Images/Sneakers.jfif", Price = 3000 },
            new Product { Id = 6, Name = "Ботинки", Description = "Классические ботинки из натуральной кожи", CategoryId = 2, ImagePath = "Images/Boots.jfif", Price = 4000 },
            new Product { Id = 7, Name = "Сандалии", Description = "Легкие сандалии для летнего сезона", CategoryId = 2, ImagePath = "Images/Sandals.jfif", Price = 1500 },

            // Продукты для категории "Еда"
            new Product { Id = 8, Name = "Пицца", Description = "Вкусная пицца с томатным соусом", CategoryId = 3, ImagePath = "Images/Pizza.jfif", Price = 800 },
            new Product { Id = 9, Name = "Бургер", Description = "Сочный бургер с говядиной и свежими овощами", CategoryId = 3, ImagePath = "Images/Burger.jfif", Price = 600 },
            new Product { Id = 10, Name = "Салат Цезарь", Description = "Свежий салат с курицей и соусом Цезарь", CategoryId = 3, ImagePath = "Images/CaesarSalad.jfif", Price = 500 },

        };
        foreach (var product in _Productes)
        {
            product.Category = _categories.FirstOrDefault(c => c.Id == product.CategoryId);
        }
    }

}

