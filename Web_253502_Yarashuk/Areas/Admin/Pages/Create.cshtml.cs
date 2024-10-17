using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_253502_Yarashuk.Domain.Entities;
using Web_253502_Yarashuk.UI.Services.ProductService;
using Web_253502_Yarashuk.UI.Services.CategoryService;

namespace Web_253502_Yarashuk.UI.Areas.Admin.Pages;

public class CreateModel : PageModel
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public CreateModel(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> OnGet()
    {
        var categories = await _categoryService.GetCategoryListAsync();
        ViewData["CategoryId"] = new SelectList(categories.Data, "Id", "Name");
        return Page();
    }

    [BindProperty]
    public Product Product { get; set; } = default!;

    [BindProperty]
    public IFormFile Image { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await _productService.CreateProductAsync(Product, Image);
        return RedirectToPage("./Index");
    }
}

