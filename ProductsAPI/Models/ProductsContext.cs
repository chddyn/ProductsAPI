using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProductsAPI.Models
{
    public class ProductsContext : IdentityDbContext<AppUser,AppRole, int>
    {

        public ProductsContext(DbContextOptions<ProductsContext> options) : base(options)
        {

        }

        // overreide onmodel yazınca otomatik atıyor
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().HasData(new Product() { productId = 1, productName = "IPhone 14", Price = 40000, IsActive = true });
            modelBuilder.Entity<Product>().HasData(new Product() { productId = 2, productName = "IPhone 15", Price = 50000, IsActive = false });
            modelBuilder.Entity<Product>().HasData(new Product() { productId = 3, productName = "IPhone 16", Price = 60000, IsActive = true });
            modelBuilder.Entity<Product>().HasData(new Product() { productId = 4, productName = "IPhone 17", Price = 70000, IsActive = false });
            modelBuilder.Entity<Product>().HasData(new Product() { productId = 5, productName = "IPhone 18", Price = 80000, IsActive = true });
        }

        public DbSet<Product> Products { get; set; }
    }
}
