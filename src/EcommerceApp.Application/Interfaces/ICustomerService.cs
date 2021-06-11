using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels;

namespace EcommerceApp.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerVM> GetCustomerAsync(int id);
        Task<List<CustomerVM>> GetCustomersAsync();
        Task DeleteCustomerAsync(int id);
    }
}