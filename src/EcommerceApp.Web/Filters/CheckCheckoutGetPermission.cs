using System;
using System.Security.Claims;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EcommerceApp.Web.Filters
{
    public class CheckCheckoutGetPermission : Attribute, IAsyncAuthorizationFilter
    {
        private readonly ICartItemService _cartItemService;

        public CheckCheckoutGetPermission(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var appUserId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var cartId = context.HttpContext.Request.Query["cartId"].ToString();

            bool intParse = int.TryParse(cartId, out int parsedCartId);
            var getCartId = await _cartItemService.GetCartIdByAppUserIdAsync(appUserId);

            if (intParse != true)
            {
                context.Result = new BadRequestResult();
            }

            if (getCartId != parsedCartId && intParse == true)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
