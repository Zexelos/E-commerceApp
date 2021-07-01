using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task<int> GetCustomerIdAsync(string AppUserId);
        IQueryable<Customer> GetCustomers();
        Task UpdateCustomerAsync(Customer customer);
    }
}
