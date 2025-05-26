using System.Text.Json.Serialization;

namespace Assessment_Frontend.Models
{
    public class Product
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public DateTime? DiscountStartDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }
        public IFormFile? Image { get; set; }
        public string ImagePath { get; set; }
    }

    public class ProductResponse
    {
        public List<Product> Products { get; set; }
        public int TotalPages { get; set; }
    }
}
