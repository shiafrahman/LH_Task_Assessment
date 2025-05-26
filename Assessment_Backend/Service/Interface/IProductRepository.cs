using Assessment_Backend.Models;

namespace Assessment_Backend.Service.Interface
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync(string? search, int page, int pageSize);
        Task<int> GetTotalPages(string? search, int pageSize);
        Task<Product?> GetByIdAsync(int id);
        Task<Product> AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
    }
}
