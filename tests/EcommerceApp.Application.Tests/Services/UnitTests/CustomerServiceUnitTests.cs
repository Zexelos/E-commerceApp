using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.Services;
using EcommerceApp.Application.ViewModels.AdminPanel;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;
using System.Linq;
using MockQueryable.Moq;
using EcommerceApp.Application.ViewModels;

namespace EcommerceApp.Application.Tests.Services.UnitTests
{
    public class CustomerServiceUnitTests
    {
        private readonly CustomerService _sut;
        private readonly Mock<ICustomerRepository> _customerRepository = new();
        private readonly Mock<UserManager<AppUser>> _userManager;
        private readonly Mock<IMapper> _mapper = new();
        private readonly Mock<IPaginatorService<CustomerForListVM>> _paginatorService = new();
        private readonly Mock<IImageConverterService> _imageConverterService = new();
        private readonly Mock<ICartItemRepository> _cartItemRepository = new();
        private readonly Mock<IOrderRepository> _orderRepository = new();

        public CustomerServiceUnitTests()
        {
            var userStore = new Mock<IUserStore<AppUser>>();
            _userManager = new Mock<UserManager<AppUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            _sut = new CustomerService(
                _customerRepository.Object,
                _userManager.Object,
                _mapper.Object,
                _paginatorService.Object,
                _imageConverterService.Object,
                _cartItemRepository.Object,
                _orderRepository.Object
            );
        }

        [Fact]
        public async Task GetCustomerIdByAppUserIdAsync_ReturnsId()
        {
            // Arrange
            var customer = new Customer { Id = 4, AppUserId = "34tv3vvy" };
            var appUser = new AppUser { Id = "34tv3vvy", Customer = customer };

            _customerRepository.Setup(s => s.GetCustomerIdAsync(It.IsAny<string>())).ReturnsAsync(customer.Id);

            // Act
            var result = await _sut.GetCustomerIdByAppUserIdAsync(appUser.Id);

            // Assert
            Assert.Equal(customer.Id, result);
        }

        [Fact]
        public async Task GetCustomerDetailsAsync_ReturnsCustomerDetailsVM()
        {
            // Arrange
            var product = new Product { Name = "cghw345cghwy45", UnitPrice = 12.12m, Image = new byte[] { 1, 2, 3 } };
            var cartItems = new List<CartItem>
            {
                new CartItem { Quantity = 2, CartId = 2, Product = product, Cart = new Cart { CustomerId = 10 } }
            };
            var cart = new Cart { Id = 2, CartItems = cartItems, CustomerId = 10 };
            var orders = new List<Order>
            {
                new Order { Id = 1, ShipFirstName = "zordon", ShipLastName = "Rasista", CustomerId = 10 }
            };
            var customer = new Customer
            {
                Id = 10,
                FirstName = "zordon",
                LastName = "Rasista",
                Cart = cart,
                Orders = orders,
            };

            var ordersQ = orders.AsQueryable().BuildMock();

            var customers = new List<Customer> { customer };
            var customersQ = customers.AsQueryable().BuildMock();
            var cartItemsQ = cartItems.AsQueryable().BuildMock();

            var cartItemForCustomerDetailsVMs = new List<CartItemForCustomerDetailsVM>
            {
                new CartItemForCustomerDetailsVM { Quantity = cartItems[0].Quantity, ImageToDisplay = "image" }
            };

            var orderForCustomerDetailsVMs = new List<OrderForCustomerDetailsVM>
            {
                new OrderForCustomerDetailsVM { Price = 12.12m }
            };

            var customerDetailsVM = new CustomerDetailsVM
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                CartItems = cartItemForCustomerDetailsVMs,
                Orders = orderForCustomerDetailsVMs
            };

            _customerRepository.Setup(s => s.GetCustomers()).Returns(customersQ.Object);

            _cartItemRepository.Setup(s => s.GetCartItems()).Returns(cartItemsQ.Object);

            _orderRepository.Setup(s => s.GetOrders()).Returns(ordersQ.Object);

            _imageConverterService.Setup(s => s.GetImageStringFromByteArray(It.IsAny<byte[]>())).Returns(cartItemForCustomerDetailsVMs[0].ImageToDisplay);

            _mapper.Setup(s => s.ConfigurationProvider).Returns(
                new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Customer, CustomerDetailsVM>();
                    cfg.CreateMap<CartItem, CartItemForCustomerDetailsVM>();
                    cfg.CreateMap<Order, OrderForCustomerDetailsVM>();
                })
            );

            // Act 
            var result = await _sut.GetCustomerDetailsAsync(customer.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customerDetailsVM.FirstName, result.FirstName);
            Assert.Equal(customerDetailsVM.LastName, result.LastName);
            Assert.Equal(customerDetailsVM.CartItems[0].ImageToDisplay, result.CartItems[0].ImageToDisplay);
        }

        [Fact]
        public async Task GetPaginatedCustomersAsync_ReturnsCustomerListVM()
        {
            // Arrange
            var customer = new Customer { FirstName = "ziutek" };
            var customers = new List<Customer> { customer };
            var customersQ = customers.AsQueryable();

            var customerForListVMs = new List<CustomerForListVM>
            {
                new CustomerForListVM { FirstName = customer.FirstName }
            };

            var paginatedVM = new PaginatedVM<CustomerForListVM>
            {
                Items = customerForListVMs,
                CurrentPage = 1,
                TotalPages = 1
            };

            var customerListVM = new CustomerListVM
            {
                Customers = paginatedVM.Items,
                CurrentPage = paginatedVM.CurrentPage,
                TotalPages = paginatedVM.TotalPages
            };

            _customerRepository.Setup(s => s.GetCustomers()).Returns(customersQ);

            _mapper.Setup(s => s.ConfigurationProvider).Returns(
                new MapperConfiguration(cfg => cfg.CreateMap<Customer, CustomerForListVM>())
            );

            _paginatorService.Setup(s => s.CreateAsync(It.IsAny<IQueryable<CustomerForListVM>>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(paginatedVM);

            _mapper.Setup(s => s.Map<CustomerListVM>(It.IsAny<PaginatedVM<CustomerForListVM>>())).Returns(customerListVM);

            // Act
            var result = await _sut.GetPaginatedCustomersAsync(10, 1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(customerListVM, result);
        }

        [Fact]
        public async Task DeleteCustomerAsync_MethodsRunOnce()
        {
            // Arrange
            var customer = new Customer
            {
                Id = 10,
                AppUser = new AppUser
                {
                    Id = "aegrwxcegrs"
                }
            };
            var customers = new List<Customer> { customer };
            var customersQ = customers.AsQueryable().BuildMock();

            _customerRepository.Setup(s => s.GetCustomers()).Returns(customersQ.Object);

            // Act
            await _sut.DeleteCustomerAsync(customer.Id);

            // Assert
            _userManager.Verify(v => v.DeleteAsync(It.IsAny<AppUser>()), Times.Once);
        }
    }
}
