using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.Services;
using EcommerceApp.Application.ViewModels.Cart;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace EcommerceApp.Application.Tests.Services.UnitTests
{
    public class CartItemServiceUnitTests
    {
        private readonly CartItemService _sut;
        private readonly Mock<ICartItemRepository> _cartItemRepository = new();
        private readonly Mock<IProductRepository> _productRepository = new();
        private readonly Mock<ICartRepository> _cartRepository = new();
        private readonly Mock<IImageConverterService> _imageConverterService = new();
        private readonly Mock<IMapper> _mapper = new();

        public CartItemServiceUnitTests()
        {
            _sut = new CartItemService(
                _cartItemRepository.Object,
                _productRepository.Object,
                _cartRepository.Object,
                _imageConverterService.Object,
                _mapper.Object
            );
        }

        [Fact]
        public async Task AddCartItem_MethodsRunOnce()
        {
            // Arrange
            var productId = 5;
            var quantity = 10;
            var appUserId = "15";
            var product = new Product { Id = 5, Name = "Mak", UnitsInStock = 20 };
            var customer = new Customer { Id = 3, AppUserId = appUserId };
            var cart = new Cart { Id = 2, Customer = customer };
            var carts = new List<Cart> { cart };
            var cartQ = carts.AsQueryable().BuildMock();

            _productRepository.Setup(x => x.GetProductAsync(productId)).ReturnsAsync(product);

            _cartRepository.Setup(x => x.GetCarts()).Returns(cartQ.Object);

            // Act
            await _sut.AddCartItem(productId, quantity, appUserId);

            // Assert
            _cartItemRepository.Verify(x => x.AddCartItemAsync(It.IsAny<CartItem>()), Times.Once);
        }

        [Fact]
        public async Task GetCartItemListAsync_ReturnsCartItemListVM()
        {
            // Arrange
            var appUserId = "w345xtycw3";
            var product = new Product { Name = "Mleko", UnitPrice = 10m, UnitsInStock = 50, Image = new byte[] { 1, 2, 3 } };
            var cartItems = new List<CartItem> { new CartItem { Product = product } };
            var customer = new Customer { AppUserId = appUserId };
            var cart = new Cart { Customer = customer, CartItems = cartItems };
            var carts = new List<Cart> { cart };
            var cartsQ = carts.AsQueryable().BuildMock();
            var imageToDisplay = "wecty34vyv4yy3";
            var cartItemListVM = new CartItemListVM
            {
                CustomerId = customer.Id,
                CartItems = new List<CartItemForListVM>
                {
                    new CartItemForListVM
                    {
                        ImageToDisplay = imageToDisplay,
                    }
                }
            };

            _cartRepository.Setup(x => x.GetCarts()).Returns(cartsQ.Object);

            _mapper.Setup(x => x.ConfigurationProvider).Returns(
                () => new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Cart, CartItemListVM>();
                    cfg.CreateMap<CartItem, CartItemForListVM>();
                }));

            _imageConverterService.Setup(s => s.GetImageStringFromByteArray(It.IsAny<byte[]>())).Returns(imageToDisplay);

            // Act
            var result = await _sut.GetCartItemListAsync(appUserId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(cartItemListVM.CartItems[0].ImageToDisplay, result.CartItems[0].ImageToDisplay);
        }

        [Fact]
        public async Task IncreaseCartItemQuantityByOneAsync_MethodsRunOnce()
        {
            // Arrange
            int cartItemId = 10;
            var cartItem = new CartItem
            {
                Id = cartItemId,
                Product = new Product
                {
                    Name = "Mleko",
                    UnitsInStock = 10
                },
                Quantity = 2
            };
            var cartItems = new List<CartItem> { cartItem };
            var cartItemsQ = cartItems.AsQueryable().BuildMock();

            _cartItemRepository.Setup(x => x.GetCartItems()).Returns(cartItemsQ.Object);

            // Act
            await _sut.IncreaseCartItemQuantityByOneAsync(cartItemId);

            // Assert
            _cartItemRepository.Verify(x => x.UpdateCartItemAsync(cartItem), Times.Once);
        }

        [Fact]
        public async Task DecreaseCartItemQuantityByOneAsync_MethodsRunOnce()
        {
            // Arrange
            int cartItemId = 10;
            var cartItem = new CartItem
            {
                Id = cartItemId,
                Product = new Product
                {
                    Name = "Mleko",
                    UnitsInStock = 10
                },
                Quantity = 2
            };

            _cartItemRepository.Setup(x => x.GetCartItemAsync(cartItemId)).ReturnsAsync(cartItem);

            // Act
            await _sut.DecreaseCartItemQuantityByOneAsync(cartItemId);

            // Assert
            _cartItemRepository.Verify(x => x.UpdateCartItemAsync(cartItem), Times.Once);
        }

        [Fact]
        public async Task DeleteCartItemAsync_MethodsRunsOnce()
        {
            // Arrange
            int cartItemId = 10;

            // Act
            await _sut.DeleteCartItemAsync(cartItemId);

            // Assert
            _cartItemRepository.Verify(x => x.DeleteCartItemAsync(It.IsAny<int>()), Times.Once);
        }
    }
}
