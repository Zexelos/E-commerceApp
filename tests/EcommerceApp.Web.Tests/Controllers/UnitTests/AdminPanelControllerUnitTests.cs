using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels;
using EcommerceApp.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EcommerceApp.Web.Tests
{
    public class AdminPanelControllerUnitTests
    {
        private readonly AdminPanelController _sut;
        private readonly Mock<ILogger<AdminPanelController>> _logger = new();
        private readonly Mock<IEmployeeService> _employeeService = new();
        private readonly Mock<ICustomerService> _customerService = new();
        private readonly Mock<ISearchService> _searchService = new();

        public AdminPanelControllerUnitTests()
        {
            _sut = new AdminPanelController(_logger.Object, _employeeService.Object, _customerService.Object, _searchService.Object);
        }

        [Fact]
        public async Task Index_ReturnViewResultWithAllEmployees()
        {
            // Arrange
            List<EmployeeVM> employeeVMs = new()
            {
                new EmployeeVM { FirstName = "wsfwaefg", LastName = "g43grea" },
                new EmployeeVM { FirstName = "gf34fz", LastName = "jkm8b7" },
            };

            _employeeService.Setup(s => s.GetEmployeesAsync()).ReturnsAsync(employeeVMs);

            // Act
            var result = await _sut.Employees(string.Empty, string.Empty);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<EmployeeVM>>(viewResult.Model);
            Assert.Equal(model[0].FirstName, employeeVMs[0].FirstName);
            Assert.Equal(model.Count, employeeVMs.Count);
        }

        [Fact]
        public async Task Index_ReturnViewResultWithSearchedEmployees()
        {
            // Arrange
            List<EmployeeVM> employeeVMs = new()
            {
                new EmployeeVM { FirstName = "Maciek", LastName = "g43grea" },
                new EmployeeVM { FirstName = "asdasaciekasd", LastName = "jkm8b7" },
            };

            _searchService.Setup(s => s.EmployeeSearchAsync("FirstName", "aciek")).ReturnsAsync(employeeVMs);

            // Act
            var result = await _sut.Employees("FirstName", "aciek");

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<EmployeeVM>>(viewResult.Model);
            Assert.Equal(employeeVMs[0].FirstName, model[0].FirstName);
            Assert.Equal(model.Count, employeeVMs.Count);
        }

        [Fact]
        public void AddEmployee_GET_ReturnCorrectViewResult()
        {
            // Act
            var result = _sut.AddEmployee();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task AddEmployee_POST_ReturnRedirectToActionResultWhenModelStateIsValid()
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
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task AddEmployee_POST_ReturnRedirectToActionResultWhenModelStateIsInvalid()
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
        public async Task UpdateEmployee_GET_ReturnNotFoundResultWhenIdIsNull()
        {
            // Act
            var result = await _sut.UpdateEmployee(id: null);

            // Assert
            Assert.NotNull(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task UpdateEmployee_GET_ReturnViewResultWhenIdHasValue()
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
        public async Task UpdateEmployee_POST_ReturnRedirectToActionResultWhenModelStateIsValid()
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
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task UpdateEmployee_POST_ReturnBadRequestResultWhenModelStateIsInvalid()
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
        public async Task DeleteEmployee_ReturnNotFoundResultWhenIdIsNull()
        {
            // Arrange

            // Act
            var result = await _sut.DeleteEmployee(id: null);

            // Assert
            Assert.NotNull(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task DeleteEmployee_ReturnRedirectToActionWhenIdHasValue()
        {
            // Arrange
            EmployeeVM employeeVM = new()
            {
                Id = 100,
                FirstName = "adsfwgge",
                LastName = "sdgwgerh"
            };

            // Act
            var result = await _sut.DeleteEmployee(employeeVM.Id);

            // Assert
            _employeeService.Verify(s => s.DeleteEmployeeAsync(employeeVM.Id), Times.Once);
            Assert.NotNull(result);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}
