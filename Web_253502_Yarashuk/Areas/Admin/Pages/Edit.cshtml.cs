using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_253502_Yarashuk.Domain.Entities;
using Web_253502_Yarashuk.Domain.Models;
using Web_253502_Yarashuk.UI.Services.ProductService;
using Web_253502_Yarashuk.UI.Services.CategoryService;
namespace Web_253502_Yarashuk.UI.Areas.Admin.Pages;


public class EditModel : PageModel
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public EditModel(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    [BindProperty]
    public Product Product { get; set; } = default!;

    [BindProperty]
    public IFormFile? Image { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _productService.GetProductByIdAsync(id.Value);
        if (product == null)
        {
            return NotFound();
        }

        Product = product.Data;
        var categories = await _categoryService.GetCategoryListAsync();
        ViewData["CategoryId"] = new SelectList(categories.Data, "Id", "Name");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        var product = await _productService.GetProductByIdAsync(Product.Id);
        Product.ImagePath = product.Data.ImagePath;
        await _productService.UpdateProductAsync(Product, Image);

        return RedirectToPage("./Index");
    }

    private async Task<bool> ProductExists(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        return product == null;
    }
}
