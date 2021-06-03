using System;
using Xunit;
using EcommerceApp.Domain.Models;
using EcommerceApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EcommerceApp.Infrastructure.Tests
{
    public class EmployeeRepositoryUnitTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public EmployeeRepositoryUnitTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        }

        [Fact]
        public async Task AddEmplyeeAsync_AddEmployee()
        {
            var employee = new Employee { Id = 100, FirstName = "Zordon", LastName = "Rasista", Position = "edhsrth" };

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                var sut = new EmployeeRepository(context);
                await sut.AddEmplyeeAsync(employee);
                var result = await context.Employees.FindAsync(employee.Id);
                Assert.NotNull(result);
                Assert.Equal(employee, result);
            }
        }

        [Fact]
        public async Task GetEmployeeAsync_ReturnEmployee()
        {
            var employee = new Employee { Id = 100, FirstName = "Zordon", LastName = "Rasista", Position = "edhsrth" };

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                await context.AddAsync(employee);
                await context.SaveChangesAsync();
                var sut = new EmployeeRepository(context);
                var result = await sut.GetEmployeeAsync(employee.Id);
                Assert.NotNull(result);
                Assert.Equal(employee.Id, result.Id);
            }
        }

        [Fact]
        public async Task GetEmployeesAsync_ReturnIQueryableOfEmployees()
        {
            var employee1 = new Employee { Id = 100, FirstName = "Zordon", LastName = "Rasista", Position = "edhsrth" };
            var employee2 = new Employee { Id = 150, FirstName = "Zordon", LastName = "Rasista", Position = "edhsrth" };
            var employee3 = new Employee { Id = 200, FirstName = "Zordon", LastName = "Rasista", Position = "edhsrth" };

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                List<Employee> employees = new() { employee1, employee2, employee3 };
                await context.AddRangeAsync(employees);
                await context.SaveChangesAsync();
                var sut = new EmployeeRepository(context);
                var result = await sut.GetEmployeesAsync();
                Assert.NotNull(result);
                Assert.Equal(employees, result);
            }
        }

        [Fact]
        public async Task UpdateEmployeeAsync_UpdateEmployee()
        {
            var employee1 = new Employee { Id = 100, FirstName = "Zordon", LastName = "Rasista", Position = "edhsrth" };
            var employee2 = new Employee { Id = 100, FirstName = "Maniek", LastName = "Fajowski", Position = "act4c4" };

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                await context.AddAsync(employee1);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                var sut = new EmployeeRepository(context);
                await sut.UpdateEmployeeAsync(employee2);
                var result = await context.Employees.FindAsync(employee2.Id);
                Assert.NotNull(result);
                Assert.Equal(employee2, result);
            }
        }

        [Fact]
        public async Task DeleteEmployeeAsync_DeleteEmployee()
        {
            var employee = new Employee { Id = 100, FirstName = "Zordon", LastName = "Rasista", Position = "edhsrth" };

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                await context.AddAsync(employee);
                await context.SaveChangesAsync();
                var sut = new EmployeeRepository(context);
                await sut.DeleteEmployeeAsync(employee.Id);
                var result = await context.Employees.FindAsync(employee.Id);
                Assert.Null(result);
            }
        }
    }
}
