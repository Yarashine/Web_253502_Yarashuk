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



        // GET: api/Products
        //[HttpGet]
        //public async Task<ActionResult<ResponseData<List<Product>>>> GetProducts(string? category,int pageNo = 1,int pageSize = 3)
        //{
        //    return Ok(await _productService.GetProductListAsync(category, pageNo, pageSize));
        //}


        // GET: api/Products/5
        [HttpGet("id={id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
