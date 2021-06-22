using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.AdminPanel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EcommerceAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Admin")]
    public class AdminPanelController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ICustomerService _customerService;
        private readonly ISearchService _searchService;
        private readonly IConfiguration _configuration;

        public AdminPanelController(
            IEmployeeService employeeService,
            ISearchService searchService,
            IConfiguration configuration,
            ICustomerService customerService)
        {
            _employeeService = employeeService;
            _searchService = searchService;
            _configuration = configuration;
            _customerService = customerService;
        }

        [HttpGet("Employees")]
        public async Task<IActionResult> Employees(string selectedValue, string searchString, string pageSize, int? pageNumber)
        {
            if (!int.TryParse(pageSize, out int intPageSize))
            {
                intPageSize = _configuration.GetValue("DefaultPageSize", 10);
            }
            if (!pageNumber.HasValue)
            {
                pageNumber = 1;
            }
            if (!string.IsNullOrEmpty(selectedValue) && !string.IsNullOrEmpty(searchString))
            {
                return Ok(await _searchService.SearchPaginatedEmployeesAsync(selectedValue, searchString, intPageSize, pageNumber.Value));
            }
            return Ok(await _employeeService.GetPaginatedEmployeesAsync(intPageSize, pageNumber.Value));
        }

        [HttpGet("Customers")]
        public async Task<IActionResult> Customers(string selectedValue, string searchString, string pageSize, int? pageNumber)
        {
            if (!int.TryParse(pageSize, out int intPageSize))
            {
                intPageSize = _configuration.GetValue("DefaultPageSize", 10);
            }
            if (!pageNumber.HasValue)
            {
                pageNumber = 1;
            }
            if (!string.IsNullOrEmpty(selectedValue) && !string.IsNullOrEmpty(searchString))
            {
                return Ok(await _searchService.SearchPaginatedCustomersAsync(selectedValue, searchString, intPageSize, pageNumber.Value));
            }
            return Ok(await _customerService.GetPaginatedCustomersAsync(intPageSize, pageNumber.Value));
        }

        [HttpGet("CustomerDetails")]
        public async Task<IActionResult> CustomerDetails(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a valid ID in the route");
            }
            return Ok(await _customerService.GetCustomerDetailsAsync(id.Value));
        }

        [HttpPost("AddEmployee")]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeVM employeeVM)
        {
            if (ModelState.IsValid)
            {
                await _employeeService.AddEmployeeAsync(employeeVM);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeVM employeeVM)
        {
            if (ModelState.IsValid)
            {
                await _employeeService.UpdateEmployeeAsync(employeeVM);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployee(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a valid ID in the route");
            }
            await _employeeService.DeleteEmployeeAsync(id.Value);
            return Ok();
        }

        [HttpPost("DeleteCustomer")]
        public async Task<IActionResult> DeleteCustomer(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a valid ID in the route");
            }
            await _customerService.DeleteCustomerAsync(id.Value);
            return Ok();
        }
    }
}
