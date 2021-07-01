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
    public class CategoryRepositoryUnitTests
    {
        private readonly DbContextOptions<AppDbContext> _options;

        public CategoryRepositoryUnitTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
        }

        [Fact]
        public async Task AddCategoryAsync_AddsCategory()
        {
            var category = new Category { Id = 100, Name = "Food", Description = "dobre bardzo" };

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                var sut = new CategoryRepository(context);
                await sut.AddCategoryAsync(category);
                var result = await context.Categories.FindAsync(category.Id);
                Assert.NotNull(result);
                Assert.Equal(category, result);
            }
        }

        [Fact]
        public async Task GetCategoryAsync_ReturnsCategory()
        {
            var category = new Category { Id = 101, Name = "Food", Description = "dobre bardzo" };

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                await context.AddAsync(category);
                await context.SaveChangesAsync();
                var sut = new CategoryRepository(context);
                var result = await sut.GetCategoryAsync(category.Id);
                Assert.NotNull(result);
                Assert.Equal(category.Id, result.Id);
            }
        }

        [Fact]
        public async Task GetCategoriesAsync_ReturnsIQueryableOfCategories()
        {
            var category1 = new Category { Id = 101, Name = "Food", Description = "dobre bardzo" };
            var category2 = new Category { Id = 102, Name = "w4d5", Description = "do43wt4zo" };
            var category3 = new Category { Id = 103, Name = "34wctf3", Description = "dob34cty3qwy45yrdzo" };
            var categories = new List<Category> { category1, category2, category3 };
            var categoriesQ = categories.AsQueryable();

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                await context.AddRangeAsync(categories);
                await context.SaveChangesAsync();
                var sut = new CategoryRepository(context);
                var result = sut.GetCategories();
                Assert.NotNull(result);
                Assert.Equal(categoriesQ, result);
            }
        }

        [Fact]
        public async Task UpdateCategoryAsync_UpdatesCategory()
        {
            var category1 = new Category { Id = 100, Name = "Food", Description = "dobre bardzo", Image = new byte[] { 1, 2, 3 } };
            var category2 = new Category { Id = 100, Name = "w4d5", Description = "do43wt4zo", Image = new byte[] { 3, 2, 1 } };

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                await context.AddAsync(category1);
                await context.SaveChangesAsync();
            }

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                var sut = new CategoryRepository(context);
                await sut.UpdateCategoryAsync(category2);
                var updatedCategory = await context.Categories.FindAsync(category2.Id);
                Assert.NotNull(updatedCategory);
                Assert.Equal(category2, updatedCategory);
            }
        }

        [Fact]
        public async Task DeleteCategoryAsync_DeletesCategory()
        {
            var category = new Category { Id = 101, Name = "Food", Description = "dobre bardzo" };

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                await context.AddAsync(category);
                await context.SaveChangesAsync();
                var sut = new CategoryRepository(context);
                await sut.DeleteCategoryAsync(category.Id);
                var deletedCategory = await context.Categories.FindAsync(category.Id);
                Assert.Null(deletedCategory);
            }
        }
    }
}