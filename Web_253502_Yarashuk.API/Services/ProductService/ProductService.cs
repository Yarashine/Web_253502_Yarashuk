using Microsoft.EntityFrameworkCore;
using Web_253502_Yarashuk.Data;
using Web_253502_Yarashuk.Domain.Entities;
using Web_253502_Yarashuk.Domain.Models;

namespace Web_253502_Yarashuk.API.Services.ProductService;

public class ProductService : IProductService
{
    private readonly AppDbContext _context; // Добавляем поле для контекста
    private readonly int _maxPageSize = 20;

    // Внедрение контекста через конструктор
    public ProductService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<ResponseData<Product>> CreateProductAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return new ResponseData<Product>
        {
            Data = product,
            Successfull = true
        };
    }

    public async Task DeleteProductAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
    public async Task<ResponseData<ListModel<Product>>> GetProductListAsync(string? categoryNormalizedName,int pageNo = 1, 
        int pageSize = 3)
    {
        if (pageSize > _maxPageSize)
            pageSize = _maxPageSize;
        var query = _context.Products.AsQueryable();
        var dataList = new ListModel<Product>();
        query = query.Where(d => categoryNormalizedName == null || d.Category.NormalizedName.Equals(categoryNormalizedName));
        // количество элементов в списке
        var count = await query.CountAsync(); //.Count();
        if (count == 0)
        {
            return ResponseData<ListModel<Product>>.Success(dataList);
        }
        // количество страниц
        int totalPages = (int)Math.Ceiling(count / (double)pageSize);
        if (pageNo > totalPages)
            return ResponseData<ListModel<Product>>.Error("No such page");
        dataList.Items = await query.OrderBy(d => d.Id).Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();
        dataList.CurrentPage = pageNo;
        dataList.TotalPages = totalPages;
        return ResponseData<ListModel<Product>>.Success(dataList);
    }

   

    public async Task<ResponseData<Product>> GetProductByIdAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            var res = new ResponseData<Product>
            {
                Data = null,
                Successfull = false,
                ErrorMessage = "Нет такого товара"
            };
            return res;
        }
        var result = new ResponseData<Product>
        {
            Data = product,
            Successfull = true
        };

        return result;
    }

    public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateProductAsync(int id, Product product)
    {
        var dbProduct = await _context.Products.FindAsync(id);
        if (dbProduct != null)
        {
            dbProduct.Name = product.Name;
            dbProduct.Description = product.Description;
            dbProduct.Price = product.Price;
            if (product.ImagePath is not null)
            {
                dbProduct.ImagePath = product.ImagePath;
            }

            dbProduct.CategoryId = product.CategoryId;
            _context.Update(dbProduct);
            await _context.SaveChangesAsync();
        }
    }
}
