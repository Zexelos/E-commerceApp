using System.Runtime.CompilerServices;
using System.Reflection.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.Cart;
using EcommerceApp.Application.ViewModels.Order;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using AutoMapper.QueryableExtensions;
using AutoMapper;

namespace EcommerceApp.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IImageConverterService _imageConverterService;
        private readonly IMapper _mapper;
        private readonly IPaginatorService<OrderForListVM> _paginatorService;

        public OrderService(ICartItemRepository cartItemRepository,
            ICartRepository cartRepository,
            ICustomerRepository customerRepository,
            UserManager<AppUser> userManager,
            IOrderItemRepository orderItemRepository,
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            IImageConverterService imageConverterService,
            IMapper mapper,
            IPaginatorService<OrderForListVM> paginatorService)
        {
            _cartItemRepository = cartItemRepository;
            _cartRepository = cartRepository;
            _customerRepository = customerRepository;
            _userManager = userManager;
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _imageConverterService = imageConverterService;
            _mapper = mapper;
            _paginatorService = paginatorService;
        }

        public async Task AddOrderAsync(OrderCheckoutVM orderCheckoutVM)
        {
            var cart = await _cartRepository.GetCartAsync(orderCheckoutVM.CartId);
            var order = new Order
            {
                Price = orderCheckoutVM.TotalPrice,
                CustomerId = cart.CustomerId,
                ShipFirstName = orderCheckoutVM.FirstName,
                ShipLastName = orderCheckoutVM.LastName,
                ShipPostalCode = orderCheckoutVM.PostalCode,
                ShipAddress = orderCheckoutVM.Address,
                ShipCity = orderCheckoutVM.City,
                ContactEmail = orderCheckoutVM.Email,
                ContactPhoneNumber = orderCheckoutVM.PhoneNumber
            };
            await _orderRepository.AddOrderAsync(order);
            foreach (var cartItem in orderCheckoutVM.CartItems)
            {
                var orderItem = new OrderItem
                {
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    OrderId = order.Id,
                };
                await _orderItemRepository.AddOrderItemAsync(orderItem);
            }
            await _cartItemRepository.DeleteCartItemsByCartIdAsync(orderCheckoutVM.CartId);
        }

        public async Task<OrderCheckoutVM> GetOrderCheckoutVMAsync(int cartId)
        {
            var cart = await _cartRepository.GetCartAsync(cartId);
            var customer = await _customerRepository.GetCustomerAsync(cart.CustomerId);
            var cartItems = await _cartItemRepository.GetCartItemsByCartId(cartId).ToListAsync();
            var appUser = await _userManager.FindByIdAsync(customer.AppUserId);
            var totalPrice = 0m;

            var cartItemForListVMs = new List<CartItemForListVM>();
            for (int i = 0; i < cartItems.Count; i++)
            {
                var product = await _productRepository.GetProductAsync(cartItems[i].ProductId);
                cartItemForListVMs.Add(new CartItemForListVM
                {
                    Id = cartItems[i].Id,
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.UnitPrice,
                    Quantity = cartItems[i].Quantity,
                    ImageToDisplay = _imageConverterService.GetImageStringFromByteArray(product.Image),
                });
                totalPrice += product.UnitPrice * cartItems[i].Quantity;
            }
            return new OrderCheckoutVM
            {
                CartId = cartId,
                CustomerId = customer.Id,
                CartItems = cartItemForListVMs,
                TotalPrice = totalPrice,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = appUser.Email,
                City = customer.City,
                PostalCode = customer.PostalCode,
                Address = customer.Address,
                PhoneNumber = appUser.PhoneNumber
            };
        }

        public async Task<OrderListVM> GetPaginatedOrdersAsync(int pageSize, int pageNumber)
        {
            var orders = _orderRepository.GetOrders().ProjectTo<OrderForListVM>(_mapper.ConfigurationProvider);
            var paginatedVM = await _paginatorService.CreateAsync(orders, pageNumber, pageSize);
            return new OrderListVM
            {
                Orders = paginatedVM.Items,
                CurrentPage = paginatedVM.CurrentPage,
                TotalPages = paginatedVM.TotalPages
            };
        }

        public async Task<OrderDetailsVM> GetOrderDetailsAsync(int id)
        {
            var result = await _orderRepository.GetOrders()
                .Where(x => x.Id == id)
                    .Include(x => x.OrderItems)
                        .ThenInclude(y => y.Product)
                .FirstOrDefaultAsync();
            var orderItemForDetailsVM = _mapper.Map<List<OrderItemForDetailsVM>>(result.OrderItems);
            var orderDetailsVM = _mapper.Map<OrderDetailsVM>(result);
            orderDetailsVM.OrderItems = orderItemForDetailsVM;
            return orderDetailsVM;
        }
    }
}
