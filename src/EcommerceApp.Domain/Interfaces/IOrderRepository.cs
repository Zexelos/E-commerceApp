using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Domain.Interfaces
{
    public interface IOrderRepository
    {
        IQueryable<Order> GetOrders();
        Task DeleteOrderAsync(int id);
    }
}
