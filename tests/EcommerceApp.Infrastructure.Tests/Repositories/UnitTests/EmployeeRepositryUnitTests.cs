using System;
using Xunit;
using EcommerceApp.Domain.Models;
using EcommerceApp.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
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
                context.Database.EnsureCreated();
                var repository = new EmployeeRepository(context);
                await repository.AddEmplyeeAsync(employee);
                var addedEmployee = await context.Employees.FindAsync(employee.Id);
                Assert.NotNull(addedEmployee);
                Assert.Equal(employee, addedEmployee);
            }
        }

        [Fact]
        public async Task GetEmployeeAsync_ReturnEmployee()
        {
            var employee = new Employee { Id = 100, FirstName = "Zordon", LastName = "Rasista", Position = "edhsrth" };

            using (var context = new AppDbContext(_options))
            {
                context.Database.EnsureCreated();
                await context.AddAsync(employee);
                await context.SaveChangesAsync();
                var repository = new EmployeeRepository(context);
                var getEmployee = await repository.GetEmployeeAsync(employee.Id);
                Assert.NotNull(getEmployee);
                Assert.Equal(employee.Id, getEmployee.Id);
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
                context.Database.EnsureCreated();
                List<Employee> employees = new() { employee1, employee2, employee3 };
                await context.AddRangeAsync(employees);
                await context.SaveChangesAsync();
                var repository = new EmployeeRepository(context);
                var getEmployees = await repository.GetEmployeesAsync();
                Assert.NotNull(getEmployees);
                Assert.Equal(employees, getEmployees);
            }
        }

        [Fact]
        public async Task UpdateEmployeeAsync_UpdateEmployee()
        {
            var employee1 = new Employee { Id = 100, FirstName = "Zordon", LastName = "Rasista", Position = "edhsrth" };
            var employee2 = new Employee { Id = 100, FirstName = "Maniek", LastName = "Fajowski", Position = "act4c4" };

            using (var context = new AppDbContext(_options))
            {
                context.Database.EnsureCreated();
                await context.AddAsync(employee1);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(_options))
            {
                context.Database.EnsureCreated();
                var repository = new EmployeeRepository(context);
                await repository.UpdateEmployeeAsync(employee2);
                var updatedEmployee = await context.Employees.FindAsync(employee2.Id);
                Assert.NotNull(updatedEmployee);
                Assert.Equal(employee2, updatedEmployee);
            }
        }

        [Fact]
        public async Task DeleteEmployeeAsync_DeleteEmployee()
        {
            var employee = new Employee { Id = 100, FirstName = "Zordon", LastName = "Rasista", Position = "edhsrth" };

            using (var context = new AppDbContext(_options))
            {
                context.Database.EnsureCreated();
                await context.AddAsync(employee);
                await context.SaveChangesAsync();
                var repository = new EmployeeRepository(context);
                await repository.DeleteEmployeeAsync(employee.Id);
                var deletedEmployee = await context.Employees.FindAsync(employee.Id);
                Assert.Null(deletedEmployee);
            }
        }
    }
}
