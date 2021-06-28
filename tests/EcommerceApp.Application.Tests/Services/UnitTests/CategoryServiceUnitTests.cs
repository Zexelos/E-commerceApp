using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceApp.Application.Services;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Moq;
using Xunit;
using System.Collections.Generic;
using EcommerceApp.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace EcommerceApp.Application.Tests
{
    public class CategoryServiceUnitTests
    {
        private readonly CategoryService _sut;
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<ICategoryRepository> _repository = new();
        private readonly Mock<IImageConverterService> _imageConverterService = new();
        private readonly Mock<IFormFile> _formFile = new();
        private readonly Mock<IPaginatorService<CategoryForListVM>> _paginatorService = new();

        public CategoryServiceUnitTests()
        {
            _sut = new CategoryService(
                _mapper.Object,
                _repository.Object,
                _imageConverterService.Object,
                _paginatorService.Object);
        }

        [Fact]
        public async Task AddCategoryAsync_MethodsRunOnce()
        {
            // Arrange
            var image = new byte[] { 2, 12, 4 };
            CategoryVM categoryVM = new() { Id = 100, Name = "asfewg", Description = "segsegrgerdhreh" };
            Category category = new() { Id = 100, Name = "asfewg", Description = "segsegrgerdhreh" };

            _mapper.Setup(s => s.Map<Category>(categoryVM)).Returns(category);

            _imageConverterService.Setup(s => s.GetByteArrayFromFormFile(_formFile.Object)).ReturnsAsync(image);

            // Act
            await _sut.AddCategoryAsync(categoryVM);

            // Assert
            _repository.Verify(v => v.AddCategoryAsync(category), Times.Once);
        }

        [Fact]
        public async Task GetCategoryAsync_ReturnsCategoryVM()
        {
            // Arrange
            CategoryVM categoryVM = new() { Id = 100, Name = "asfewg", Description = "segsegrgerdhreh", ImageToDisplay = "adsfgewgegsw" };
            Category category = new() { Id = 100, Name = "asfewg", Description = "segsegrgerdhreh", Image = new byte[] { 2, 12, 4 } };

            _repository.Setup(s => s.GetCategoryAsync(categoryVM.Id)).ReturnsAsync(category);

            _mapper.Setup(s => s.Map<CategoryVM>(category)).Returns(categoryVM);

            _imageConverterService.Setup(s => s.GetImageStringFromByteArray(category.Image)).Returns(categoryVM.ImageToDisplay);

            // Act
            var result = await _sut.GetCategoryAsync(categoryVM.Id);

            // Assert
            Assert.NotNull(result)
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
            CategoryVM categoryVM = new() { Id = 100, Name = "asfewg", Description = "segsegrgerdhreh", Image = new byte[] { 2, 12, 4 } };
            Category category = new() { Id = 100, Name = "asfewg", Description = "segsegrgerdhreh" };

            _mapper.Setup(s => s.Map<Category>(categoryVM)).Returns(category);

            _imageConverterService.Setup(s => s.GetImageStringFromByteArray(categoryVM.Image));

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
