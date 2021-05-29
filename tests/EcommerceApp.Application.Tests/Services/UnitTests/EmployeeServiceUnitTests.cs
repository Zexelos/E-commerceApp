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
        public Mock<UserManager<AppUser>> GetMockUserManager()
        {
            var mockUserStore = new Mock<IUserStore<AppUser>>();
            return new Mock<UserManager<AppUser>>(mockUserStore.Object, null, null, null, null, null, null, null, null);
        }

        [Fact]
        public async Task CheckAddEmployee()
        {
            var employee = new Employee();

            var employeeVMe = new EmployeeVM();

            var employeeVM = new EmployeeVM
            {
                Id = 100,
                FirstName = "Zordon",
                LastName = "Rasista",
                Position = "edhsrth",
                Email = "maniel@pajac.com",
                Password = "Pa$$w0rd!"
            };

            var user = new AppUser { UserName = employeeVM.Email, Email = employeeVM.Email };

            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));

            var mapper = config.CreateMapper();

            var mock = new Mock<IEmployeeRepository>();

            var mock2 = GetMockUserManager();

            mock.SetupAsync(s => s.AddEmplyeeAsync(It.IsAny<Employee>())).Callback<Employee>((p) =>
            {
                employee = p;
            });

            mock2.Setup(m => m.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>())).Callback<AppUser, string>((a, s) =>
             {
                 employeeVMe.Email = a.Email;
                 employeeVMe.Password = s;
             });

            var service = new EmployeeService(mapper, mock.Object, mock2.Object);

            await service.AddEmployeeAsync(employeeVM);

            mock.Verify(x => x.AddEmplyeeAsync(It.IsAny<Employee>()), Times.Once());
            mock2.Verify(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()), Times.Once());

            Assert.Equal(employee.FirstName, employeeVM.FirstName);
            Assert.Equal(employee.LastName, employeeVM.LastName);
            Assert.Equal(employee.Position, employeeVM.Position);
            Assert.Equal(employeeVMe.Email, employeeVM.Email);
            Assert.Equal(employeeVMe.Password, employeeVM.Password);
        }

        [Fact]
        public async Task CheckGetEmployee()
        {
            var employee = new Employee
            {
                Id = 100,
                FirstName = "adssad",
                LastName = "sadsad",
                Position = "edhsrsadasdth"
            };

            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));

            var mapper = config.CreateMapper();

            var mock = new Mock<IEmployeeRepository>();

            var mock2 = GetMockUserManager();

            mock.SetupAsync(s => s.GetEmployeeAsync(It.IsAny<int>())).Returns(employee);

            var service = new EmployeeService(mapper, mock.Object, mock2.Object);

            var result = await service.GetEmployeeAsync(employee.Id);

            Assert.Equal(employee.FirstName, result.FirstName);
        }

        [Fact]
        public async Task CheckEmployeesExistence()
        {
            var employee1 = new Employee { Id = 100, FirstName = "adssad", LastName = "sadsad", Position = "edhsrsadasdth" };
            var employee2 = new Employee { Id = 150, FirstName = "sadfsd", LastName = "o7hl9", Position = "edhsrsadasy35w3wdth" };
            var employee3 = new Employee { Id = 200, FirstName = "34t34t", LastName = "sa34xtdsad", Position = "w434b5" };

            List<Employee> employees = new() { employee1, employee2, employee3 };

            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));

            var mapper = config.CreateMapper();

            var mock = new Mock<IEmployeeRepository>();

            var mock2 = GetMockUserManager();

            mock.SetupAsync(s => s.GetEmployeesAsync()).Returns(employees.AsQueryable());

            var service = new EmployeeService(mapper, mock.Object, mock2.Object);

            var result = await service.GetEmployeesAsync();

            Assert.NotNull(result);
            for (int i = 0; i < employees.Count; i++)
            {
                Assert.Equal(employees[i].FirstName, result[i].FirstName);
                Assert.Equal(employees[i].LastName, result[i].LastName);
                Assert.Equal(employees[i].Position, result[i].Position);
            }
        }

        [Fact]
        public async Task CheckUpdateEmployee()
        {
            var employee = new Employee();

            var employeeVMe = new EmployeeVM();

            var employeeVM = new EmployeeVM
            {
                Id = 100,
                FirstName = "Zordon",
                LastName = "Rasista",
                Position = "edhsrth",
                Email = "maniel@pajac.com",
                Password = "Pa$$w0rd!"
            };

            var userE = new AppUser();

            var user = new AppUser
            {
                Id = "sfawrg",
                UserName = "Zordon",
                Email = "maniel@pajac.com"
            };

            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));

            var mapper = config.CreateMapper();

            var mock = new Mock<IEmployeeRepository>();

            var mock2 = GetMockUserManager();

            mock2.SetupAsync(s => s.FindByIdAsync(It.IsAny<string>())).Returns(user);

            mock2.Setup(s => s.SetEmailAsync(user, It.IsAny<string>())).Callback<AppUser, string>((a, s) =>
            {
                Assert.Equal(a.Id, user.Id);
                Assert.Equal(a.UserName, user.UserName);
                Assert.Equal(a.Email, user.Email);
            });

            mock.SetupAsync(s => s.UpdateEmployeeAsync(It.IsAny<Employee>())).Callback<Employee>((p) =>
            {
                employee = p;
            });

            var service = new EmployeeService(mapper, mock.Object, mock2.Object);

            await service.UpdateEmployeeAsync(employeeVM);

            mock.Verify(x => x.UpdateEmployeeAsync(It.IsAny<Employee>()), Times.Once());

            Assert.Equal(employee.FirstName, employeeVM.FirstName);
            Assert.Equal(employee.LastName, employeeVM.LastName);
            Assert.Equal(employee.Position, employeeVM.Position);
        }
        /*
                [Fact]
                public async Task CheckIfRepositoryDeleteMethodWasCalledWithCorrectData()
                {
                    var employee1 = new Employee();
                    var employee2 = new Employee { Id = 150, FirstName = "sadfsd", LastName = "o7hl9", Position = "edhsrsadasy35w3wdth" };

                    var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));

                    var mapper = config.CreateMapper();

                    var mock = new Mock<IEmployeeRepository>();

                    mock.SetupAsync(s => s.DeleteEmployeeAsync(It.IsAny<int>())).Callback<int>((p) =>
                    {
                        employee1.Id = p;
                    });

                    var service = new EmployeeService(mapper, mock.Object);

                    await service.DeleteEmployeeAsync(employee2.Id);

                    mock.Verify(x => x.DeleteEmployeeAsync(It.IsAny<int>()), Times.Once());

                    Assert.Equal(employee1.Id, employee2.Id);
                }
        */
    }
}
