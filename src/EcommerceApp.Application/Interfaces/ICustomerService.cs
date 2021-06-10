using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<Customer> GetCustomerAsync(int id);
        Task<List<Customer>> GetCustomersAsync();
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(int id);
    }
}