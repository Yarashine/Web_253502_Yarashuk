using Web_253502_Yarashuk.Domain.Entities;
using Web_253502_Yarashuk.Domain.Models;

namespace Web_253502_Yarashuk.UI.Services.CategoryService;

public class MemoryCategoryService : ICategoryService
{
    public Task<ResponseData<List<Category>>> GetCategoryListAsync()
    {
        var categories = new List<Category>
        {
            new Category { Id = 1, Name = "Одежда", NormalizedName = "clothing" },
            new Category { Id = 2, Name = "Обувь", NormalizedName = "footwear" },
            new Category { Id = 3, Name = "Еда", NormalizedName = "food" }
        };
        var result = ResponseData<List<Category>>.Success(categories);
        return Task.FromResult(result);
    }
}

