using System.Threading.Tasks;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Domain.Interfaces
{
    public interface ICartRepository
    {
        Task AddCartAsync(Cart cart);
        Task<Cart> GetCartAsync(int id);
        Task<int> GetCartIdAsync(int customerId);
    }
}
