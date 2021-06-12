using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.AdminPanel;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace EcommerceApp.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public EmployeeService(IMapper mapper, IEmployeeRepository repository, UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _repository = repository;
            _userManager = userManager;
        }

        public async Task AddEmployeeAsync(EmployeeVM employeeVM)
        {
            var employee = _mapper.Map<Employee>(employeeVM);
            var user = new AppUser { UserName = employeeVM.Email, Email = employeeVM.Email };
            employee.AppUserId = user.Id;
            await _userManager.CreateAsync(user, employeeVM.Password);
            await _repository.AddEmplyeeAsync(employee);
            var claim = new Claim("IsEmployee", "True");
            await _userManager.AddClaimAsync(user, claim);
            await _userManager.ConfirmEmailAsync(user, await _userManager.GenerateEmailConfirmationTokenAsync(user));
        }

        public async Task<EmployeeVM> GetEmployeeAsync(int id)
        {
            var employee = await _repository.GetEmployeeAsync(id);
            var employeeVM = _mapper.Map<EmployeeVM>(employee);
            var user = await _userManager.FindByIdAsync(employee.AppUserId);
            employeeVM.Email = user.Email;
            return employeeVM;
        }

        public async Task<EmployeeListVM> GetEmployeesAsync()
        {
            var employees = (await _repository.GetEmployeesAsync()).ToList();
            var employeeForListVMs = _mapper.Map<List<EmployeeForListVM>>(employees);
            for (int i = 0; i < employees.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(employees[i].AppUserId);
                employeeForListVMs[i].Email = user.Email;
            }
            return new EmployeeListVM
            {
                Employees = employeeForListVMs
            };
        }

        public async Task UpdateEmployeeAsync(EmployeeVM employeeVM)
        {
            var employee = await _repository.GetEmployeeAsync(employeeVM.Id);
            var user = await _userManager.FindByIdAsync(employee.AppUserId);
            await _userManager.SetEmailAsync(user, employeeVM.Email);
            await _userManager.UpdateNormalizedEmailAsync(user);
            await _userManager.SetUserNameAsync(user, employeeVM.Email);
            await _userManager.UpdateNormalizedUserNameAsync(user);
            if (employeeVM.Password != null)
            {
                await _userManager.ResetPasswordAsync(user, await _userManager.GeneratePasswordResetTokenAsync(user), employeeVM.Password);
            }
            var employeeMap = _mapper.Map<Employee>(employeeVM);
            employeeMap.AppUserId = employee.AppUserId;
            await _repository.UpdateEmployeeAsync(employeeMap);
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await _repository.GetEmployeeAsync(id);
            var user = await _userManager.FindByIdAsync(employee.AppUserId);
            await _userManager.DeleteAsync(user);
        }
    }
}