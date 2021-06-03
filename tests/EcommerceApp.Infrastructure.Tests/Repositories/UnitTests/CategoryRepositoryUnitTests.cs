using System;
using Xunit;
using EcommerceApp.Domain.Models;
using EcommerceApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EcommerceApp.Infrastructure.Tests
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
        public async Task AddCategoryAsync_AddCategory()
        {
            var category = new Category { Id = 100, Name = "Food", Description = "dobre bardzo" };

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                var sut = new CategoryRepository(context);
                await sut.AddCategoryAsync(category);
                var addedCategory = await context.Categories.FindAsync(category.Id);
                Assert.NotNull(addedCategory);
                Assert.Equal(category, addedCategory);
            }
        }

        [Fact]
        public async Task GetCategoryAsync_ReturnCategory()
        {
            var category = new Category { Id = 101, Name = "Food", Description = "dobre bardzo" };

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                await context.AddAsync(category);
                await context.SaveChangesAsync();
                var sut = new CategoryRepository(context);
                var getCategory = await sut.GetCategoryAsync(category.Id);
                Assert.NotNull(getCategory);
                Assert.Equal(category.Id, category.Id);
            }
        }

        [Fact]
        public async Task GetCategoriesAsync_ReturnIQueryableOfCategories()
        {
            var category1 = new Category { Id = 101, Name = "Food", Description = "dobre bardzo" };
            var category2 = new Category { Id = 102, Name = "w4d5", Description = "do43wt4zo" };
            var category3 = new Category { Id = 103, Name = "34wctf3", Description = "dob34cty3qwy45yrdzo" };

            using (var context = new AppDbContext(_options))
            {
                await context.Database.EnsureCreatedAsync();
                List<Category> categories = new() { category1, category2, category3 };
                await context.AddRangeAsync(categories);
                await context.SaveChangesAsync();
                var sut = new CategoryRepository(context);
                var getCategories = await sut.GetCategoriesAsync();
                Assert.NotNull(getCategories);
                Assert.Equal(categories, getCategories);
            }
        }

        [Fact]
        public async Task UpdateCategoryAsync_UpdateCategory()
        {
            var category1 = new Category { Id = 101, Name = "Food", Description = "dobre bardzo" };
            var category2 = new Category { Id = 101, Name = "w4d5", Description = "do43wt4zo" };

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
        public async Task DeleteCategoryAsync_DeleteCategory()
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