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
            if (ModelState.IsValid)
            {
                await _employeeService.AddEmployeeAsync(employeeVM);
                return RedirectToAction("Index");
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
                return NotFound("You must pass a valid Employee ID in the route, for example, /AdminPanel/EditEmployee/21");
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
                return RedirectToAction("Index");
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
                return NotFound("You must pass a valid Employee ID in the route, for example, /AdminPanel/EditEmployee/21");
            }
            await _employeeService.DeleteEmployeeAsync(id.Value);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
