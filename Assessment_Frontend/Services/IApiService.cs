using Assessment_Frontend.Models;

namespace Assessment_Frontend.Services
{
    public interface IApiService
    {
       // Task<List<ProductResponse>> GetProducts(string search, int page, int pageSize);
        Task<ProductResponse> GetProducts(string search, int page, int pageSize);
        Task AddToCart(int productId);
        Task<List<Cart>> GetCart();
        Task AddProductAsync(Product product);
    }
}
