using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading;
using System.Linq;
using System.Reflection;
using System;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceApp.Application.Services;
using EcommerceApp.Application.ViewModels.AdminPanel;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Moq;
using Xunit;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using EcommerceApp.Application.Interfaces;
using MockQueryable.Moq;
using EcommerceApp.Application.ViewModels;

namespace EcommerceApp.Application.Tests
{
    public class EmployeeServiceUnitTests
    {

        private readonly EmployeeService _sut;
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<IEmployeeRepository> _employeeRepository = new();
        private readonly Mock<UserManager<AppUser>> _userManager;
        private readonly Mock<IPasswordHasher<AppUser>> _passwordHasher = new();
        private readonly Mock<IPaginatorService<EmployeeForListVM>> _paginatorService = new();

        public EmployeeServiceUnitTests()
        {
            var userStore = new Mock<IUserStore<AppUser>>();
            _userManager = new Mock<UserManager<AppUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            _sut = new EmployeeService(
                _mapper.Object,
                _employeeRepository.Object,
                _userManager.Object,
                _paginatorService.Object,
                _passwordHasher.Object);
        }

        [Fact]
        public async Task AddEmployeeAsync_MethodsRunOnce()
        {
            // Arrange
            var employeeVM = new EmployeeVM()
            {
                FirstName = "maciek",
                LastName = "makowski"
            };

            var employee = new Employee()
            {
                FirstName = "maciek",
                LastName = "makowski"
            };

            _mapper.Setup(s => s.Map<Employee>(employeeVM)).Returns(employee);

            _userManager.Setup(s => s.GenerateEmailConfirmationTokenAsync(It.IsAny<AppUser>())).ReturnsAsync("escg45egsv5egrs");

            // Act
            await _sut.AddEmployeeAsync(employeeVM);

            // Assert 
            _userManager.Verify(s => s.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()), Times.Once);
            _userManager.Verify(s => s.AddClaimAsync(It.IsAny<AppUser>(), It.IsAny<Claim>()), Times.Once);
            _userManager.Verify(s => s.ConfirmEmailAsync(It.IsAny<AppUser>(), It.IsAny<string>()));
        }

        [Fact]
        public async Task GetEmployeeAsync_ReturnsEmployeeVM()
        {
            // Arrange
            var employee = new Employee
            {
                Id = 10,
                FirstName = "maciek",
                LastName = "makowski",
                AppUser = new AppUser
                {
                    Email = "maciek@masd.com"
                }
            };
            var employees = new List<Employee> { employee };
            var employeesQ = employees.AsQueryable().BuildMock();

            EmployeeVM employeeVM = new()
            {
                Id = 10,
                FirstName = "maciek",
                LastName = "makowski",
                Email = "maciek@masd.com"
            };

            _employeeRepository.Setup(s => s.GetEmployees()).Returns(employeesQ.Object);

            _mapper.Setup(x => x.ConfigurationProvider).Returns(
                () => new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Employee, EmployeeVM>().ForMember(x => x.Email, x => x.MapFrom(src => src.AppUser.Email));
                }));

            // Act
            var result = await _sut.GetEmployeeAsync(employee.Id);

            // Assert
            Assert.Equal(employeeVM.Id, result.Id);
            Assert.Equal(employeeVM.FirstName, result.FirstName);
            Assert.Equal(employeeVM.LastName, result.LastName);
            Assert.Equal(employeeVM.Email, result.Email);
        }

        [Fact]
        public async Task GetPaginatedEmployeesAsync_ReturnsEmployeeListVM()
        {
            // Arrange
            List<Employee> employees = new()
            {
                new Employee() { FirstName = "maciek", LastName = "makowski" },
                new Employee() { FirstName = "swe9guhv", LastName = "s34c63w4" },
            };
            var employeesQ = employees.AsQueryable().BuildMock();

            List<EmployeeForListVM> employeesVM = new()
            {
                new EmployeeForListVM() { FirstName = "maciek", LastName = "makowski" },
                new EmployeeForListVM() { FirstName = "swe9guhv", LastName = "s34c63w4" },
            };

            var paginatedVM = new PaginatedVM<EmployeeForListVM>
            {
                Items = employeesVM,
                CurrentPage = 1,
                TotalPages = 2,
            };

            var employeeListVM = new EmployeeListVM
            {
                Employees = paginatedVM.Items,
                CurrentPage = paginatedVM.CurrentPage,
                TotalPages = paginatedVM.TotalPages
            };

            _employeeRepository.Setup(s => s.GetEmployees()).Returns(employeesQ.Object);

            _mapper.Setup(x => x.ConfigurationProvider).Returns(
                () => new MapperConfiguration(cfg => { cfg.CreateMap<Employee, EmployeeForListVM>(); }));

            _paginatorService.Setup(x => x.CreateAsync(It.IsAny<IQueryable<EmployeeForListVM>>(), 1, 10)).ReturnsAsync(paginatedVM);

            _mapper.Setup(x => x.Map<EmployeeListVM>(It.IsAny<PaginatedVM<EmployeeForListVM>>())).Returns(employeeListVM);

            // Act
            var result = await _sut.GetPaginatedEmployeesAsync(10, 1);

            // Assert
            Assert.Equal(employeeListVM, result);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_MethodsRunOnce()
        {
            // Arrange
            Employee employee = new()
            {
                Id = 10,
                FirstName = "maciek",
                LastName = "makowski",
                AppUser = new AppUser
                {
                    Email = "ziutek@askd.com",
                    UserName = "ziutek",
                    PasswordHash = "w34ct234ctq234ct"
                }
            };
            var employees = new List<Employee> { employee };
            var employeesQ = employees.AsQueryable().BuildMock();

            EmployeeVM employeeVM = new()
            {
                Id = 10,
                FirstName = "maciek",
                LastName = "makowski",
                Password = "asfew",
                Email = "ziutek@askd.com"
            };

            _employeeRepository.Setup(s => s.GetEmployees()).Returns(employeesQ.Object);

            _passwordHasher.Setup(x => x.HashPassword(It.IsAny<AppUser>(), It.IsAny<string>())).Returns(employee.AppUser.PasswordHash);

            // Act
            await _sut.UpdateEmployeeAsync(employeeVM);

            // Assert
            _employeeRepository.Verify(x => x.UpdateEmployeeAsync(It.IsAny<Employee>()), Times.Once);
        }

        [Fact]
        public async Task DeleteEmployeeAsync_MethodsRunOnce()
        {
            // Arrange
            Employee employee = new()
            {
                Id = 10,
                FirstName = "maciek",
                LastName = "makowski",
                AppUser = new AppUser
                {
                    Email = "ziutek@askd.com",
                    UserName = "ziutek",
                    PasswordHash = "w34ct234ctq234ct"
                }
            };
            var employees = new List<Employee> { employee };
            var employeesQ = employees.AsQueryable().BuildMock();

            _employeeRepository.Setup(s => s.GetEmployees()).Returns(employeesQ.Object);

            // Act
            await _sut.DeleteEmployeeAsync(employee.Id);

            // Assert
            _userManager.Verify(x => x.DeleteAsync(It.IsAny<AppUser>()), Times.Once);
        }
    }
}
