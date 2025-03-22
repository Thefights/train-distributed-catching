using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using train_distributed_catching.Data;
using train_distributed_catching.Models;

namespace train_distributed_catching.Controllers
{
    [Route("product-management")]
    [ApiController]
    public class ProductManagementControler(ApplicationDbContext _dbContext, IDistributedCache _cache) : ControllerBase
    {
        private const string cacheKey = "productList";

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var cacheData = await _cache.GetStringAsync(cacheKey);

            if (cacheData != null)
            {
                var cacheProducts = JsonSerializer.Deserialize<List<Product>>(cacheData);
                return Ok(new { source = "cache", data = cacheProducts });
            }

            var productsFromDb = await _dbContext.Product.ToListAsync();

            var serializedProducts = JsonSerializer.Serialize(productsFromDb);
            await _cache.SetStringAsync(cacheKey, serializedProducts);

            return Ok(new { source = "database", data = productsFromDb });
        }

        [HttpGet("noCache")]
        public async Task<IActionResult> GetProductsNoDb()
        {
           var products =  await _dbContext.Product.ToListAsync();
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            await _dbContext.Product.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            await _cache.RemoveAsync(cacheKey);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            _dbContext.Product.Update(product);
            await _dbContext.SaveChangesAsync();
            await _cache.RemoveAsync(cacheKey);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _dbContext.Product.FindAsync(id);
            _dbContext.Product.Remove(product);
            await _dbContext.SaveChangesAsync();
            await _cache.RemoveAsync(cacheKey);
            return Ok();
        }
    }
}
