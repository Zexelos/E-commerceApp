using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels.Order;

namespace EcommerceApp.Application.Interfaces
{
    public interface IOrderService
    {
        Task AddOrderAsync(OrderCheckoutVM orderCheckoutVM);
        Task<OrderCheckoutVM> GetOrderCheckoutVMAsync(int cartId);
    }
}
