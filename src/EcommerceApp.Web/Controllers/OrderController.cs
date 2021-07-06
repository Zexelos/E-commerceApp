using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.Order;
using EcommerceApp.Web.Filters;
using EcommerceApp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EcommerceApp.Web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IConfiguration _configuration;

        public OrderController(
            IOrderService orderService,
            IConfiguration configuration)
        {
            _orderService = orderService;
            _configuration = configuration;
        }

        [HttpGet]
        [TypeFilter(typeof(CheckCheckoutGetPermission))]
        public async Task<IActionResult> Checkout(int? customerId)
        {
            if (!customerId.HasValue)
            {
                return NotFound("You must pass a valid ID in the route");
            }
            return View(await _orderService.GetOrderCheckoutVMAsync(customerId.Value));
        }

        [HttpPost]
        [TypeFilter(typeof(CheckCheckoutPostPermission))]
        public async Task<IActionResult> Checkout(OrderCheckoutVM orderCheckoutVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _orderService.AddOrderAsync(orderCheckoutVM);
            return RedirectToAction(nameof(CheckoutSuccessful));
        }

        public async Task<IActionResult> CustomerOrders(string pageSize, int? pageNumber)
        {
            if (!int.TryParse(pageSize, out int intPageSize))
            {
                intPageSize = _configuration.GetValue("DefaultPageSize", 10);
            }
            if (!pageNumber.HasValue)
            {
                pageNumber = 1;
            }
            var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(await _orderService.GetPaginatedCustomerOrdersAsync(appUserId, intPageSize, pageNumber.Value));
        }

        public async Task<IActionResult> CustomerOrderDetails(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a valid ID in the route");
            }
            var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(await _orderService.GetCustomerOrderDetailsAsync(appUserId, id.Value));
        }

        public IActionResult CheckoutSuccessful()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
