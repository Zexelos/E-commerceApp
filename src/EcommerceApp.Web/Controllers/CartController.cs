using System.Security.Claims;
using System.Diagnostics;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Domain.Models;
using EcommerceApp.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EcommerceApp.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;
        private readonly ICartItemService _cartItemService;

        public CartController(
            ILogger<CartController> logger,
            ICartItemService cartItemService)
        {
            _logger = logger;
            _cartItemService = cartItemService;
        }

        public async Task<IActionResult> AddToCart(int id, int quantity)
        {
            var appUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await _cartItemService.AddCartItem(id, quantity, appUserId);
            return Redirect("/Home/Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
