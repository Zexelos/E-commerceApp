using System;
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
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

namespace EcommerceApp.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IPaginatorService<EmployeeForListVM> _paginatorService;

        public EmployeeService(
            IMapper mapper,
            IEmployeeRepository repository,
            UserManager<AppUser> userManager,
            IPaginatorService<EmployeeForListVM> paginatorService)
        {
            _mapper = mapper;
            _repository = repository;
            _userManager = userManager;
            _paginatorService = paginatorService;
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
            return await _repository.GetEmployees()
                .Where(x => x.Id == id)
                .Include(a => a.AppUser)
                .ProjectTo<EmployeeVM>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<EmployeeListVM> GetPaginatedEmployeesAsync(int pageSize, int pageNumber)
        {
            var employeesQuery = _repository.GetEmployees()
                .Include(a => a.AppUser)
                .ProjectTo<EmployeeForListVM>(_mapper.ConfigurationProvider);
            var paginatedVM = await _paginatorService.CreateAsync(employeesQuery, pageNumber, pageSize);
            return new EmployeeListVM
            {
                Employees = paginatedVM.Items,
                CurrentPage = paginatedVM.CurrentPage,
                TotalPages = paginatedVM.TotalPages
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