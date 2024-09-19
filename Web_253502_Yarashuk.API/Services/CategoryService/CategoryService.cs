using Web_253502_Yarashuk.Data;
using Web_253502_Yarashuk.Domain.Entities;
using Web_253502_Yarashuk.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Web_253502_Yarashuk.API.Services.CategoryService;

public class CategoryService : ICategoryService
{
     
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    // Реализация метода GetCategoryListAsync
    public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
    {
        var categories = await _context.Categories.ToListAsync();

        if (categories == null || !categories.Any())
        {
            return ResponseData<List<Category>>.Error("No categories");
        }

        return ResponseData<List<Category>>.Success(categories);
    }
}

