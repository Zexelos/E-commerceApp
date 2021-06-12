using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels.AdminPanel;

namespace EcommerceApp.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task AddEmployeeAsync(EmployeeVM employeeVM);
        Task<EmployeeVM> GetEmployeeAsync(int id);
        Task<EmployeeListVM> GetEmployeesAsync();
        Task UpdateEmployeeAsync(EmployeeVM employeeVM);
        Task DeleteEmployeeAsync(int id);
    }
}