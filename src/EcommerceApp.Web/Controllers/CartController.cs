using System.Security.Claims;
using System.Diagnostics;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace EcommerceApp.Web.Controllers
{
    [Authorize]
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

        public async Task<IActionResult> Index()
        {
            var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = await _cartItemService.GetCartItemListAsync(appUserId);
            return View(model);
        }

        public async Task<IActionResult> AddToCart(int id, int quantity)
        {
            var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _cartItemService.AddCartItem(id, quantity, appUserId);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> IncreaseCartItemQuantityByOne(int? cartItemId)
        {
            if(!cartItemId.HasValue)
            {
                return NotFound("You must pass a valid ID in the route");
            }
            await _cartItemService.IncreaseCartItemQuantityByOneAsync(cartItemId.Value);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DecreaseCartItemQuantityByOne(int? cartItemId)
        {
            if(!cartItemId.HasValue)
            {
                return NotFound("You must pass a valid ID in the route");
            }
            await _cartItemService.DecreaseCartItemQuantityByOneAsync(cartItemId.Value);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteCartItem(int? cartItemId)
        {
            if(!cartItemId.HasValue)
            {
                return NotFound("You must pass a valid ID in the route");
            }
            await _cartItemService.DeleteCartItemAsync(cartItemId.Value);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
