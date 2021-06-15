using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Domain.Interfaces
{
    public interface IOrderItemRepository
    {
        Task AddOrderItemAsync(OrderItem orderItem);
        Task<OrderItem> GetOrderItemAsync(int id);
        Task<IQueryable<OrderItem>> GetOrderItemsAsync();
        Task UpdateOrderItemAsync(OrderItem orderItem);
        Task DeleteOrderItemAsync(int id);
    }
}
