using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels.AdminPanel;

namespace EcommerceApp.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerVM> GetCustomerAsync(int id);
        Task<CustomerListVM> GetCustomersAsync();
        Task DeleteCustomerAsync(int id);
    }
}