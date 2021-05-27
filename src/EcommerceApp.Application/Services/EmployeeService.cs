using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Model;

namespace EcommerceApp.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;

        public EmployeeService(IMapper mapper, IEmployeeRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task AddEmployeeAsync(EmployeeVM employee)
        {
            var employeeVM = _mapper.Map<Employee>(employee);
            await _repository.AddEmplyeeAsync(employeeVM);
        }

        public async Task<EmployeeVM> GetEmployeeAsync(int id)
        {
            var employee = await _repository.GetEmployeeAsync(id);
            return _mapper.Map<EmployeeVM>(employee);
        }

        public async Task<IQueryable<EmployeeVM>> GetEmployeesAsync()
        {
            var employees = await _repository.GetEmployeesAsync();
            return _mapper.Map<IQueryable<EmployeeVM>>(employees);
        }

        public async Task UpdateEmployeeAsync(EmployeeVM employeeVM)
        {
            var employee = _mapper.Map<Employee>(employeeVM);
            await _repository.UpdateEmployeeAsync(employee);
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            await _repository.DeleteEmployeeAsync(id);
        }
    }
}