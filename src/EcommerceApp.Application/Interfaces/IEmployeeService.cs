using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels;

namespace EcommerceApp.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task AddEmployeeAsync(EmployeeVM employee);
        Task<EmployeeVM> GetEmployeeAsync(int id);
        Task<IQueryable<EmployeeVM>> GetEmployeesAsync();
        Task UpdateEmployeeAsync(EmployeeVM employee);
        Task DeleteEmployeeAsync(int id);
    }
}