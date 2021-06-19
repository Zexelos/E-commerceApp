using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels.AdminPanel;

namespace EcommerceApp.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerForListVM> GetCustomerAsync(int id);
        Task<int> GetCustomerIdByAppUserIdAsync(string appUserId);
        Task<CustomerListVM> GetCustomersAsync();
        Task<CustomerListVM> GetPaginatedCustomersAsync(int pageSize, int pageNumber);
        Task DeleteCustomerAsync(int id);
    }
}