using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Domain.Interfaces
{
    public interface IOrderItemRepository
    {
        Task AddOrderItemAsync(OrderItem orderItem);
        Task AddOrderItemsAsync(List<OrderItem> orderItems);
        Task<OrderItem> GetOrderItemAsync(int id);
        IQueryable<OrderItem> GetOrderItems();
        Task UpdateOrderItemAsync(OrderItem orderItem);
        Task DeleteOrderItemAsync(int id);
    }
}
