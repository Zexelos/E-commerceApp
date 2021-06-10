using System;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task AddCustomerAsync(Customer customer);
        Task<Customer> GetCustomerAsync(int id);
        Task<IQueryable<Customer>> GetCustomersAsync();
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(int id);
    }
}
