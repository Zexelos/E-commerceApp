using System;
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
        private readonly Mock<IEmployeeService> _service = new();

        public AdminPanelControllerUnitTests()
        {
            _sut = new AdminPanelController(_logger.Object, _service.Object);
        }

        [Fact]
        public async Task Index_ReturnCorrectViewResult()
        {
            // Arrange
            List<EmployeeVM> employeeVMs = new()
            {
                new EmployeeVM { FirstName = "wsfwaefg", LastName = "g43grea" },
                new EmployeeVM { FirstName = "gf34fz", LastName = "jkm8b7" },
            };

            _service.Setup(s => s.GetEmployeesAsync()).ReturnsAsync(employeeVMs);

            // Act
            var result = await _sut.Index();

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<EmployeeVM>>(viewResult.Model);
            Assert.Equal(model[0].FirstName, employeeVMs[0].FirstName);
            Assert.Equal(model.Count, employeeVMs.Count);
        }

        [Fact]
        public void AddEmployee_GET_ReturnCorrectViewResult()
        {
            // Arrange

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
            _service.Verify(s => s.AddEmployeeAsync(It.IsAny<EmployeeVM>()), Times.Once);
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
            // Arrange

            // Act
            var result = await _sut.UpdateEmployee(id: null);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task UpdateEmployee_GET_ReturnNotFoundResultWhenIdHasValue()
        {
            // Arrange
            EmployeeVM employeeVM = new()
            {
                Id = 100,
                FirstName = "adsfwgge",
                LastName = "sdgwgerh"
            };

            _service.Setup(s => s.GetEmployeeAsync(employeeVM.Id)).ReturnsAsync(employeeVM);

            // Act
            var result = await _sut.UpdateEmployee(employeeVM.Id);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<EmployeeVM>(viewResult.Model);
            Assert.Equal(model, employeeVM);
        }

        [Fact]
        public async Task DeleteEmployee_ReturnNotFoundResultWhenIdIsNull()
        {
            // Arrange

            // Act
            var result = await _sut.DeleteEmployee(id: null);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundObjectResult>(result);
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
            _service.Verify(s => s.DeleteEmployeeAsync(employeeVM.Id), Times.Once);
            Assert.NotNull(result);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}
