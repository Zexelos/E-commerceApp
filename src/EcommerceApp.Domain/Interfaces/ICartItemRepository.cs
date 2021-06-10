using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Domain.Interfaces
{
    public interface ICartItemRepository
    {
        Task AddCartItemAsync(CartItem cartItem);
        Task<CartItem> GetCartItemAsync(int id);
        Task<IQueryable<CartItem>> GetCartItemsAsync();
        Task UpdateCartItemAsync(CartItem cartItem);
        Task DeleteCartItemAsync(int id);
    }
}
