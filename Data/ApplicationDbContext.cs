using Microsoft.EntityFrameworkCore;
using train_distributed_catching.Models;

namespace train_distributed_catching.Data
{
    public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Product> Product { get; set; } = null!;
    }
}
