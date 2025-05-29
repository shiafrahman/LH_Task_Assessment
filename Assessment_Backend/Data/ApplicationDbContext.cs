using Assessment_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Assessment_Backend.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 12,
                    Name = "Nike Free Flyknit",
                    Description = "A lightweight running shoe with a breathable Flyknit upper and flexible Free sole for natural movement.",
                    Price = 120.00M,
                    DiscountPercentage = 10.00M,
                    DiscountStartDate = new DateTime(2025, 5, 30),
                    DiscountEndDate = new DateTime(2025, 6, 6),
                    ImagePath = "/ProductImages/domino-studio-164_6wVEHfI-unsplash.jpg"
                },
                new Product
                {
                    Id = 13,
                    Name = "Ray-Ban Classic",
                    Description = "Classic black sunglasses with polarized lenses, offering UV protection and timeless style.",
                    Price = 150.00M,
                    DiscountPercentage = 15.00M,
                    DiscountStartDate = new DateTime(2025, 5, 31),
                    DiscountEndDate = new DateTime(2025, 6, 6),
                    ImagePath = "/ProductImages/giorgio-trovato-K62u25Jk6vo-unsplash.jpg"
                },
                new Product
                {
                    Id = 14,
                    Name = "Michael Kors Chronograph Watch",
                    Description = "A sleek chronograph watch with a stainless steel bracelet, dark green dial, and Roman numeral markers.",
                    Price = 250.00M,
                    DiscountPercentage = 20.00M,
                    DiscountStartDate = new DateTime(2025, 5, 30),
                    DiscountEndDate = new DateTime(2025, 6, 6),
                    ImagePath = "/ProductImages/pexels-javon-swaby-197616-2783873.jpg"
                },
                new Product
                {
                    Id = 15,
                    Name = "Pink Lipstick and Blush Set",
                    Description = "A vibrant pink lipstick paired with a matching blush compact, ideal for a coordinated makeup look.",
                    Price = 35.00M,
                    DiscountPercentage = 10.00M,
                    DiscountStartDate = new DateTime(2025, 5, 30),
                    DiscountEndDate = new DateTime(2025, 6, 6),
                    ImagePath = "/ProductImages/pexels-suzyhazelwood-2533266.jpg"
                },
                new Product
                {
                    Id = 16,
                    Name = "Fujifilm X-T10 with XF 18-55mm Lens",
                    Description = "A compact mirrorless camera with a 16.3MP sensor, retro design, and a versatile 18-55mm f/2.8-4 lens for high-quality photography.",
                    Price = 800.00M,
                    DiscountPercentage = 15.00M,
                    DiscountStartDate = new DateTime(2025, 5, 29),
                    DiscountEndDate = new DateTime(2025, 6, 6),
                    ImagePath = "/ProductImages/pexels-madebymath-90946.jpg"
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
