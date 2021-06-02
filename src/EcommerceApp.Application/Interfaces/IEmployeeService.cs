using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels;

namespace EcommerceApp.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task AddEmployeeAsync(EmployeeVM employee);
        Task<EmployeeVM> GetEmployeeAsync(int id);
        Task<List<EmployeeVM>> GetEmployeesAsync();
        Task UpdateEmployeeAsync(EmployeeVM employee);
        Task DeleteEmployeeAsync(int id);
    }
}