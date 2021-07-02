using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceApp.Application.Services;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Moq;
using Xunit;
using System.Linq;
using EcommerceApp.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using MockQueryable.Moq;
using EcommerceApp.Application.ViewModels.Product;
using EcommerceApp.Application.ViewModels;
using EcommerceApp.Application.ViewModels.Home;

namespace EcommerceApp.Application.Tests.Services.UnitTests
{
    public class ProductServiceUnitTests
    {
        private readonly ProductService _sut;
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<IProductRepository> _repository = new();
        private readonly Mock<IImageConverterService> _imageConverterService = new();
        private readonly Mock<IFormFile> _formFile = new();
        private readonly Mock<IPaginatorService<ProductForListVM>> _paginatorService = new();

        public ProductServiceUnitTests()
        {
            _sut = new ProductService(
                _mapper.Object,
                _repository.Object,
                _imageConverterService.Object,
                _paginatorService.Object
            );
        }

        [Fact]
        public async Task AddProductAsync_MethodsRunOnce()
        {
            // Arrange
            var productVM = new ProductVM
            {
                Id = 100,
                Name = "Mleko",
                Description = "erbt5gh35hh",
                UnitPrice = 12.32m,
                UnitsInStock = 2,
                ImageToDisplay = "edty345fty45fw53"
            };
            var product = new Product
            {
                Id = 100,
                Name = "Mleko",
                Description = "erbt5gh35hh",
                UnitPrice = 12.32m,
                UnitsInStock = 2,
                Image = new byte[] { 1, 15 }
            };

            _mapper.Setup(s => s.Map<Product>(productVM)).Returns(product);

            _imageConverterService.Setup(s => s.GetByteArrayFromFormFileAsync(_formFile.Object)).ReturnsAsync(product.Image);

            // Act
            await _sut.AddProductAsync(productVM);

            // Assert
            _repository.Verify(v => v.AddProductAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task GetProductAsync_ReturnsProductVM()
        {
            // Arrange
            var productVM = new ProductVM { Id = 100, Name = "Mleko", Description = "erbt5gh35hh", UnitPrice = 12.32m, UnitsInStock = 2, ImageToDisplay = "swegwe" };
            var product = new Product { Id = 100, Name = "Mleko", Description = "erbt5gh35hh", UnitPrice = 12.32m, UnitsInStock = 2, Image = new byte[] { 1, 15 } };
            var products = new List<Product> { product };
            var productsQ = products.AsQueryable().BuildMock();

            _repository.Setup(s => s.GetProducts()).Returns(productsQ.Object);

            _mapper.Setup(s => s.Map<ProductVM>(It.IsAny<Product>())).Returns(productVM);

            _imageConverterService.Setup(s => s.GetImageStringFromByteArray(product.Image)).Returns(productVM.ImageToDisplay);

            // Act
            var result = await _sut.GetProductAsync(productVM.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productVM, result);
        }

        [Fact]
        public async Task GetProductDetailsForUserAsync_ReturnsProductDetailsForUserVM()
        {
            var product = new Product { Id = 100, Name = "Mleko", Description = "354vsd", UnitPrice = 12.32m, UnitsInStock = 2, Image = new byte[] { 1, 15 } };
            var products = new List<Product> { product };
            var productDetailsForUserVMs = new ProductDetailsForUserVM { Id = 100, Name = "Mleko", Description = "erbt5gh35hh", UnitPrice = 12.32m, UnitsInStock = 2, ImageToDisplay = "245dfs" };

            // Arrange
            _repository.Setup(s => s.GetProductAsync(It.IsAny<int>())).ReturnsAsync(product);

            _mapper.Setup(s => s.Map<ProductDetailsForUserVM>(It.IsAny<Product>())).Returns(productDetailsForUserVMs);

            _imageConverterService.Setup(s => s.GetImageStringFromByteArray(It.IsAny<byte[]>())).Returns(productDetailsForUserVMs.ImageToDisplay);

            // Act
            var result = await _sut.GetProductDetailsForUserAsync(product.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productDetailsForUserVMs, result);
        }

        [Fact]
        public async Task GetPaginatedProductsAsync_ReturnsProductListVM()
        {
            // Arrange
            var product = new Product { Id = 10, Name = "Frytki" };
            var products = new List<Product> { product };
            var productsQ = products.AsQueryable().BuildMock();

            var productForListVM = new ProductForListVM
            {
                Id = product.Id,
                Name = product.Name
            };
            var productForListVMs = new List<ProductForListVM> { productForListVM };

            var paginatedVM = new PaginatedVM<ProductForListVM>
            {
                Items = productForListVMs,
                CurrentPage = 1,
                TotalPages = 2,
            };

            var productListVM = new ProductListVM
            {
                Products = paginatedVM.Items,
                CurrentPage = paginatedVM.CurrentPage,
                TotalPages = paginatedVM.TotalPages
            };

            _repository.Setup(s => s.GetProducts()).Returns(productsQ.Object);

            _mapper.Setup(s => s.ConfigurationProvider).Returns(
                new MapperConfiguration(cfg => { cfg.CreateMap<Product, ProductForListVM>(); })
            );

            _paginatorService.Setup(s => s.CreateAsync(It.IsAny<IQueryable<ProductForListVM>>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(paginatedVM);

            _mapper.Setup(s => s.Map<ProductListVM>(It.IsAny<PaginatedVM<ProductForListVM>>())).Returns(productListVM);

            // Act
            var result = await _sut.GetPaginatedProductsAsync(10, 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productListVM, result);
        }

        [Fact]
        public async Task GetRandomProductsWithImageAsync_ReturnsHomeVM()
        {
            // Ararnge
            var product = new Product { Id = 10, Name = "Frytki" };
            var products = new List<Product> { product };
            var productsQ = products.AsQueryable().BuildMock();

            var productDetailsForHomeVM = new ProductDetailsForHomeVM
            {
                Id = product.Id,
                Name = product.Name,
                ImageToDisplay = "dqtw34dtw345"
            };
            var productDetailsForHomeVMs = new List<ProductDetailsForHomeVM> { productDetailsForHomeVM };

            var homeVM = new HomeVM
            {
                Products = productDetailsForHomeVMs
            };

            _repository.Setup(s => s.GetProducts()).Returns(productsQ.Object);

            _mapper.Setup(s => s.ConfigurationProvider).Returns(
                new MapperConfiguration(cfg => { cfg.CreateMap<Product, ProductDetailsForHomeVM>(); })
            );

            _imageConverterService.Setup(s => s.GetImageStringFromByteArray(It.IsAny<byte[]>())).Returns(productDetailsForHomeVM.ImageToDisplay);

            // Act
            var result = await _sut.GetRandomProductsWithImageAsync(8);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(homeVM.Products[0].ImageToDisplay, result.Products[0].ImageToDisplay);
        }

        [Fact]
        public async Task GetProductsByCategoryIdAsync_ReturnsListProductDetailsForUserVM()
        {
            // Ararnge
            var caregoryId = 1;
            var category = new Category { Id = caregoryId };
            var product1 = new Product { Category = category, CategoryId = caregoryId, Name = "frytjki" };
            var product2 = new Product { Category = category, CategoryId = caregoryId, Name = "fy34564635" };
            var product3 = new Product { Category = category, CategoryId = caregoryId, Name = "frytehrtvvehrtjki" };
            var products = new List<Product> { product1, product2, product3 };
            var productsQ = products.AsQueryable().BuildMock();

            var productDetailsForUserVM1 = new ProductDetailsForUserVM { Name = product1.Name, ImageToDisplay = "degrsccgescegs4" };
            var productDetailsForUserVM2 = new ProductDetailsForUserVM { Name = product2.Name, ImageToDisplay = "degrsccgescegs4" };
            var productDetailsForUserVM3 = new ProductDetailsForUserVM { Name = product3.Name, ImageToDisplay = "degrsccgescegs4" };
            var productDetailsForUserVMs = new List<ProductDetailsForUserVM> { productDetailsForUserVM1, productDetailsForUserVM2, productDetailsForUserVM3 };

            var listProductDetailsForUserVM = new ListProductDetailsForUserVM { Products = productDetailsForUserVMs };

            _repository.Setup(s => s.GetProducts()).Returns(productsQ.Object);

            _mapper.Setup(s => s.Map<List<ProductDetailsForUserVM>>(It.IsAny<List<Product>>())).Returns(productDetailsForUserVMs);

            _imageConverterService.Setup(s => s.GetImageStringFromByteArray(It.IsAny<byte[]>())).Returns(productDetailsForUserVM1.ImageToDisplay);

            // Act
            var result = await _sut.GetProductsByCategoryIdAsync(caregoryId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(listProductDetailsForUserVM.Products[0].Name, result.Products[0].Name);
            Assert.Equal(listProductDetailsForUserVM.Products[2].ImageToDisplay, result.Products[2].ImageToDisplay);
        }

        [Fact]
        public async Task UpdateProductAsync_MethodsRunOnce()
        {
            // Ararnge
            var productVM = new ProductVM { Id = 100, Name = "Mleko", Description = "erbt5gh35hh", UnitPrice = 12.32m, UnitsInStock = 2 };
            var product = new Product { Id = 100, Name = "Mleko", Description = "erbt5gh35hh", UnitPrice = 12.32m, UnitsInStock = 2, Image = new byte[] { 1, 15 } };
            var category = new Category { Id = 1, Name = "Pieczywo", Description = "dobre" };

            _mapper.Setup(s => s.Map<Product>(It.IsAny<ProductVM>())).Returns(product);

            _imageConverterService.Setup(s => s.GetByteArrayFromFormFileAsync(_formFile.Object)).ReturnsAsync(product.Image);

            // Act
            await _sut.UpdateProductAsync(productVM);

            // Assert
            _repository.Verify(v => v.UpdateProductAsync(product), Times.Once);
        }

        [Fact]
        public async Task DeleteProductAsync_MethodsRunOnce()
        {
            // Arrange
            int id = 100;

            // Act
            await _sut.DeleteProductAsync(id);

            // Assert
            _repository.Verify(v => v.DeleteProductAsync(id), Times.Once);
        }
    }
}
