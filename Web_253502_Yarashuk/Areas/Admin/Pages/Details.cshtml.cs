using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_253502_Yarashuk.Domain.Entities;
using Web_253502_Yarashuk.UI.Services.ProductService;


namespace Web_253502_Yarashuk.UI.Areas.Admin.Pages;

public class DetailsModel : PageModel
{
    private readonly IProductService _productService;

    public DetailsModel(IProductService productService)
    {
        _productService = productService;
    }

    public Product Product { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _productService.GetProductByIdAsync(id.Value);
        product = null;
        if (product == null)
        {
            return NotFound();
        }
        else
        {
            Product = product.Data;
        }

        return Page();
    }
}

