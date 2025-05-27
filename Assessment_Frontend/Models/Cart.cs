namespace Assessment_Frontend.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }
    }

    public class CartItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class CartResponse
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string? ImagePath { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime? DiscountStartDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }
        public int Quantity { get; set; }
        public decimal DiscountedPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class CartResponseWrapper
    {
        public List<CartResponse> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
