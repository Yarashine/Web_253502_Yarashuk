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

    public Task<ResponseData<Product>> CreateProductAsync(Product product)
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

    public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
    {
        throw new NotImplementedException();
    }

    public Task UpdateProductAsync(int id, Product product)
    {
        throw new NotImplementedException();
    }
}
