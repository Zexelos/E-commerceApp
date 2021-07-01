using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using EcommerceApp.Domain.Models;
using EcommerceApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Linq;

namespace EcommerceApp.Infrastructure.Tests.Repositories.UnitTests
{
    public class CartRepositoryUnitTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public CartRepositoryUnitTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        }

        [Fact]
        public async Task GetCarts_ReturnsIQueryableOfCarts()
        {
            var cart1 = new Cart { Id = 100 };
            var cart2 = new Cart { Id = 150 };
            var cart3 = new Cart { Id = 200 };
            var carts = new List<Cart> { cart1, cart2, cart3 };
            var cartsQ = carts.AsQueryable();

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                await context.Carts.AddRangeAsync(carts);
                await context.SaveChangesAsync();
                var sut = new CartRepository(context);
                var result = sut.GetCarts();
                Assert.NotNull(result);
                Assert.Equal(cartsQ, result);
            }
        }
    }
}
