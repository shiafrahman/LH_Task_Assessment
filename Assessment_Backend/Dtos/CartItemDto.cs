namespace Assessment_Backend.Dtos
{
    public class CartItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }


    public class CartResponseDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string? ImagePath { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public DateTime? DiscountStartDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }
        public int Quantity { get; set; }

        public decimal DiscountedPrice
        {
            get
            {
                var now = DateTime.Now;
                if (DiscountStartDate.HasValue && DiscountEndDate.HasValue &&
                    DiscountStartDate.Value <= now && now <= DiscountEndDate.Value)
                {
                    return Price * (1 - (DiscountPercentage ?? 0) / 100);
                }
                return Price;
            }
        }

        public decimal TotalPrice => DiscountedPrice * Quantity;
    }

}



