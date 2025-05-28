using Assessment_Frontend.Models;

namespace Assessment_Frontend.Services
{
    public interface IApiService
    {
        Task<ProductResponse> GetProducts(string search, int page, int pageSize);
        Task<Product> GetProductById(int id);
        Task AddToCart(int productId);
        Task<List<CartResponse>> GetCart();
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product productDto);
        Task DeleteProductAsync(int id);
        Task DeleteCart(int productId);
    }
}
