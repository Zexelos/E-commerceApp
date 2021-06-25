using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Domain.Models;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Application.ViewModels.Cart;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using AutoMapper;

namespace EcommerceApp.Application.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IImageConverterService _imageConverterService;
        private readonly IMapper _mapper;

        public CartItemService(
            ICartItemRepository cartItemRepository,
            IProductRepository productRepository,
            ICartRepository cartRepository,
            ICustomerRepository customerRepository,
            IImageConverterService imageConverterService,
            IMapper mapper)
        {
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
            _cartRepository = cartRepository;
            _customerRepository = customerRepository;
            _imageConverterService = imageConverterService;
            _mapper = mapper;
        }

        public async Task AddCartItem(int productId, int quantity, string appUserId)
        {
            var product = await _productRepository.GetProductAsync(productId);
            if (product.UnitsInStock > 0)
            {
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

        public async Task<CartItemListVM> GetCartItemListAsync(string appUserId)
        {
            var cartItemListVM = await _cartRepository.GetCarts()
                .Where(x => x.Customer.AppUserId == appUserId)
                    .Include(ci => ci.CartItems)
                        .ThenInclude(p => p.Product)
                .ProjectTo<CartItemListVM>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
            for (int i = 0; i < cartItemListVM.CartItems.Count; i++)
            {
                cartItemListVM.CartItems[i].ImageToDisplay = _imageConverterService.GetImageStringFromByteArray(cartItemListVM.CartItems[i].ImageByteArray);
                cartItemListVM.TotalPrice += cartItemListVM.CartItems[i].TotalPrice;
            }
            return cartItemListVM;
        }

        public async Task IncreaseCartItemQuantityByOneAsync(int cartItemId)
        {
            var cartItem = await _cartItemRepository.GetCartItems().Where(x => x.Id == cartItemId).Include(p => p.Product).FirstOrDefaultAsync();
            if (cartItem.Quantity >= cartItem.Product.UnitsInStock)
            {
                cartItem.Quantity = cartItem.Product.UnitsInStock;
            }
            else
            {
                cartItem.Quantity++;
            }
            await _cartItemRepository.UpdateCartItemAsync(cartItem);
        }

        public async Task DecreaseCartItemQuantityByOneAsync(int cartItemId)
        {
            var cartItem = await _cartItemRepository.GetCartItemAsync(cartItemId);
            if (cartItem.Quantity <= 1)
            {
                cartItem.Quantity = 1;
            }
            else
            {
                cartItem.Quantity--;
            }
            await _cartItemRepository.UpdateCartItemAsync(cartItem);
        }

        public async Task DeleteCartItemAsync(int cartItemId)
        {
            await _cartItemRepository.DeleteCartItemAsync(cartItemId);
        }
    }
}
