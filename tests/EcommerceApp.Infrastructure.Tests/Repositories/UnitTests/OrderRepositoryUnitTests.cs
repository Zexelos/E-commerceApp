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
    public class OrderRepositoryUnitTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public OrderRepositoryUnitTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        }

        [Fact]
        public async Task AddOrderAsync_AddsOrder()
        {
            var order = new Order { Id = 100, ShipFirstName = "sdegwer" };

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                var sut = new OrderRepository(context);
                await sut.AddOrderAsync(order);
                var result = await context.Orders.FindAsync(order.Id);
                Assert.NotNull(result);
                Assert.Equal(order, result);
            }
        }

        [Fact]
        public async Task GetOrders_ReturnsIQueryableOfOrders()
        {
            var order1 = new Order { Id = 100, ShipFirstName = "sdegwer" };
            var order2 = new Order { Id = 150, ShipFirstName = "45f7345" };
            var order3 = new Order { Id = 200, ShipFirstName = "dxvrvxy5ybvx5" };
            var orders = new List<Order> { order1, order2, order3 };
            var ordersQ = orders.AsQueryable();

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                await context.Orders.AddRangeAsync(orders);
                await context.SaveChangesAsync();
                var sut = new OrderRepository(context);
                var result = sut.GetOrders();
                Assert.NotNull(result);
                Assert.Equal(ordersQ, result);
            }
        }

        [Fact]
        public async Task DeleteOrderAsync_DeletesOrder()
        {
            var order = new Order { Id = 100, ShipFirstName = "sdegwer" };

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                await context.Orders.AddAsync(order);
                await context.SaveChangesAsync();
                var sut = new OrderRepository(context);
                await sut.DeleteOrderAsync(order.Id);
                var result = await context.Orders.FindAsync(order.Id);
                Assert.Null(result);
            }
        }
    }
}
