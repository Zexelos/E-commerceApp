using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Models;
using EcommerceApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EcommerceApp.Infrastructure.Tests.Repositories.UnitTests
{
    public class CustomerRepositoryUnitTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public CustomerRepositoryUnitTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        }

        [Fact]
        public async Task GetCustomerIdAsync_ReturnsCustomerId()
        {
            var customer = new Customer { Id = 100, Address = "zseg3t24234t", AppUserId = "ders5yc4ewcvy4" };

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                await context.Customers.AddAsync(customer);
                await context.SaveChangesAsync();
                var sut = new CustomerRepository(context);
                var result = await sut.GetCustomerIdAsync(customer.AppUserId);
                Assert.Equal(customer.Id, result);
            }
        }

        [Fact]
        public async Task GetCustomers_ReturnsIQueryableOfCustomers()
        {
            var customer1 = new Customer { Id = 100, Address = "zseg3t24234t" };
            var customer2 = new Customer { Id = 150, Address = "zseg3t24234t" };
            var customer3 = new Customer { Id = 200, Address = "zseg3t24234t" };
            var customers = new List<Customer> { customer1, customer2, customer3 };
            var customersQ = customers.AsQueryable();

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                await context.Customers.AddRangeAsync(customers);
                await context.SaveChangesAsync();
                var sut = new CustomerRepository(context);
                var result = sut.GetCustomers();
                Assert.NotNull(result);
                Assert.Equal(customersQ, result);
            }
        }
    }
}
