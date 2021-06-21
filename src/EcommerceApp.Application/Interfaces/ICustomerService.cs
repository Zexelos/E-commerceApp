using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels.AdminPanel;

namespace EcommerceApp.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<int> GetCustomerIdByAppUserIdAsync(string appUserId);
        Task<CustomerDetailsVM> GetCustomerDetailsAsync(int id);
        Task<CustomerListVM> GetPaginatedCustomersAsync(int pageSize, int pageNumber);
        Task DeleteCustomerAsync(int id);
    }
}