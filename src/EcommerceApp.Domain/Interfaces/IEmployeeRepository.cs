using EcommerceApp.Domain.Models;
using System.Threading.Tasks;
using System.Linq;

namespace EcommerceApp.Domain.Interfaces
{
    public interface IEmployeeRepository
    {
        IQueryable<Employee> GetEmployees();

        Task UpdateEmployeeAsync(Employee employee);
    }
}