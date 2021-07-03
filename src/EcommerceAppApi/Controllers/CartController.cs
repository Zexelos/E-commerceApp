using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;

        public CartController(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var userId = User.Claims.First(x => x.Type == "UserId").Value;
            var model = await _cartItemService.GetCartItemListAsync(userId);
            return Ok(model);
        }

        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart(int id, int quantity)
        {
            var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _cartItemService.AddCartItem(id, quantity, appUserId);
            return Ok();
        }

        [HttpPut("IncreaseCartItemQuantityByOne")]
        public async Task<IActionResult> IncreaseCartItemQuantityByOne(int cartItemId)
        {
            await _cartItemService.IncreaseCartItemQuantityByOneAsync(cartItemId);
            return Ok();
        }

        [HttpPut("DecreaseCartItemQuantityByOne")]
        public async Task<IActionResult> DecreaseCartItemQuantityByOne(int cartItemId)
        {
            await _cartItemService.DecreaseCartItemQuantityByOneAsync(cartItemId);
            return Ok();
        }

        [HttpDelete("DeleteCartItem")]
        public async Task<IActionResult> DeleteCartItem(int cartItemId)
        {
            await _cartItemService.DeleteCartItemAsync(cartItemId);
            return Ok();
        }
    }
}
