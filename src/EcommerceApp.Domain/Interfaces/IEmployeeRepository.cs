using EcommerceApp.Domain.Models;
using System.Threading.Tasks;
using System.Linq;

namespace EcommerceApp.Domain.Interfaces
{
    public interface IEmployeeRepository
    {
        Task AddEmplyeeAsync(Employee employee);

        Task<Employee> GetEmployeeAsync(int id);

        Task<IQueryable<Employee>> GetEmployeesAsync();

        Task UpdateEmployeeAsync(Employee employee);

        Task DeleteEmployeeAsync(int id);
    }
}