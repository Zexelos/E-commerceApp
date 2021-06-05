using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceApp.Application.Services;
using EcommerceApp.Application.ViewModels;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Moq;
using Xunit;
using System.Linq;
using EcommerceApp.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace EcommerceApp.Application
{
    public class ProductServiceUnitTests
    {
        private readonly ProductService _sut;
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<IProductRepository> _productRepository = new();
        private readonly Mock<ICategoryRepository> _categoryRepository = new();
        private readonly Mock<IImageConverterService> _imageConverterService = new();
        private readonly Mock<IFormFile> _formFile = new();

        public ProductServiceUnitTests()
        {
            _sut = new ProductService(_mapper.Object, _productRepository.Object, _categoryRepository.Object, _imageConverterService.Object);
        }

        [Fact]
        public async Task AddProductAsync_MethodsRunOnce()
        {
            // Arrange
            var productVM = new ProductVM { Id = 100, Name = "Mleko", Description = "erbt5gh35hh", UnitPrice = 12.32m, UnitsInStock = 2, Image = new byte[] { 1, 15 } };
            var product = new Product { Id = 100, Name = "Mleko", Description = "erbt5gh35hh", UnitPrice = 12.32m, UnitsInStock = 2 };
            var category = new Category { Id = 1, Name = "Pieczywo", Description = "dobre" };

            _mapper.Setup(s => s.Map<Product>(productVM)).Returns(product);

            _categoryRepository.Setup(s => s.GetCategoryAsync(product.CategoryName)).ReturnsAsync(category);

            _imageConverterService.Setup(s => s.GetByteArrayFromImage(_formFile.Object)).ReturnsAsync(productVM.Image);

            // Act
            await _sut.AddProductAsync(productVM);

            // Assert
            _productRepository.Verify(v => v.AddProductAsync(product), Times.Once);
        }

        [Fact]
        public async Task GetProductAsync_ReturnProduct()
        {
            // Arrange
            var productVM = new ProductVM { Id = 100, Name = "Mleko", Description = "erbt5gh35hh", UnitPrice = 12.32m, UnitsInStock = 2, Image = new byte[] { 1, 15 }, ImageToDisplay = "swegwe" };
            var product = new Product { Id = 100, Name = "Mleko", Description = "erbt5gh35hh", UnitPrice = 12.32m, UnitsInStock = 2 };

            _productRepository.Setup(s => s.GetProductAsync(productVM.Id)).ReturnsAsync(product);

            _mapper.Setup(s => s.Map<ProductVM>(product)).Returns(productVM);

            _imageConverterService.Setup(s => s.GetImageStringFromByteArray(productVM.Image)).Returns(productVM.ImageToDisplay);

            // Act
            var result = await _sut.GetProductAsync(productVM.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productVM, result);
        }

        [Fact]
        public async Task GetProductsAsync_ReturnListOfProducts()
        {
            var products = new List<Product>
            {
                new Product { Id = 100, Name = "Mleko", Description = "erbt5gh35hh", UnitPrice = 12.32m, UnitsInStock = 2 },
                new Product { Id = 150, Name = "Ziemniaki", Description = "23f", UnitPrice = 4.32m, UnitsInStock = 1 },
                new Product { Id = 200, Name = "Kasza", Description = "243", UnitPrice = 12.7m, UnitsInStock = 5 }
            };

            var productVMs = new List<ProductVM>
            {
                new ProductVM { Id = 100, Name = "Mleko", Description = "erbt5gh35hh", UnitPrice = 12.32m, UnitsInStock = 2 },
                new ProductVM { Id = 150, Name = "Ziemniaki", Description = "23f", UnitPrice = 4.32m, UnitsInStock = 1 },
                new ProductVM { Id = 200, Name = "Kasza", Description = "243", UnitPrice = 12.7m, UnitsInStock = 5 }
            };

            // Arrange
            _productRepository.Setup(s => s.GetProductsAsync()).ReturnsAsync(products.AsQueryable());

            _mapper.Setup(s => s.Map<List<ProductVM>>(products)).Returns(productVMs);

            // Act
            var result = await _sut.GetProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productVMs, result);
        }

        [Fact]
        public async Task UpdateProductAsync_MethodsRunOnce()
        {
            // Ararnge
            var productVM = new ProductVM { Id = 100, Name = "Mleko", Description = "erbt5gh35hh", UnitPrice = 12.32m, UnitsInStock = 2, Image = new byte[] { 1, 15 } };
            var product = new Product { Id = 100, Name = "Mleko", Description = "erbt5gh35hh", UnitPrice = 12.32m, UnitsInStock = 2 };
            var category = new Category { Id = 1, Name = "Pieczywo", Description = "dobre" };

            _mapper.Setup(s => s.Map<Product>(productVM)).Returns(product);

            _categoryRepository.Setup(s => s.GetCategoryAsync(product.CategoryName)).ReturnsAsync(category);

            _imageConverterService.Setup(s => s.GetByteArrayFromImage(_formFile.Object)).ReturnsAsync(productVM.Image);

            // Act
            await _sut.UpdateProductAsync(productVM);

            // Assert
            _productRepository.Verify(v => v.UpdateProductAsync(product), Times.Once);
        }

        [Fact]
        public async Task DeleteProductAsync_MethodsRunOnce()
        {
            // Arrange
            int id = 100;

            // Act
            await _sut.DeleteProductAsync(id);

            // Assert
            _productRepository.Verify(v => v.DeleteProductAsync(id), Times.Once);
        }
    }
}
