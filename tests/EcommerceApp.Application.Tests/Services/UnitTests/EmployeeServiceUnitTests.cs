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

namespace EcommerceApp.Application.Tests
{
    public class EmployeeServiceUnitTests
    {
        [Fact]
        public async Task CheckEmployeeExistenceAfterAdd()
        {
            var employee = new Employee();

            var employeeVM = new EmployeeVM { FirstName = "Zordon", LastName = "Rasista", Position = "edhsrth" };

            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));

            var mapper = config.CreateMapper();

            var mock = new Mock<IEmployeeRepository>();

            mock.SetupAsync(s => s.AddEmplyeeAsync(It.IsAny<Employee>())).Callback<Employee>((p) =>
            {
                employee = p;
            });

            var service = new EmployeeService(mapper, mock.Object);

            await service.AddEmployeeAsync(employeeVM);

            mock.Verify(x => x.AddEmplyeeAsync(It.IsAny<Employee>()), Times.Once());

            Assert.Equal(employee.FirstName, employeeVM.FirstName);
            Assert.Equal(employee.LastName, employeeVM.LastName);
            Assert.Equal(employee.Position, employeeVM.Position);
        }

        [Fact]
        public async Task CheckEmployeeExistence()
        {
            var employee = new Employee { Id = 100, FirstName = "adssad", LastName = "sadsad", Position = "edhsrsadasdth" };

            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));

            var mapper = config.CreateMapper();

            var mock = new Mock<IEmployeeRepository>();

            mock.SetupAsync(s => s.GetEmployeeAsync(It.IsAny<int>())).Returns(employee);

            var service = new EmployeeService(mapper, mock.Object);

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

            mock.SetupAsync(s => s.GetEmployeesAsync()).Returns(employees.AsQueryable());

            var service = new EmployeeService(mapper, mock.Object);

            var result = await service.GetEmployeesAsync();

            Assert.NotNull(result);
            for (int i = 0; i < employees.Count; i++)
            {
                Assert.Equal(employees[i].FirstName, result[i].FirstName);
                Assert.Equal(employees[i].LastName, result[i].LastName);
                Assert.Equal(employees[i].Position, result[i].Position);
            }
        }
    }
}
