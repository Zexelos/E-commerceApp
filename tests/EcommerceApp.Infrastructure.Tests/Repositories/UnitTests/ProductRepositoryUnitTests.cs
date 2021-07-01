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
    public class ProductRepositoryUnitTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public ProductRepositoryUnitTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        }

        [Fact]
        public async Task AddProductAsync_AddsCategory()
        {
            var product = new Product { Id = 100, Name = "sagwer", Description = "esragberg", UnitPrice = 12.32m, UnitsInStock = 2 };

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                var sut = new ProductRepository(context);
                await sut.AddProductAsync(product);
                var result = await context.Products.FindAsync(product.Id);
                Assert.NotNull(result);
                Assert.Equal(result, product);
            }
        }

        [Fact]
        public async Task GetProductAsync_ReturnsProduct()
        {
            var product = new Product { Id = 100, Name = "sagwer", Description = "esragberg", UnitPrice = 12.32m, UnitsInStock = 2 };

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                await context.Products.AddAsync(product);
                await context.SaveChangesAsync();
                var sut = new ProductRepository(context);
                var result = await sut.GetProductAsync(product.Id);
                Assert.NotNull(result);
                Assert.Equal(product, result);
            }
        }

        [Fact]
        public async Task GetProductsAsync_ReturnsIQueryableOfProducts()
        {
            var product1 = new Product { Id = 100, Name = "sagwer", Description = "esragberg", UnitPrice = 12.32m, UnitsInStock = 2 };
            var product2 = new Product { Id = 150, Name = "ew4t", Description = "awx23", UnitPrice = 5.32m, UnitsInStock = 5 };
            var product3 = new Product { Id = 200, Name = "ecrye", Description = "vgw53", UnitPrice = 12.2m, UnitsInStock = 1 };
            var products = new List<Product> { product1, product2, product3 };
            var productsQ = products.AsQueryable();

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                await context.Products.AddRangeAsync(products);
                await context.SaveChangesAsync();
                var sut = new ProductRepository(context);
                var result = sut.GetProducts();
                Assert.NotNull(result);
                Assert.Equal(productsQ, result);
            }
        }

        [Fact]
        public async Task UpdateProductAsync_UpdatesProduct()
        {
            var product1 = new Product { Id = 100, Name = "sagwer", Description = "esragberg", UnitPrice = 12.32m, UnitsInStock = 2, Image = new byte[] { 1, 2, 3 } };
            var product2 = new Product { Id = 100, Name = "ew4t", Description = "awx23", UnitPrice = 5.32m, UnitsInStock = 5, Image = new byte[] { 3, 2, 1 } };

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
        public async Task UpdateProductsAsync_UpdatesProducts()
        {
            var product1 = new Product { Id = 100, Name = "sagwer", Description = "esragberg", UnitPrice = 12.32m, UnitsInStock = 2, Image = new byte[] { 1, 2, 3 } };
            var product2 = new Product { Id = 150, Name = "ew4t", Description = "awx23", UnitPrice = 5.32m, UnitsInStock = 5, Image = new byte[] { 3, 2, 1 } };
            var products1 = new List<Product> { product1, product2 };
            var product3 = new Product { Id = 100, Name = "567bj", Description = "3c2y", UnitPrice = 3.32m, UnitsInStock = 1, Image = new byte[] { 1, 1, 1 } };
            var product4 = new Product { Id = 150, Name = "4vw3u6", Description = "687bo4", UnitPrice = 2.32m, UnitsInStock = 1, Image = new byte[] { 2, 2, 2 } };
            var products2 = new List<Product> { product3, product4 };

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                await context.Products.AddRangeAsync(products1);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                var sut = new ProductRepository(context);
                await sut.UpdateProductsAsync(products2);
                var result = await context.Products.ToListAsync();
                Assert.NotNull(result);
                Assert.Equal(products2, result);
            }
        }

        [Fact]
        public async Task DeleteProductAsync_DeletesProduct()
        {
            var product = new Product { Id = 100, Name = "sagwer", Description = "esragberg", UnitPrice = 12.32m, UnitsInStock = 2 };

            using (var context = new AppDbContext(_options))
            {
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
}
