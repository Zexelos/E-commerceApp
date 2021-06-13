using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Domain.Models;
using EcommerceApp.Domain.Interfaces;

namespace EcommerceApp.Application.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ICustomerRepository _customerRepository;

        public CartItemService(
            ICartItemRepository cartItemRepository,
            IProductRepository productRepository,
            ICartRepository cartRepository,
            ICustomerRepository customerRepository)
        {
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
            _cartRepository = cartRepository;
            _customerRepository = customerRepository;
        }

        public async Task AddCartItem(int productId, int quantity, string appUserId)
        {
            var product = await _productRepository.GetProductAsync(productId);
            var customerId = await _customerRepository.GetCustomerIdAsync(appUserId);
            var cartId = await _cartRepository.GetCartIdAsync(customerId);
            var cartItem = new CartItem
            {
                Product = product,
                Quantity = quantity,
                CartId = cartId,
            };
            await _cartItemRepository.AddCartItemAsync(cartItem);
        }
    }
}
