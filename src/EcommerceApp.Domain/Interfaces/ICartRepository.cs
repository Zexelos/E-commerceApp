using System.Threading.Tasks;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Domain.Interfaces
{
    public interface ICartRepository
    {
        Task AddCartAsync(Cart cart);
        Task<int> GetCartIdAsync(int customerId);
    }
}