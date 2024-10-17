using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_253502_Yarashuk.API.Services.ProductService;
using Web_253502_Yarashuk.Data;
using Web_253502_Yarashuk.Domain.Entities;
using Web_253502_Yarashuk.Domain.Models;

namespace Web_253502_Yarashuk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly IProductService _productService; // Определение зависимости от сервиса продуктов

        public ProductsController(AppDbContext context, IProductService productService) // Внедрение зависимости
        {
            _productService = productService;
            _context = context;
        }

        [HttpGet("{category?}")]
        //[HttpGet]
        public async Task<ActionResult<ResponseData<List<Product>>>> GetProducts([FromRoute] string? category = null, [FromQuery] int pageNo = 1,
            [FromQuery] int pageSize = 3)
        {
            // Проверяем, если категория указана как "Все", то обнуляем значение
            if (string.Equals(category, "Все", StringComparison.OrdinalIgnoreCase))
                category = null;

            // Получаем список продуктов через сервис
            var products = await _productService.GetProductListAsync(category, pageNo, pageSize);

            return Ok(products);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            await _productService.UpdateProductAsync(id, product);
            return Ok();
        }

        [HttpGet("id-{id}")]
        public async Task<ActionResult<Product>> GetProduct([FromRoute] int id)
        {
            var productResponse = await _productService.GetProductByIdAsync(id);
            return Ok(productResponse);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            return Ok(await _productService.CreateProductAsync(product));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProductAsync(id);
            return Ok();
        }
    }
}
