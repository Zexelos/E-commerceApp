using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;

        public CartRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddCartAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Cart> GetCarts()
        {
            return _context.Carts.AsQueryable();
        }
    }
}
