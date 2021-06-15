using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(Order order);
        Task<Order> GetOrderAsync(int id);
        Task<IQueryable<Order>> GetOrdersAsync();
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
    }
}
