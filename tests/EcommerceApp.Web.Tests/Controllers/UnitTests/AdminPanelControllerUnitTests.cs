using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Microsoft.Extensions.Configuration;
using EcommerceApp.Application.ViewModels.AdminPanel;

namespace EcommerceApp.Web.Tests.Controllers.UnitTests
{
    public class AdminPanelControllerUnitTests
    {
        private readonly AdminPanelController _sut;
        private readonly Mock<ILogger<AdminPanelController>> _logger = new();
        private readonly Mock<IEmployeeService> _employeeService = new();
        private readonly Mock<ICustomerService> _customerService = new();
        private readonly Mock<ISearchService> _searchService = new();
        private readonly Mock<IConfiguration> _configuration = new();

        public AdminPanelControllerUnitTests()
        {
            _sut = new AdminPanelController(
                _logger.Object,
                _employeeService.Object,
                _customerService.Object,
                _searchService.Object,
                _configuration.Object
            );
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

        [Fact]
        public async Task Employees_ReturnsViewResultWithAllEmployees()
        {
            // Arrange
            var employeeListVM = new EmployeeListVM
            {
                Employees = new List<EmployeeForListVM>
                {
                    new EmployeeForListVM
                    {
                        FirstName = "ziutek",
                        LastName = "makowski"
                    }
                },
                CurrentPage = 1,
                TotalPages = 1
            };

            _employeeService.Setup(s => s.GetPaginatedEmployeesAsync(10, 1)).ReturnsAsync(employeeListVM);

            // Act
            var result = await _sut.Employees(string.Empty, string.Empty, "10", 1);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<EmployeeListVM>(viewResult.Model);
            Assert.Equal(employeeListVM.Employees[0].FirstName, model.Employees[0].FirstName);
            Assert.Equal(employeeListVM.Employees.Count, model.Employees.Count);
            Assert.Equal(employeeListVM.CurrentPage, model.CurrentPage);
        }

        [Fact]
        public async Task Employees_ReturnViewResultWithSearchedEmployees()
        {
            // Arrange
            var employeeListVM = new EmployeeListVM
            {
                Employees = new List<EmployeeForListVM>
                {
                    new EmployeeForListVM
                    {
                        FirstName = "maciek",
                        LastName = "ackowski"
                    }
                },
                CurrentPage = 1,
                TotalPages = 1
            };

            _searchService.Setup(s => s.SearchPaginatedEmployeesAsync("FirstName", "aciek", 10, 1)).ReturnsAsync(employeeListVM);

            // Act
            var result = await _sut.Employees("FirstName", "aciek", "10", 1);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<EmployeeListVM>(viewResult.Model);
            Assert.Equal(employeeListVM.Employees[0].FirstName, model.Employees[0].FirstName);
            Assert.Single(model.Employees);
            Assert.Equal(employeeListVM.CurrentPage, model.CurrentPage);
        }

        [Fact]
        public async Task Customers_ReturnsViewResultWithAllCustomers()
        {
            // Arrange
            var customerListVM = new CustomerListVM
            {
                Customers = new List<CustomerForListVM>
                {
                    new CustomerForListVM
                    {
                        FirstName = "ziutek",
                        LastName = "makowski"
                    }
                },
                CurrentPage = 1,
                TotalPages = 1
            };

            _customerService.Setup(s => s.GetPaginatedCustomersAsync(10, 1)).ReturnsAsync(customerListVM);

            // Act
            var result = await _sut.Customers(string.Empty, string.Empty, "10", 1);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CustomerListVM>(viewResult.Model);
            Assert.Equal(customerListVM.Customers[0].FirstName, model.Customers[0].FirstName);
            Assert.Equal(customerListVM.Customers.Count, model.Customers.Count);
            Assert.Equal(customerListVM.CurrentPage, model.CurrentPage);
        }

        [Fact]
        public async Task Customers_ReturnsViewResultWithSearchedCustomers()
        {
            // Arrange
            var customerListVM = new CustomerListVM
            {
                Customers = new List<CustomerForListVM>
                {
                    new CustomerForListVM
                    {
                        FirstName = "ziutek",
                        LastName = "makowski"
                    }
                },
                CurrentPage = 1,
                TotalPages = 1
            };

            _searchService.Setup(s => s.SearchPaginatedCustomersAsync("LastName", "utek", 10, 1)).ReturnsAsync(customerListVM);

            // Act
            var result = await _sut.Customers("LastName", "utek", "10", 1);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CustomerListVM>(viewResult.Model);
            Assert.Equal(customerListVM.Customers[0].FirstName, model.Customers[0].FirstName);
            Assert.Equal(customerListVM.Customers.Count, model.Customers.Count);
            Assert.Equal(customerListVM.CurrentPage, model.CurrentPage);
        }

        [Fact]
        public async Task CustomerDetails_ReturnsNotFoundResultWhenIdHasNoValue()
        {
            // Act
            var result = await _sut.CustomerDetails(null);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task CustomerDetails_ReturnsViewResultWhenIdHasValue()
        {
            // Arrange
            var customerDetailsVM = new CustomerDetailsVM
            {
                FirstName = "ziutek",
                CartItems = new List<CartItemForCustomerDetailsVM>
                {
                    new CartItemForCustomerDetailsVM
                    {
                        Name = "frytki"
                    }
                }
            };

            _customerService.Setup(s => s.GetCustomerDetailsAsync(1)).ReturnsAsync(customerDetailsVM);

            // Act
            var result = await _sut.CustomerDetails(1);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CustomerDetailsVM>(viewResult.Model);
            Assert.Single(model.CartItems);
            Assert.Equal(customerDetailsVM.FirstName, model.FirstName);
            Assert.Equal(customerDetailsVM.CartItems[0].Name, model.CartItems[0].Name);
        }

        [Fact]
        public void AddEmployee_GET_ReturnsViewResult()
        {
            // Act
            var result = _sut.AddEmployee();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task AddEmployee_POST_ReturnsRedirectToActionResultWhenModelStateIsValid()
        {
            // Arrange
            EmployeeVM employeeVM = new()
            {
                FirstName = "adsfwgge",
                LastName = "sdgwgerh"
            };

            // Act
            var result = await _sut.AddEmployee(employeeVM);

            // Assert
            _employeeService.Verify(s => s.AddEmployeeAsync(It.IsAny<EmployeeVM>()), Times.Once);
            Assert.NotNull(result);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Employees", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task AddEmployee_POST_ReturnsBadRequestResultWhenModelStateIsNotInvalid()
        {
            // Arrange
            EmployeeVM employeeVM = new()
            {
                FirstName = "adsfwgge",
                LastName = "sdgwgerh"
            };

            _sut.ModelState.AddModelError("error", "jakis error");

            // Act
            var result = await _sut.AddEmployee(employeeVM);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task UpdateEmployee_GET_ReturnsNotFoundResultWhenIdHasNoValue()
        {
            // Act
            var result = await _sut.UpdateEmployee(id: null);

            // Assert
            Assert.NotNull(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task UpdateEmployee_GET_ReturnsViewResultWhenIdHasValue()
        {
            // Arrange
            EmployeeVM employeeVM = new()
            {
                Id = 100,
                FirstName = "adsfwgge",
                LastName = "sdgwgerh"
            };

            _employeeService.Setup(s => s.GetEmployeeAsync(employeeVM.Id)).ReturnsAsync(employeeVM);

            // Act
            var result = await _sut.UpdateEmployee(employeeVM.Id);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<EmployeeVM>(viewResult.Model);
            Assert.Equal(model, employeeVM);
        }

        [Fact]
        public async Task UpdateEmployee_POST_ReturnsRedirectToActionResultWhenModelStateIsValid()
        {
            // Arrange
            EmployeeVM employeeVM = new()
            {
                Id = 100,
                FirstName = "adsfwgge",
                LastName = "sdgwgerh"
            };

            // Act
            var result = await _sut.UpdateEmployee(employeeVM);

            // Assert
            _employeeService.Verify(s => s.UpdateEmployeeAsync(employeeVM), Times.Once);
            Assert.NotNull(result);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Employees", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task UpdateEmployee_POST_ReturnsBadRequestResultWhenModelStateIsNotInvalid()
        {
            // Arrange
            EmployeeVM employeeVM = new()
            {
                Id = 100,
                FirstName = "adsfwgge",
                LastName = "sdgwgerh"
            };

            _sut.ModelState.AddModelError("error", "jakis error");

            // Act
            var result = await _sut.UpdateEmployee(employeeVM);

            // Assert
            Assert.NotNull(result);
            var badRequestResult = Assert.IsType<BadRequestResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task DeleteEmployee_ReturnsNotFoundResultWhenIdHasNoValue()
        {
            // Act
            var result = await _sut.DeleteEmployee(id: null);

            // Assert
            Assert.NotNull(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task DeleteEmployee_ReturnsRedirectToActionResultWhenIdHasValue()
        {
            // Arrange
            var id = 100;

            // Act
            var result = await _sut.DeleteEmployee(id);

            // Assert
            _employeeService.Verify(s => s.DeleteEmployeeAsync(It.IsAny<int>()), Times.Once);
            Assert.NotNull(result);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Employees", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task DeleteCustomer_ReturnsNotFoundResultWhenIdHasNoValue()
        {
            // Act
            var result = await _sut.DeleteCustomer(id: null);

            // Assert
            Assert.NotNull(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task DeleteCustomer_ReturnsRedirectToActionResultWhenIdHasValue()
        {
            // Arrange
            var id = 100;

            // Act
            var result = await _sut.DeleteCustomer(id);

            // Assert
            _customerService.Verify(v => v.DeleteCustomerAsync(It.IsAny<int>()), Times.Once);
            Assert.NotNull(result);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Customers", redirectToActionResult.ActionName);
        }
    }
}
