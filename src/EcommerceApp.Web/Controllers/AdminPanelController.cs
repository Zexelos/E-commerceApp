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
        private readonly ILogger<HomeController> _logger;
        private readonly IEmployeeService _employeeService;

        public AdminPanelController(ILogger<HomeController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _employeeService.GetEmployeesAsync();
            return View(model);
        }

        [HttpGet]
        public IActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(EmployeeVM employeeVM)
        {
            await _employeeService.AddEmployeeAsync(employeeVM);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateEmployee(int id)
        {
            var model = await _employeeService.GetEmployeeAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEmployee(EmployeeVM employeeVM)
        {
            await _employeeService.UpdateEmployeeAsync(employeeVM);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteEmployee(int id)
        {
            await _employeeService.DeleteEmployeeAsync(id);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
