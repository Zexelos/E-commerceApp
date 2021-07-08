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
        public async Task<IActionResult> AddToCart([FromQuery] int? id, [FromQuery] int? quantity)
        {
            if (!id.HasValue || !quantity.HasValue)
            {
                return NotFound("You must pass a valid ID and quantity in the route");
            }
            var appUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _cartItemService.AddCartItem(id.Value, quantity.Value, appUserId);
            return Ok();
        }

        [HttpPut("IncreaseCartItemQuantityByOne/{cartItemId}")]
        public async Task<IActionResult> IncreaseCartItemQuantityByOne([FromRoute] int cartItemId)
        {
            await _cartItemService.IncreaseCartItemQuantityByOneAsync(cartItemId);
            return Ok();
        }

        [HttpPut("DecreaseCartItemQuantityByOne/{cartItemId}")]
        public async Task<IActionResult> DecreaseCartItemQuantityByOne([FromRoute] int cartItemId)
        {
            await _cartItemService.DecreaseCartItemQuantityByOneAsync(cartItemId);
            return Ok();
        }

        [HttpDelete("DeleteCartItem/{cartItemId}")]
        public async Task<IActionResult> DeleteCartItem([FromRoute] int cartItemId)
        {
            await _cartItemService.DeleteCartItemAsync(cartItemId);
            return Ok();
        }
    }
}
