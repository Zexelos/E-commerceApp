using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels.AdminPanel;

namespace EcommerceApp.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task AddEmployeeAsync(EmployeeVM employeeVM);
        Task<EmployeeVM> GetEmployeeAsync(int id);
        Task<EmployeeListVM> GetPaginatedEmployeesAsync(int pageSize, int pageNumber);
        Task UpdateEmployeeAsync(EmployeeVM employeeVM);
        Task DeleteEmployeeAsync(int id);
    }
}