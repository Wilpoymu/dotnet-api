using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using softsolutions.Models;

namespace softsolutions.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        }
        
        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Provider> Provider { get; set; }
        public DbSet<ProductProvider> ProductProvider { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Ensure Identity-related entities are configured

            modelBuilder.Entity<Category>().HasData(new Category { Id = 1, Name = "Category 1" });
            modelBuilder.Entity<Product>().HasData(new Product { Id = 1, Name = "Product 1", Price = 100, Description = "Description 1", CategoryId = 1 });
            modelBuilder.Entity<Provider>().HasData(new Provider { Id = 1, Name = "Provider 1", Address = "Address 1", Phone = "Phone 1", Email = "Email 1" });
            
            modelBuilder.Entity<Product>()
                .HasMany<Provider>(p => p.Providers)
                .WithMany(p => p.Products)
                .UsingEntity<ProductProvider>();

            modelBuilder.Entity<ProductProvider>().HasData(new ProductProvider { ProductId = 1, ProviderId = 1 });
        }
    }
}