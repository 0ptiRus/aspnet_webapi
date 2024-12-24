using Microsoft.EntityFrameworkCore;

namespace _1812_webapi.Entity
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions options) : base(options)
        {
        }

        public ProductDbContext() : base()
        {
        }

        public DbSet<Product> Products { get; set; }


    }
}
