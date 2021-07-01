using System;
using Xunit;
using EcommerceApp.Domain.Models;
using EcommerceApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace EcommerceApp.Infrastructure.Tests.Repositories.UnitTests
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
        public async Task GetEmployeesAsync_ReturnsIQueryableOfEmployees()
        {
            var employee1 = new Employee { Id = 100, FirstName = "Zordon", LastName = "Rasista", Position = "edhsrth" };
            var employee2 = new Employee { Id = 150, FirstName = "Zordon", LastName = "Rasista", Position = "edhsrth" };
            var employee3 = new Employee { Id = 200, FirstName = "Zordon", LastName = "Rasista", Position = "edhsrth" };
            var employees = new List<Employee> { employee1, employee2, employee3 };
            var employeesQ = employees.AsQueryable();

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                await context.AddRangeAsync(employees);
                await context.SaveChangesAsync();
                var sut = new EmployeeRepository(context);
                var result = sut.GetEmployees();
                Assert.NotNull(result);
                Assert.Equal(employeesQ, result);
            }
        }

        [Fact]
        public async Task UpdateEmployeeAsync_UpdatesEmployee()
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
    }
}
