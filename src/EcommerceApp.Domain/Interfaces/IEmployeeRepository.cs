using EcommerceApp.Domain.Model;
using System.Threading.Tasks;
using System.Linq;

namespace EcommerceApp.Domain.Interfaces
{
    public interface IEmployeeRepository
    {
        Task AddEmplyee(Employee employee);

        Task<Employee> GetEmployee(int id);

        Task<IQueryable<Employee>> GetEmployees();

        Task UpdateEmployee(int id);

        Task DeleteEmployee(int id);
    }
}