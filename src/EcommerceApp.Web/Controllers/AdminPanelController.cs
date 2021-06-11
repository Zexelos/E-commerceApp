using System.Data.Common;
using System.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EcommerceApp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels;

namespace EcommerceApp.Web.Controllers
{
    [Authorize("Admin")]
    public class AdminPanelController : Controller
    {
        private readonly ILogger<AdminPanelController> _logger;
        private readonly IEmployeeService _employeeService;
        private readonly ICustomerService _customerService;
        private readonly ISearchService _searchService;

        public AdminPanelController(ILogger<AdminPanelController> logger,
            IEmployeeService employeeService,
            ICustomerService customerService,
            ISearchService searchService)
        {
            _logger = logger;
            _employeeService = employeeService;
            _customerService = customerService;
            _searchService = searchService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Employees(string selectedValue, string searchString)
        {
            if (!string.IsNullOrEmpty(selectedValue) && !string.IsNullOrEmpty(searchString))
            {
                return View(await _searchService.EmployeeSearchAsync(selectedValue, searchString));
            }
            return View(await _employeeService.GetEmployeesAsync());
        }

        public async Task<IActionResult> Customers(string selectedValue, string searchString)
        {
            if (!string.IsNullOrEmpty(selectedValue) && !string.IsNullOrEmpty(searchString))
            {
                return View(await _searchService.CustomerSearchAsync(selectedValue, searchString));
            }
            return View(await _customerService.GetCustomersAsync());
        }

        [HttpGet]
        public IActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(EmployeeVM employeeVM)
        {
            if (ModelState.IsValid)
            {
                await _employeeService.AddEmployeeAsync(employeeVM);
                return RedirectToAction(nameof(Employees));
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateEmployee(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a valid ID in the route");
            }
            var model = await _employeeService.GetEmployeeAsync(id.Value);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEmployee(EmployeeVM employeeVM)
        {
            if (ModelState.IsValid)
            {
                await _employeeService.UpdateEmployeeAsync(employeeVM);
                return RedirectToAction(nameof(Employees));
            }
            else
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> DeleteEmployee(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a valid ID in the route");
            }
            await _employeeService.DeleteEmployeeAsync(id.Value);
            return RedirectToAction(nameof(Employees));
        }

        public async Task<IActionResult> DeleteCustomer(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a valid ID in the route");
            }
            await _customerService.DeleteCustomerAsync(id.Value);
            return RedirectToAction(nameof(Customers));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
