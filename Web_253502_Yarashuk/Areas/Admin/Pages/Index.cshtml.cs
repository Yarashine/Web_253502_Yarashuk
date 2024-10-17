using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_253502_Yarashuk.Domain.Entities;
using Web_253502_Yarashuk.Domain.Models;
using Web_253502_Yarashuk.UI.Services.ProductService;


namespace Web_253502_Yarashuk.UI.Areas.Admin.Pages;

public class IndexModel : PageModel
{
    private readonly IProductService _productService;

    public IndexModel(IProductService productService)
    {
        _productService = productService;
    }

    public ListModel<Web_253502_Yarashuk.Domain.Entities.Product> Products { get; set; } = new ListModel<Domain.Entities.Product>();

    public async Task OnGetAsync(int pageNo = 1)
    {
        var productsResponse = await _productService.GetProductListAsync(null, pageNo);
        if (productsResponse.Successfull)
        {
            Products = productsResponse.Data!;
        }
    }
}
