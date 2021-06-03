using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceApp.Application.Services;
using EcommerceApp.Application.ViewModels;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Moq;
using Xunit;
using System.Linq;

namespace EcommerceApp.Application
{
    public class ProductServiceUnitTests
    {
        private readonly ProductService _sut;
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<IProductRepository> _productRepository = new();
        private readonly Mock<ICategoryRepository> _categoryRepository = new();

        public ProductServiceUnitTests()
        {
            _sut = new ProductService(_mapper.Object, _productRepository.Object, _categoryRepository.Object);
        }

        [Fact]
        public async Task AddProductAsync_MethodsRunOnce()
        {
            // Arrange
            var productVM = new ProductVM { Id = 100, Name = "Mleko", Description = "erbt5gh35hh", UnitPrice = 12.32m, UnitsInStock = 2 };
            var product = new Product { Id = 100, Name = "Mleko", Description = "erbt5gh35hh", UnitPrice = 12.32m, UnitsInStock = 2 };

            _mapper.Setup(s => s.Map<Product>(productVM)).Returns(product);

            // Act
            await _sut.AddProductAsync(productVM);

            // Assert
            _productRepository.Verify(v => v.AddProductAsync(product), Times.Once);
        }

        [Fact]
        public async Task GetProductAsync_ReturnProduct()
        {
            // Arrange
            var productVM = new ProductVM { Id = 100, Name = "Mleko", Description = "erbt5gh35hh", UnitPrice = 12.32m, UnitsInStock = 2 };
            var product = new Product { Id = 100, Name = "Mleko", Description = "erbt5gh35hh", UnitPrice = 12.32m, UnitsInStock = 2 };

            var category1 = new Category { Id = 100, Name = "sgfawer", Description = "wsgerwweg" };
            var category2 = new Category { Id = 150, Name = "ergf34", Description = "56vu68" };
            var category3 = new Category { Id = 200, Name = "eargg3", Description = "hqb2424" };

            var categories = new List<Category> { category1, category2, category3 };

            var categoriesVM1 = new CategoriesVM { Id = 100, Name = "sgfawer" };
            var categoriesVM2 = new CategoriesVM { Id = 150, Name = "ergf34" };
            var categoriesVM3 = new CategoriesVM { Id = 200, Name = "eargg3" };

            var categoriesVM = new List<CategoriesVM> { categoriesVM1, categoriesVM2, categoriesVM3 };

            var productVMWithCategoriesList = new ProductVM { Id = 100, Name = "Mleko", Description = "erbt5gh35hh", UnitPrice = 12.32m, UnitsInStock = 2, Categories = categoriesVM };

            _productRepository.Setup(s => s.GetProductAsync(product.Id)).ReturnsAsync(product);

            _categoryRepository.Setup(s => s.GetCategoriesAsync()).ReturnsAsync(categories.AsQueryable());

            _mapper.Setup(s => s.Map<List<CategoriesVM>>(categories)).Returns(categoriesVM);

            _mapper.Setup(s => s.Map<ProductVM>(product)).Returns(productVM);

            // Act
            var result = await _sut.GetProductAsync(product.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productVMWithCategoriesList.Id, result.Id);
            Assert.Equal(productVMWithCategoriesList.Categories, result.Categories);
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
            var productVM = new ProductVM { Id = 100, Name = "Mleko", Description = "erbt5gh35hh", UnitPrice = 12.32m, UnitsInStock = 2 };
            var product = new Product { Id = 100, Name = "Mleko", Description = "erbt5gh35hh", UnitPrice = 12.32m, UnitsInStock = 2 };

            _mapper.Setup(s => s.Map<Product>(productVM)).Returns(product);

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
