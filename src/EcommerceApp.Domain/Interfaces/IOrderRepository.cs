using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task AddOrderAsync(Order order);
        IQueryable<Order> GetOrders();
        Task DeleteOrderAsync(int id);
    }
}
