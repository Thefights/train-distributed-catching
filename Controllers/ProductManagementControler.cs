using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using train_distributed_catching.Data;
using train_distributed_catching.Models;

namespace train_distributed_catching.Controllers
{
    [Route("product-management")]
    [ApiController]
    public class ProductManagementControler (ApplicationDbContext _dbContext) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _dbContext.Product.ToListAsync();
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            await _dbContext.Product.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            _dbContext.Product.Update(product);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _dbContext.Product.FindAsync(id);
            _dbContext.Product.Remove(product);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
