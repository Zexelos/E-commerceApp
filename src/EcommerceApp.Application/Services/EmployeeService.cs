using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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

        public async Task AddEmployeeAsync(EmployeeVM employeeVM)
        {
            var employee = _mapper.Map<Employee>(employeeVM);
            await _repository.AddEmplyeeAsync(employee);
        }

        public async Task<EmployeeVM> GetEmployeeAsync(int id)
        {
            var employee = await _repository.GetEmployeeAsync(id);
            return _mapper.Map<EmployeeVM>(employee);
        }

        public async Task<List<EmployeeVM>> GetEmployeesAsync()
        {
            var employees = (await _repository.GetEmployeesAsync()).ToList();
            return _mapper.Map<List<EmployeeVM>>(employees);
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