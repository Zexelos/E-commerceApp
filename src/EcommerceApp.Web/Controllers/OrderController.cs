using System;
using System.Diagnostics;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.Order;
using EcommerceApp.Web.Filters;
using EcommerceApp.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [TypeFilter(typeof(CheckCheckoutGetPermission))]
        public async Task<IActionResult> Checkout(int? customerId)
        {
            if (!customerId.HasValue)
            {
                return NotFound("You must pass a valid ID in the route");
            }
            var model = await _orderService.GetOrderCheckoutVMAsync(customerId.Value);
            return View(model);
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
