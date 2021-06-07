using System;
using Xunit;
using EcommerceApp.Domain.Models;
using EcommerceApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EcommerceApp.Infrastructure.Tests.Repositories.UnitTests
{
    public class ProductRepositoryUnitTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public ProductRepositoryUnitTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        }

        [Fact]
        public async Task AddProductAsync_AddCategory()
        {
            var product = new Product { Id = 100, Name = "sagwer", Description = "esragberg", UnitPrice = 12.32m, UnitsInStock = 2 };

            using var context = new AppDbContext(_options);
            await context.Database.EnsureCreatedAsync();
            var sut = new ProductRepository(context);
            await sut.AddProductAsync(product);
            var result = await context.Products.FindAsync(product.Id);
            Assert.NotNull(result);
            Assert.Equal(result, product);
        }

        [Fact]
        public async Task GetProductAsync_ReturnProduct()
        {
            var product = new Product { Id = 100, Name = "sagwer", Description = "esragberg", UnitPrice = 12.32m, UnitsInStock = 2 };

            using var context = new AppDbContext(_options);
            await context.Database.EnsureCreatedAsync();
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
            var sut = new ProductRepository(context);
            var result = await sut.GetProductAsync(product.Id);
            Assert.NotNull(result);
            Assert.Equal(product, result);
        }

        [Fact]
        public async Task GetProductsAsync_ReturnProducts()
        {
            var product1 = new Product { Id = 100, Name = "sagwer", Description = "esragberg", UnitPrice = 12.32m, UnitsInStock = 2 };
            var product2 = new Product { Id = 150, Name = "ew4t", Description = "awx23", UnitPrice = 5.32m, UnitsInStock = 5 };
            var product3 = new Product { Id = 200, Name = "ecrye", Description = "vgw53", UnitPrice = 12.2m, UnitsInStock = 1 };
            var products = new List<Product> { product1, product2, product3 };

            using var context = new AppDbContext(_options);
            await context.Database.EnsureCreatedAsync();
            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
            var sut = new ProductRepository(context);
            var result = await sut.GetProductsAsync();
            Assert.NotNull(result);
            Assert.Equal(products, result);
        }

        [Fact]
        public async Task UpdateProductAsync_UpdateProduct()
        {
            var product1 = new Product { Id = 100, Name = "sagwer", Description = "esragberg", UnitPrice = 12.32m, UnitsInStock = 2 };
            var product2 = new Product { Id = 100, Name = "ew4t", Description = "awx23", UnitPrice = 5.32m, UnitsInStock = 5 };

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                await context.Products.AddAsync(product1);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                var sut = new ProductRepository(context);
                await sut.UpdateProductAsync(product2);
                var result = await context.Products.FindAsync(product2.Id);
                Assert.NotNull(result);
                Assert.Equal(product2, result);
            }
        }

        [Fact]
        public async Task DeleteProductAsync_DeleteProduct()
        {
            var product = new Product { Id = 100, Name = "sagwer", Description = "esragberg", UnitPrice = 12.32m, UnitsInStock = 2 };

            using var context = new AppDbContext(_options);
            await context.Database.EnsureCreatedAsync();
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
            var sut = new ProductRepository(context);
            await sut.DeleteProductAsync(product.Id);
            var result = await context.Products.FindAsync(product.Id);
            Assert.Null(result);
        }
    }
}
