using System.Security.Claims;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.Order;
using EcommerceAppApi.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EcommerceAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
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

        [HttpGet("Checkout")]
        [TypeFilter(typeof(CheckCheckoutGetPermission))]
        public async Task<IActionResult> Checkout(int? customerId)
        {
            if (!customerId.HasValue)
            {
                return NotFound("You must pass a valid ID in the route");
            }
            return Ok(await _orderService.GetOrderCheckoutVMAsync(customerId.Value));
        }

        [HttpPost("Checkout")]
        [TypeFilter(typeof(CheckCheckoutPostPermission))]
        public async Task<IActionResult> Checkout([FromBody] OrderCheckoutVM orderCheckoutVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _orderService.AddOrderAsync(orderCheckoutVM);
            return Ok();
        }

        [HttpPost("CustomerOrders")]
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
            return Ok(await _orderService.GetPaginatedCustomerOrdersAsync(appUserId, intPageSize, pageNumber.Value));
        }

        [HttpPost("CustomerOrderDetails")]
        public async Task<IActionResult> CustomerOrderDetails(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("You must pass a valid ID in the route");
            }
            var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Ok(await _orderService.GetCustomerOrderDetailsAsync(appUserId, id.Value));
        }
    }
}
