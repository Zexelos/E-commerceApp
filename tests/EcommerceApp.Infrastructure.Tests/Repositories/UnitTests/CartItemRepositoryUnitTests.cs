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
    public class CartItemRepositoryUnitTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public CartItemRepositoryUnitTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        }

        [Fact]
        public async Task AddCartItemAsync_AddsCartItem()
        {
            var cartItem = new CartItem { Id = 100, Quantity = 3 };

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                var sut = new CartItemRepository(context);
                await sut.AddCartItemAsync(cartItem);
                var result = await context.CartItems.FindAsync(cartItem.Id);
                Assert.NotNull(result);
                Assert.Equal(cartItem, result);
            }
        }

        [Fact]
        public async Task GetCartItemAsync_ReturnsCartItem()
        {
            var cartItem = new CartItem { Id = 100, Quantity = 3 };

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                await context.CartItems.AddAsync(cartItem);
                await context.SaveChangesAsync();
                var sut = new CartItemRepository(context);
                var result = await sut.GetCartItemAsync(cartItem.Id);
                Assert.NotNull(result);
                Assert.Equal(cartItem, result);
            }
        }

        [Fact]
        public async Task GetCartItems_ReturnsIQueryableOfCartItems()
        {
            var cartItem1 = new CartItem { Id = 100, Quantity = 3 };
            var cartItem2 = new CartItem { Id = 150, Quantity = 2 };
            var cartItem3 = new CartItem { Id = 200, Quantity = 15 };
            var cartItems = new List<CartItem> { cartItem1, cartItem2, cartItem3 };
            var cartItemsQ = cartItems.AsQueryable();

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                await context.CartItems.AddRangeAsync(cartItems);
                await context.SaveChangesAsync();
                var sut = new CartItemRepository(context);
                var result = sut.GetCartItems();
                Assert.NotNull(result);
                Assert.Equal(cartItemsQ, result);
            }
        }

        [Fact]
        public async Task UpdateCartItemAsync_UpdatedCartItem()
        {
            var cartItem1 = new CartItem { Id = 100, Quantity = 3 };
            var cartItem2 = new CartItem { Id = 100, Quantity = 2 };

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                await context.CartItems.AddAsync(cartItem1);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                var sut = new CartItemRepository(context);
                await sut.UpdateCartItemAsync(cartItem2);
                var result = await context.CartItems.FindAsync(cartItem1.Id);
                Assert.NotNull(result);
                Assert.Equal(cartItem2, result);
            }
        }

        [Fact]
        public async Task DeleteCartItemAsync_DeletesCartItem()
        {
            var cartItem = new CartItem { Id = 100, Quantity = 3 };

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                await context.CartItems.AddAsync(cartItem);
                await context.SaveChangesAsync();
                var sut = new CartItemRepository(context);
                await sut.DeleteCartItemAsync(cartItem.Id);
                var result = await context.CartItems.FindAsync(cartItem.Id);
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task DeleteCartItemsByCartIdAsync_DeletesCartItems()
        {
            var cartItem1 = new CartItem { Id = 100, Quantity = 3 };
            var cartItem2 = new CartItem { Id = 150, Quantity = 2 };
            var cartItem3 = new CartItem { Id = 200, Quantity = 15 };
            var cartItems = new List<CartItem> { cartItem1, cartItem2, cartItem3 };
            var cart = new Cart { Id = 1, CartItems = cartItems }; ;

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                await context.Carts.AddAsync(cart);
                await context.SaveChangesAsync();
                var sut = new CartItemRepository(context);
                await sut.DeleteCartItemsByCartIdAsync(cart.Id);
                var result = await context.CartItems.ToListAsync();
                var resultCount = result.Count;
                Assert.Equal(0, resultCount);
            }
        }
    }
}
