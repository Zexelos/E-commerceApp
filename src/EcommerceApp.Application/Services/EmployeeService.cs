using System.Net.Mime;
using System.Linq;
using System.Security.Claims;
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
        private readonly IPasswordHasher<AppUser> _passwordHasher;
        private readonly IPaginatorService<EmployeeForListVM> _paginatorService;

        public EmployeeService(
            IMapper mapper,
            IEmployeeRepository repository,
            UserManager<AppUser> userManager,
            IPaginatorService<EmployeeForListVM> paginatorService,
            IPasswordHasher<AppUser> passwordHasher)
        {
            _mapper = mapper;
            _repository = repository;
            _userManager = userManager;
            _paginatorService = paginatorService;
            _passwordHasher = passwordHasher;
        }

        public async Task AddEmployeeAsync(EmployeeVM employeeVM)
        {
            var employee = _mapper.Map<Employee>(employeeVM);
            var user = new AppUser { UserName = employeeVM.Email, Email = employeeVM.Email, Employee = employee };
            await _userManager.CreateAsync(user, employeeVM.Password);
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
            //var employeeVM = _mapper.Map<EmployeeVM>(employee);
            //var user = await _userManager.FindByIdAsync(employee.AppUserId);
            //employeeVM.Email = employee.AppUser.Email;
            //return employeeVM;
        }

        public async Task<EmployeeListVM> GetPaginatedEmployeesAsync(int pageSize, int pageNumber)
        {
            var employeesQuery = _repository.GetEmployees()
                .Include(a => a.AppUser)
                .ProjectTo<EmployeeForListVM>(_mapper.ConfigurationProvider);
            var paginatedVM = await _paginatorService.CreateAsync(employeesQuery, pageNumber, pageSize);
            return _mapper.Map<EmployeeListVM>(paginatedVM);
        }

        public async Task UpdateEmployeeAsync(EmployeeVM employeeVM)
        {

            var test = await _repository.GetEmployees()
                .Where(x => x.Id == employeeVM.Id)
                    .Include(a => a.AppUser)
            .FirstOrDefaultAsync();
            test.FirstName = employeeVM.FirstName;
            test.LastName = employeeVM.LastName;
            test.Position = employeeVM.Position;
            test.AppUser.UserName = employeeVM.Email;
            test.AppUser.NormalizedUserName = employeeVM.Email.ToUpper();
            test.AppUser.Email = employeeVM.Email;
            test.AppUser.NormalizedEmail = employeeVM.Email.ToUpper();
            if (employeeVM.Password != null)
            {
                test.AppUser.PasswordHash = _passwordHasher.HashPassword(test.AppUser, employeeVM.Password);
            }
            await _repository.UpdateEmployeeAsync(test);
            /*
            var employee = await _repository.GetEmployeeAsync(employeeVM.Id);
            var user = await _userManager.FindByIdAsync(employee.AppUserId);
            await _userManager.SetEmailAsync(user, employeeVM.Email);
            await _userManager.UpdateNormalizedEmailAsync(user);
            await _userManager.SetUserNameAsync(user, employeeVM.Email);
            await _userManager.UpdateNormalizedUserNameAsync(user);
            if (employeeVM.Password != null)
            {
                var tokenPassword = await _userManager.GeneratePasswordResetTokenAsync(user);
                await _userManager.ResetPasswordAsync(user, tokenPassword, employeeVM.Password);
            }
            var employeeMap = _mapper.Map<Employee>(employeeVM);
            employeeMap.AppUserId = employee.AppUserId;
            await _repository.UpdateEmployeeAsync(employeeMap);
            */
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var test = await _repository.GetEmployees().Where(x => x.Id == id).Include(a => a.AppUser).FirstOrDefaultAsync();
            //var employee = await _repository.GetEmployeeAsync(id);
            //var user = await _userManager.FindByIdAsync(employee.AppUserId);
            await _userManager.DeleteAsync(test.AppUser);
        }
    }
}