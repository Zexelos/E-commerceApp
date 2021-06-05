using System.Timers;
using System.Threading;
using System.Linq;
using System.Reflection;
using System;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceApp.Application.Mapping;
using EcommerceApp.Application.Services;
using EcommerceApp.Application.ViewModels;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Moq;
using Xunit;
using System.Collections.Generic;
using EcommerceApp.Application.Interfaces;

namespace EcommerceApp.Application.Tests
{
    public class CategoryServiceUnitTests
    {
        private readonly CategoryService _sut;
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<ICategoryRepository> _repository = new();
        private readonly Mock<IImageConverterService> _imageConverterService = new();

        public CategoryServiceUnitTests()
        {
            _sut = new CategoryService(_mapper.Object, _repository.Object, _imageConverterService.Object);
        }

        [Fact]
        public async Task AddCategoryAsync_MethodsRunOnce()
        {
            // Arrange
            CategoryVM categoryVM = new() { Id = 100, Name = "asfewg", Description = "segsegrgerdhreh" };
            Category category = new() { Id = 100, Name = "asfewg", Description = "segsegrgerdhreh" };

            _mapper.Setup(s => s.Map<Category>(categoryVM)).Returns(category);

            // Act
            await _sut.AddCategoryAsync(categoryVM);

            // Assert
            _repository.Verify(v => v.AddCategoryAsync(category), Times.Once);
        }

        [Fact]
        public async Task GetCategoryAsync_ReturnCategoryVM()
        {
            // Arrange
            CategoryVM categoryVM = new() { Id = 100, Name = "asfewg", Description = "segsegrgerdhreh" };
            Category category = new() { Id = 100, Name = "asfewg", Description = "segsegrgerdhreh" };

            _repository.Setup(s => s.GetCategoryAsync(categoryVM.Id)).ReturnsAsync(category);
            _mapper.Setup(s => s.Map<CategoryVM>(category)).Returns(categoryVM);

            // Act
            var result = await _sut.GetCategoryAsync(categoryVM.Id);

            // Assert
            Assert.Equal(categoryVM, result);
        }

        [Fact]
        public async Task GetCategoriesAsync_ReturnListOfCategories()
        {
            // Arrange
            List<Category> categories = new()
            {
                new() { Id = 100, Name = "asfewg", Description = "segsegrgerdhreh" },
                new() { Id = 150, Name = "we43tx34", Description = "3q4xtgf2qx4" },
                new() { Id = 200, Name = "rce6h756bj", Description = "67ib64vjh46" },
            };

            List<CategoryVM> categoryVMs = new()
            {
                new() { Id = 100, Name = "asfewg", Description = "segsegrgerdhreh" },
                new() { Id = 150, Name = "we43tx34", Description = "3q4xtgf2qx4" },
                new() { Id = 200, Name = "rce6h756bj", Description = "67ib64vjh46" },
            };

            _repository.Setup(s => s.GetCategoriesAsync()).ReturnsAsync(categories.AsQueryable());
            _mapper.Setup(s => s.Map<List<CategoryVM>>(categories)).Returns(categoryVMs);

            // Act
            var result = await _sut.GetCategoriesAsync();

            // Assert
            Assert.Equal(categoryVMs, result);
        }

        [Fact]
        public async Task UpdateCategoryAsync_RunsMethodsOnce()
        {
            // Arrange
            CategoryVM categoryVM = new() { Id = 100, Name = "asfewg", Description = "segsegrgerdhreh" };
            Category category = new() { Id = 100, Name = "asfewg", Description = "segsegrgerdhreh" };

            _mapper.Setup(s => s.Map<Category>(categoryVM)).Returns(category);

            // Act
            await _sut.UpdateCategoryAsync(categoryVM);

            // Assert
            _repository.Verify(v => v.UpdateCategoryAsync(category), Times.Once);
        }

        [Fact]
        public async Task DeleteCategoryAsync_RunMethodOnce()
        {
            // Arrange
            int id = 100;

            // Act
            await _sut.DeleteCategoryAsync(id);

            // Assert
            _repository.Verify(v => v.DeleteCategoryAsync(id), Times.Once);
        }
    }
}
