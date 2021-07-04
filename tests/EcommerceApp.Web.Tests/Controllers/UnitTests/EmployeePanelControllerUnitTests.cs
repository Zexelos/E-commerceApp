using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using EcommerceApp.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EcommerceApp.Web.Tests.Controllers.UnitTests
{
    public class EmployeePanelControllerUnitTests
    {
        private readonly EmployeePanelController _sut;
        private readonly Mock<ILogger<EmployeePanelController>> _logger = new();
        private readonly Mock<ICategoryService> _categoryService = new();
        private readonly Mock<IProductService> _productService = new();
        private readonly Mock<ISearchService> _searchService = new();
        private readonly Mock<IConfiguration> _configuration = new();
        private readonly Mock<IOrderService> _orderService = new();

        public EmployeePanelControllerUnitTests()
        {
            _sut = new EmployeePanelController(
                _logger.Object,
                _categoryService.Object,
                _productService.Object,
                _searchService.Object,
                _configuration.Object,
                _orderService.Object);
        }

        [Fact]
        public void Index_ReturnsViewResult()
        {
            // Act
            var result = _sut.Index();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        #region Categories
        [Fact]
        public async Task Categories_ReturnsViewResultWithAllCategories()
        {
            // Arrange
            var categoryListVM = new CategoryListVM
            {
                Categories = new List<CategoryForListVM>
                {
                    new CategoryForListVM
                    {
                        Name = "mleko"
                    }
                },
                CurrentPage = 1
            };

            _categoryService.Setup(s => s.GetPaginatedCategoriesAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(categoryListVM);

            // Act
            var result = await _sut.Categories(string.Empty, string.Empty, "10", 1);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CategoryListVM>(viewResult.ViewData.Model);
            Assert.Single(model.Categories);
            Assert.Equal(categoryListVM.CurrentPage, model.CurrentPage);
            Assert.Equal(categoryListVM.Categories[0].Name, model.Categories[0].Name);
        }

        [Fact]
        public async Task Categories_ReturnsViewResultWithSearchedCategories()
        {
            // Arrange
            var categoryListVM = new CategoryListVM
            {
                Categories = new List<CategoryForListVM>
                {
                    new CategoryForListVM
                    {
                        Name = "mleko"
                    }
                },
                CurrentPage = 1
            };

            _searchService.Setup(s =>
                s.SearchPaginatedCategoriesAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(categoryListVM);

            // Act
            var result = await _sut.Categories("Name", "eko", "10", 1);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CategoryListVM>(viewResult.ViewData.Model);
            Assert.Single(model.Categories);
            Assert.Equal(categoryListVM.CurrentPage, model.CurrentPage);
            Assert.Equal(categoryListVM.Categories[0].Name, model.Categories[0].Name);
        }
        #endregion

        #region Products
        [Fact]
        public async Task Products_ReturnsViewResultWithAllProducts()
        {
            // Arrange
            var productListVM = new ProductListVM
            {
                Products = new List<ProductForListVM>
                {
                    new ProductForListVM
                    {
                        Name = "mleko"
                    }
                },
                CurrentPage = 1
            };

            _productService.Setup(s => s.GetPaginatedProductsAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(productListVM);

            // Act
            var result = await _sut.Products(string.Empty, string.Empty, "10", 1);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductListVM>(viewResult.ViewData.Model);
            Assert.Single(model.Products);
            Assert.Equal(productListVM.CurrentPage, model.CurrentPage);
            Assert.Equal(productListVM.Products[0].Name, model.Products[0].Name);
        }

        [Fact]
        public async Task Products_ReturnsViewResultWithSearchedProducts()
        {
            // Arrange
            var productListVM = new ProductListVM
            {
                Products = new List<ProductForListVM>
                {
                    new ProductForListVM
                    {
                        Name = "mleko"
                    }
                },
                CurrentPage = 1
            };

            _searchService.Setup(s =>
                s.SearchPaginatedProductsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(productListVM);

            // Act
            var result = await _sut.Products("Name", "eko", "10", 1);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductListVM>(viewResult.ViewData.Model);
            Assert.Single(model.Products);
            Assert.Equal(productListVM.CurrentPage, model.CurrentPage);
            Assert.Equal(productListVM.Products[0].Name, model.Products[0].Name);
        }
        #endregion

        #region Orders
        [Fact]
        public async Task Orders_ReturnsViewResultWithAllOrders()
        {
            // Arrange
            var orderListVM = new OrderListVM
            {
                Orders = new List<OrderForListVM>
                {
                    new OrderForListVM
                    {
                        Price = 11m
                    }
                },
                CurrentPage = 1
            };

            _orderService.Setup(s => s.GetPaginatedOrdersAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(orderListVM);

            // Act
            var result = await _sut.Orders(string.Empty, string.Empty, "10", 1);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<OrderListVM>(viewResult.ViewData.Model);
            Assert.Single(model.Orders);
            Assert.Equal(orderListVM.CurrentPage, model.CurrentPage);
            Assert.Equal(orderListVM.Orders[0].Price, model.Orders[0].Price);
        }

        [Fact]
        public async Task Orders_ReturnsViewResultWithSearchedOrders()
        {
            // Arrange
            var orderListVM = new OrderListVM
            {
                Orders = new List<OrderForListVM>
                {
                    new OrderForListVM
                    {
                        Price = 11m
                    }
                },
                CurrentPage = 1
            };

            _searchService.Setup(s =>
                s.SearchPaginatedOrdersAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(orderListVM);

            // Act
            var result = await _sut.Orders("Price", "11", "10", 1);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<OrderListVM>(viewResult.ViewData.Model);
            Assert.Single(model.Orders);
            Assert.Equal(orderListVM.CurrentPage, model.CurrentPage);
            Assert.Equal(orderListVM.Orders[0].Price, model.Orders[0].Price);
        }
        #endregion

        #region OrderDetails
        [Fact]
        public async Task OrderDetails_ReturnsNotFoundResult()
        {
            // Act
            var result = await _sut.OrderDetails(id: null);

            // Assert
            Assert.NotNull(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task OrderDetails_ReturnsViewResult()
        {
            // Arrange
            var orderDetailsVM = new OrderDetailsVM
            {
                Id = 1,
                OrderItems = new List<OrderItemForDetailsVM>
                {
                    new OrderItemForDetailsVM
                    {
                        ProductName = "mleko"
                    }
                }
            };

            _orderService.Setup(s => s.GetOrderDetailsAsync(It.IsAny<int>())).ReturnsAsync(orderDetailsVM);

            // Act
            var result = await _sut.OrderDetails(1);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<OrderDetailsVM>(viewResult.Model);
            Assert.Single(model.OrderItems);
            Assert.Equal(orderDetailsVM.Id, model.Id);
            Assert.Equal(orderDetailsVM.OrderItems[0].ProductName, model.OrderItems[0].ProductName);
        }
        #endregion

        #region AddCategory, AddProduct _GET
        [Fact]
        public void AddCategory_GET_ReturnsViewResult()
        {
            // Act
            var result = _sut.AddCategory();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void AddProduct_GET_ReturnsViewResult()
        {
            // Act
            var result = _sut.AddProduct();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }
        #endregion

        #region AddCategory_POST
        [Fact]
        public async Task AddCategory_POST_ReturnsBadRequestResultWhenModelStateIsNotValid()
        {
            // Arrange
            var categoryVM = new CategoryVM();

            _sut.ModelState.AddModelError("error", "idk");

            // Act
            var result = await _sut.AddCategory(categoryVM);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task AddCategory_POST_ReturnsRedirectToActionWhenModelStateIsValid()
        {
            // Arrange
            var categoryVM = new CategoryVM { Id = 100, Name = "argrarg", Description = "sadwegwe" };

            // Act
            var result = await _sut.AddCategory(categoryVM);

            // Assert
            _categoryService.Verify(v => v.AddCategoryAsync(It.IsAny<CategoryVM>()), Times.Once);
            Assert.NotNull(result);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(_sut.Categories), redirectToActionResult.ActionName);
        }
        #endregion

        #region AddProduct_POST
        [Fact]
        public async Task AddProduct_POST_ReturnBadRequestResultWhenModelStateIsNotValid()
        {
            // Arrange
            var productVM1 = new ProductVM();

            _sut.ModelState.AddModelError("error", "idk");
            // Act
            var result = await _sut.AddProduct(productVM1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task AddProduct_POST_ReturnRedirectToActionResultWhenModelStateIsValid()
        {
            // Arrange
            var productVM = new ProductVM { Id = 100, Name = "argrarg", Description = "sadwegwe" };

            // Act
            var result = await _sut.AddProduct(productVM);

            // Assert
            _productService.Verify(v => v.AddProductAsync(It.IsAny<ProductVM>()), Times.Once);
            Assert.NotNull(result);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(_sut.Products), redirectToActionResult.ActionName);
        }
        #endregion

        #region UpdateCategory_GET
        [Fact]
        public async Task UpdateCategory_GET_ReturnsNotFoundResultWhenIdHasNoValue()
        {
            // Act
            var result = await _sut.UpdateCategory(id: null);

            // Assert
            Assert.NotNull(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task UpdateCategory_GET_ReturnsViewResultWhenIdHasValue()
        {
            // Arrange
            var categoryVM = new CategoryVM
            {
                Name = "nabial",
                Description = "dobnte"
            };

            _categoryService.Setup(s => s.GetCategoryAsync(It.IsAny<int>())).ReturnsAsync(categoryVM);

            // Act
            var result = await _sut.UpdateCategory(1);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CategoryVM>(viewResult.Model);
            Assert.Equal(categoryVM.Name, model.Name);
            Assert.Equal(categoryVM.Description, model.Description);
        }
        #endregion

        #region UpdateProduct_GET
        [Fact]
        public async Task UpdateProduct_GET_ReturnsNotFoundResultWhenIdHasNoValue()
        {
            // Act
            var result = await _sut.UpdateProduct(id: null);

            // Assert
            Assert.NotNull(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task UpdateProduct_GET_ReturnsViewResultWhenIdHasValue()
        {
            // Arrange
            var productVM = new ProductVM
            {
                Name = "nabial",
                Description = "dobnte"
            };

            _productService.Setup(s => s.GetProductAsync(It.IsAny<int>())).ReturnsAsync(productVM);

            // Act
            var result = await _sut.UpdateProduct(1);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductVM>(viewResult.Model);
            Assert.Equal(productVM.Name, model.Name);
            Assert.Equal(productVM.Description, model.Description);
        }
        #endregion

        #region UpdateCategory_POST
        [Fact]
        public async Task UpdateCategory_POST_ReturnsBadRequestResultWhenModelStateIsNotValid()
        {
            // Arrange
            var categoryVM = new CategoryVM();

            _sut.ModelState.AddModelError("error", "idk");

            // Act
            var result = await _sut.UpdateCategory(categoryVM);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task UpdateCategory_POST_ReturnsRedirectToActionWhenModelStateIsValid()
        {
            // Arrange
            var categoryVM = new CategoryVM { Id = 100, Name = "argrarg", Description = "sadwegwe" };

            // Act
            var result = await _sut.UpdateCategory(categoryVM);

            // Assert
            _categoryService.Verify(v => v.UpdateCategoryAsync(It.IsAny<CategoryVM>()), Times.Once);
            Assert.NotNull(result);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(_sut.Categories), redirectToActionResult.ActionName);
        }
        #endregion

        #region UpdateProduct_POST
        [Fact]
        public async Task UpdateProduct_POST_ReturnsBadRequestResultWhenModelStateIsNotValid()
        {
            // Arrange
            var productVM = new ProductVM { Name = "nabial", Description = "dobnte" };

            _sut.ModelState.AddModelError("error", "idk");

            // Act
            var result = await _sut.UpdateProduct(productVM);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task UpdateProduct_POST_ReturnsRedirectToActionWhenModelStateIsValid()
        {
            // Arrange
            var productVM = new ProductVM { Name = "nabial", Description = "dobnte" };

            // Act
            var result = await _sut.UpdateProduct(productVM);

            // Assert
            _productService.Verify(v => v.UpdateProductAsync(It.IsAny<ProductVM>()), Times.Once);
            Assert.NotNull(result);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(_sut.Products), redirectToActionResult.ActionName);
        }
        #endregion

        #region DeleteCategory
        [Fact]
        public async Task DeleteCategory_ReturnsNotFoundResultWhenIdHasNoValue()
        {
            // Act
            var result = await _sut.DeleteCategory(id: null);

            // Assert
            Assert.NotNull(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task DeleteCategory_ReturnsViewResultWhenIdHasValue()
        {
            // Act
            var result = await _sut.DeleteCategory(1);

            // Assert
            _categoryService.Verify(s => s.DeleteCategoryAsync(It.IsAny<int>()), Times.Once);
            Assert.NotNull(result);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(_sut.Categories), redirectToActionResult.ActionName);
        }
        #endregion

        #region DeleteProduct
        [Fact]
        public async Task DeleteProduct_ReturnsNotFoundResultWhenIdHasNoValue()
        {
            // Act
            var result = await _sut.DeleteProduct(id: null);

            // Assert
            Assert.NotNull(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsViewResultWhenIdHasValue()
        {
            // Act
            var result = await _sut.DeleteProduct(1);

            // Assert
            _productService.Verify(s => s.DeleteProductAsync(It.IsAny<int>()), Times.Once);
            Assert.NotNull(result);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(_sut.Products), redirectToActionResult.ActionName);
        }
        #endregion

        #region DeleteOrder
        [Fact]
        public async Task DeleteOrder_ReturnsNotFoundResultWhenIdHasNoValue()
        {
            // Act
            var result = await _sut.DeleteProduct(id: null);

            // Assert
            Assert.NotNull(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task DeleteOrder_ReturnsViewResultWhenIdHasValue()
        {
            // Act
            var result = await _sut.DeleteOrder(1);

            // Assert
            _orderService.Verify(s => s.DeleteOrderAsync(It.IsAny<int>()), Times.Once);
            Assert.NotNull(result);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(_sut.Orders), redirectToActionResult.ActionName);
        }
        #endregion
    }
}
