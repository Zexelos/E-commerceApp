using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Domain.Models;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Application.ViewModels.Cart;
using System.Collections.Generic;
using System.Linq;

namespace EcommerceApp.Application.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IImageConverterService _imageConverterService;

        public CartItemService(
            ICartItemRepository cartItemRepository,
            IProductRepository productRepository,
            ICartRepository cartRepository,
            ICustomerRepository customerRepository,
            IImageConverterService imageConverterService)
        {
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
            _cartRepository = cartRepository;
            _customerRepository = customerRepository;
            _imageConverterService = imageConverterService;
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

        public async Task<ListCartItemForListVM> GetListCartItemForListVMAsync(string appUserId)
        {
            var customerId = await _customerRepository.GetCustomerIdAsync(appUserId);
            var cartId = await _cartRepository.GetCartIdAsync(customerId);
            var cartItems = (await _cartItemRepository.GetCartItemsByCartIdAsync(cartId)).ToList();
            var cartItemForListVMs = new List<CartItemForListVM>();
            for (int i = 0; i < cartItems.Count; i++)
            {
                var product = await _productRepository.GetProductAsync(cartItems[i].ProductId);
                cartItemForListVMs.Add(new CartItemForListVM
                {
                    Id = cartItems[i].Id,
                    Name = product.Name,
                    Price = product.UnitPrice,
                    Quantity = cartItems[i].Quantity,
                    ImageToDisplay = _imageConverterService.GetImageStringFromByteArray(product.Image)
                });
            }
            return new ListCartItemForListVM
            {
                CartId = cartId,
                CartItems = cartItemForListVMs
            };
        }

        public async Task IncreaseCartItemQuantityByOneAsync(int cartItemId)
        {
            var cartItem = await _cartItemRepository.GetCartItemAsync(cartItemId);
            cartItem.Quantity++;
            await _cartItemRepository.UpdateCartItemAsync(cartItem);
        }

        public async Task DecreaseCartItemQuantityByOneAsync(int cartItemId)
        {
            var cartItem = await _cartItemRepository.GetCartItemAsync(cartItemId);
            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity--;
                await _cartItemRepository.UpdateCartItemAsync(cartItem);
            }
        }

        public async Task DeleteCartItemAsync(int cartItemId)
        {
            await _cartItemRepository.DeleteCartItemAsync(cartItemId);
        }
    }
}
