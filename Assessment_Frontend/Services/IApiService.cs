using Assessment_Frontend.Models;

namespace Assessment_Frontend.Services
{
    public interface IApiService
    {
        Task<ProductResponse> GetProducts(string search, int page, int pageSize);
        Task AddToCart(int productId);
        Task<List<CartResponse>> GetCart();
        Task AddProductAsync(Product product);
        Task DeleteCart(int productId);
    }
}
