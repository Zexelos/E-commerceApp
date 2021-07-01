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
using EcommerceApp.Application.ViewModels;

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

            _imageConverterService.Setup(s => s.GetByteArrayFromFormFileAsync(_formFile.Object)).ReturnsAsync(image);

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
            Assert.NotNull(result);
            Assert.Equal(categoryVM, result);
        }

        [Fact]
        public async Task GetPaginatedCategoriesAsync_ReturnsCategoryListVM()
        {
            // Arrange
            List<Category> categories = new()
            {
                new() { Id = 100, Name = "asfewg", Description = "segsegrgerdhreh" },
                new() { Id = 150, Name = "we43tx34", Description = "3q4xtgf2qx4" },
                new() { Id = 200, Name = "rce6h756bj", Description = "67ib64vjh46" }
            };
            var categoriesQ = categories.AsQueryable();

            List<CategoryForListVM> categoryForListVMs = new()
            {
                new() { Id = 100, Name = "asfewg" },
                new() { Id = 150, Name = "we43tx34" },
                new() { Id = 200, Name = "rce6h756bj" }
            };
            var categoryForListVMsQ = categoryForListVMs.AsQueryable();

            PaginatedVM<CategoryForListVM> paginatedVM = new()
            {
                Items = categoryForListVMs,
                CurrentPage = 1,
                TotalPages = 1
            };

            CategoryListVM categoryListVM = new()
            {
                Categories = categoryForListVMs,
                CurrentPage = 1,
                TotalPages = 1,
            };

            _mapper.Setup(x => x.ConfigurationProvider).Returns(
                () => new MapperConfiguration(cfg => { cfg.CreateMap<Category, CategoryForListVM>(); }));
            _repository.Setup(s => s.GetCategories()).Returns(categoriesQ);
            _paginatorService.Setup(x => x.CreateAsync(It.IsAny<IQueryable<CategoryForListVM>>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(paginatedVM);
            _mapper.Setup(x => x.Map<CategoryListVM>(It.IsAny<PaginatedVM<CategoryForListVM>>())).Returns(categoryListVM);

            // Act
            var result = await _sut.GetPaginatedCategoriesAsync(10, 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(categoryListVM, result);
        }

        [Fact]
        public async Task UpdateCategoryAsync_RunsMethodsOnce()
        {
            // Arrange
            var imageByteArray = new byte[] { 2, 12, 4 };
            CategoryVM categoryVM = new() { Id = 100, Name = "asfewg", Description = "segsegrgerdhreh" };
            Category category = new() { Id = 100, Name = "asfewg", Description = "segsegrgerdhreh" };

            _mapper.Setup(s => s.Map<Category>(categoryVM)).Returns(category);

            _imageConverterService.Setup(s => s.GetByteArrayFromFormFileAsync(_formFile.Object)).ReturnsAsync(imageByteArray);

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
