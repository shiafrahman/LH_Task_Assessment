using Assessment_Backend.Data;
using Assessment_Backend.Models;
using Assessment_Backend.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace Assessment_Backend.Service.Repositories
{
    public class CartRepository:ICartRepository
    {
        private readonly ApplicationDbContext _context;
        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cart>> GetAllAsync() =>
            await _context.Carts.Include(c => c.Product).ToListAsync();

        public async Task<Cart?> GetByIdAsync(int id) =>
            await _context.Carts.Include(c => c.Product)
                                .FirstOrDefaultAsync(c => c.ProductId == id);

        public async Task<Cart?> GetByProductIdAsync(int productId) =>
        await _context.Carts.Include(c => c.Product)
                           .FirstOrDefaultAsync(c => c.ProductId == productId);

        public async Task AddAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();
            }
        }

    }
}
