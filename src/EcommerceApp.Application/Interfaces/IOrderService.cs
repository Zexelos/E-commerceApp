using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using EcommerceApp.Application.ViewModels.Order;

namespace EcommerceApp.Application.Interfaces
{
    public interface IOrderService
    {
        Task AddOrderAsync(OrderCheckoutVM orderCheckoutVM);
        Task<OrderCheckoutVM> GetOrderCheckoutVMAsync(int cartId);
        Task<OrderListVM> GetPaginatedOrdersAsync(int pageSize, int pageNumber);
        Task<OrderDetailsVM> GetOrderDetailsAsync(int id);
        Task DeleteOrderAsync(int id);
    }
}
