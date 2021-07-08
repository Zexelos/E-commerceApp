using System.Linq;
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

        [HttpGet("Checkout/{customerId}")]
        [TypeFilter(typeof(CheckCheckoutGetPermission))]
        public async Task<IActionResult> Checkout([FromRoute] int customerId)
        {
            return Ok(await _orderService.GetOrderCheckoutVMAsync(customerId));
        }

        [HttpPost("Checkout")]
        [TypeFilter(typeof(CheckCheckoutPostPermission))]
        public async Task<IActionResult> Checkout([FromBody] OrderCheckoutVM orderCheckoutVM)
        {
            await _orderService.AddOrderAsync(orderCheckoutVM);
            return Ok();
        }

        [HttpGet("CustomerOrders")]
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
            var appUserId = User.Claims.First(x => x.Type == "UserId").Value;
            return Ok(await _orderService.GetPaginatedCustomerOrdersAsync(appUserId, intPageSize, pageNumber.Value));
        }

        [HttpGet("CustomerOrderDetails/{id}")]
        public async Task<IActionResult> CustomerOrderDetails([FromRoute] int id)
        {
            var appUserId = User.Claims.First(x => x.Type == "UserId").Value;
            return Ok(await _orderService.GetCustomerOrderDetailsAsync(appUserId, id));
        }
    }
}
