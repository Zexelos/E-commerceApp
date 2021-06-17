using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Domain.Interfaces
{
    public interface ICartItemRepository
    {
        Task AddCartItemAsync(CartItem cartItem);
        Task<CartItem> GetCartItemAsync(int id);
        IQueryable<CartItem> GetCartItems();
        IQueryable<CartItem> GetCartItemsByCartId(int cartId);
        Task UpdateCartItemAsync(CartItem cartItem);
        Task DeleteCartItemAsync(int id);
        Task DeleteCartItemsByCartIdAsync(int cartId);
    }
}
