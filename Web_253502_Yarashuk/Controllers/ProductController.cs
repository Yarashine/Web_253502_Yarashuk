﻿namespace Web_253502_Yarashuk.UI.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web_253502_Yarashuk.UI.Services.CategoryService;
using Web_253502_Yarashuk.UI.Services.ProductService;

public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public ProductController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index(string? category, int pageNo = 1)
    {
        var categories = await _categoryService.GetCategoryListAsync();

        var productsResponse = await _productService.GetProductListAsync(category, pageNo);

        if (!productsResponse.Successfull)
        {
            return View("Error");
        }

        ViewData["currentCategory"] = categories.Data.FirstOrDefault(c => c.NormalizedName == category)?.Name ?? "Все";
        ViewData["curCategory"] = category ?? "Все";
        ViewBag.Categories = categories.Data;

        return View(productsResponse.Data);
    }

}