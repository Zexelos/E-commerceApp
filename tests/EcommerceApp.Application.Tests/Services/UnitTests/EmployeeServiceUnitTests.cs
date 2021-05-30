using System.Threading;
using System.Linq;
using System.Reflection;
using System;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceApp.Application.Mapping;
using EcommerceApp.Application.Services;
using EcommerceApp.Application.ViewModels;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Model;
using Moq;
using Xunit;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace EcommerceApp.Application.Tests
{
    public class EmployeeServiceUnitTests
    {
        private readonly EmployeeService _sut;
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<IEmployeeRepository> _employeeRepository = new();
        private readonly Mock<UserManager<AppUser>> _userManager;

        public EmployeeServiceUnitTests()
        {
            var userStore = new Mock<IUserStore<AppUser>>();
            _userManager = new Mock<UserManager<AppUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            _sut = new EmployeeService(_mapper.Object, _employeeRepository.Object, _userManager.Object);
        }

        [Fact]
        public async Task AddEmployee_MapperShouldReturnEmployee()
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

            // Act
            await _sut.AddEmployeeAsync(employeeVM);

            // Assert 
            _userManager.Verify(s => s.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()), Times.Once);
            _employeeRepository.Verify(s => s.AddEmplyeeAsync(It.IsAny<Employee>()), Times.Once);
            Assert.Equal(employee.FirstName, employeeVM.FirstName);
            Assert.Equal(employee.LastName, employeeVM.LastName);
        }

        [Fact]
        public async Task GetEmployeeAsync_ReturnEMployeeVM()
        {
            // Arrange
            Employee employee = new()
            {
                Id = 10,
                FirstName = "maciek",
                LastName = "makowski"
            };

            EmployeeVM employeeVM = new()
            {
                Id = 10,
                FirstName = "maciek",
                LastName = "makowski"
            };

            _employeeRepository.Setup(s => s.GetEmployeeAsync(employee.Id)).ReturnsAsync(employee);

            _mapper.Setup(s => s.Map<EmployeeVM>(employee)).Returns(employeeVM);

            // Act
            var result = await _sut.GetEmployeeAsync(employee.Id);

            // Assert
            Assert.Equal(employee.Id, result.Id);
            Assert.Equal(employee.FirstName, result.FirstName);
            Assert.Equal(employee.LastName, result.LastName);
        }

        [Fact]
        public async Task GetEmployeesAsync_ReturnListOfEmployeeVM()
        {
            // Arrange
            List<Employee> employees = new()
            {
                new Employee() { FirstName = "maciek", LastName = "makowski" },
                new Employee() { FirstName = "swe9guhv", LastName = "s34c63w4" },
            };

            List<EmployeeVM> employeesVM = new()
            {
                new EmployeeVM() { FirstName = "maciek", LastName = "makowski" },
                new EmployeeVM() { FirstName = "swe9guhv", LastName = "s34c63w4" },
            };

            _employeeRepository.Setup(s => s.GetEmployeesAsync()).ReturnsAsync(employees.AsQueryable);

            _mapper.Setup(s => s.Map<List<EmployeeVM>>(employees)).Returns(employeesVM);

            // Act
            var result = await _sut.GetEmployeesAsync();

            // Assert
            Assert.Equal(result[0].FirstName, employeesVM[0].FirstName);
            Assert.Equal(result[1].LastName, employeesVM[1].LastName);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_ShouldUpdateEEmployee()
        {
            // Arrange
            Employee employee = new()
            {
                Id = 10,
                FirstName = "maciek",
                LastName = "makowski",
                AppUserId = "w23qrx2q3",
            };

            EmployeeVM employeeVM = new()
            {
                Id = 10,
                FirstName = "maciek",
                LastName = "makowski",
                Password = "asfew"
            };

            EmployeeVM employeeVM2 = new()
            {
                Id = 10,
                FirstName = "maciek",
                LastName = "makowski",
                Password = null
            };

            AppUser user = new()
            {
                Id = "w23qrx2q3"
            };

            _employeeRepository.Setup(s => s.GetEmployeeAsync(employeeVM.Id)).ReturnsAsync(employee);

            _userManager.Setup(s => s.FindByIdAsync(employee.AppUserId)).ReturnsAsync(new AppUser()
            {
                Id = "w23qrx2q3"
            });

            _mapper.Setup(s => s.Map<Employee>(employeeVM)).Returns(employee);

            // Act
            await _sut.UpdateEmployeeAsync(employeeVM);

            // Assert
            Assert.Equal(employee.Id, employeeVM.Id);
            Assert.Equal(employee.AppUserId, user.Id);
            _userManager.Verify(v => v.SetEmailAsync(It.IsAny<AppUser>(), It.IsAny<string>()), Times.Once);
            _userManager.Verify(v => v.UpdateNormalizedEmailAsync(It.IsAny<AppUser>()), Times.Once);
            _userManager.Verify(v => v.SetUserNameAsync(It.IsAny<AppUser>(), It.IsAny<string>()), Times.Once);
            _userManager.Verify(v => v.UpdateNormalizedUserNameAsync(It.IsAny<AppUser>()), Times.Once);
            _userManager.Verify(v => v.ResetPasswordAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task CheckUpdateEmployee()
        {
            // Arrange
            EmployeeVM employeeVM = new()
            {
                Id = 10,
                FirstName = "maciek",
                LastName = "makowski",
                Password = "asfew"
            };

            Employee employee = new()
            {
                Id = 10,
                FirstName = "maciek",
                LastName = "makowski",
                AppUserId = "w23qrx2q3",
            };

            AppUser user = new()
            {
                Id = "w23qrx2q3"
            };

            _employeeRepository.Setup(s => s.GetEmployeeAsync(employeeVM.Id)).ReturnsAsync(employee);

            _userManager.Setup(s => s.FindByIdAsync(employee.AppUserId)).ReturnsAsync(user);

            // Act
            await _sut.DeleteEmployeeAsync(employeeVM.Id);

            // Assert
            Assert.Equal(user.Id, employee.AppUserId);
            _userManager.Verify(v => v.DeleteAsync(It.IsAny<AppUser>()), Times.Once);
            _employeeRepository.Verify(v => v.DeleteEmployeeAsync(It.IsAny<int>()), Times.Once);
        }
    }
}
