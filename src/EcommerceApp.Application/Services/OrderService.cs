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
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IImageConverterService _imageConverterService;
        private readonly IMapper _mapper;
        private readonly IPaginatorService<OrderForListVM> _orderPaginatorService;
        private readonly IPaginatorService<CustomerOrderForListVM> _customerOrderService;
        private readonly IProductRepository _productRepository;

        public OrderService(ICartItemRepository cartItemRepository,
            ICartRepository cartRepository,
            ICustomerRepository customerRepository,
            IOrderItemRepository orderItemRepository,
            IOrderRepository orderRepository,
            IImageConverterService imageConverterService,
            IMapper mapper,
            IPaginatorService<OrderForListVM> paginatorService,
            IPaginatorService<CustomerOrderForListVM> customerOrderService,
            IProductRepository productRepository)
        {
            _cartItemRepository = cartItemRepository;
            _cartRepository = cartRepository;
            _customerRepository = customerRepository;
            _orderItemRepository = orderItemRepository;
            _orderRepository = orderRepository;
            _imageConverterService = imageConverterService;
            _mapper = mapper;
            _orderPaginatorService = paginatorService;
            _customerOrderService = customerOrderService;
            _productRepository = productRepository;
        }

        public async Task AddOrderAsync(OrderCheckoutVM orderCheckoutVM)
        {
            orderCheckoutVM.CartItems = orderCheckoutVM.CartItems.OrderBy(x => x.ProductId).ToList();
            var cart = await _cartRepository.GetCartAsync(orderCheckoutVM.CartId);
            var order = new Order
            {
                CreateDate = DateTime.Now,
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

            List<int> productIdList = new();
            foreach (var item in orderCheckoutVM.CartItems)
            {
                productIdList.Add(item.ProductId);
            }
            var orderItemList = new List<OrderItem>();
            var productList = await _productRepository.GetProducts().Where(x => productIdList.Contains(x.Id)).AsNoTracking().ToListAsync();
            for (int i = 0; i < orderCheckoutVM.CartItems.Count; i++)
            {
                orderItemList.Add(new OrderItem
                {
                    ProductId = orderCheckoutVM.CartItems[i].ProductId,
                    Quantity = orderCheckoutVM.CartItems[i].Quantity,
                    OrderId = order.Id,
                });
                productList[i].UnitsInStock -= orderCheckoutVM.CartItems[i].Quantity;
            }
            await _productRepository.UpdateProductsAsync(productList);
            await _orderItemRepository.AddOrderItemsAsync(orderItemList);
            await _cartItemRepository.DeleteCartItemsByCartIdAsync(orderCheckoutVM.CartId);
        }

        public async Task<OrderCheckoutVM> GetOrderCheckoutVMAsync(int customerId)
        {
            var order = await _customerRepository.GetCustomers()
                .Where(c => c.Id == customerId)
                    .Include(a => a.AppUser)
                    .Include(c => c.Cart)
                        .ThenInclude(ci => ci.CartItems)
                            .ThenInclude(p => p.Product)
                .ProjectTo<OrderCheckoutVM>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            for (int i = order.CartItems.Count - 1; i >= 0; i--)
            {
                if (order.CartItems[i].ProductsInStock <= 0)
                {
                    order.CartItems.RemoveAt(i);
                }
                else if (order.CartItems[i].Quantity > order.CartItems[i].ProductsInStock)
                {
                    order.CartItems[i].Quantity = order.CartItems[i].ProductsInStock;
                    order.CartItems[i].ImageToDisplay = _imageConverterService.GetImageStringFromByteArray(order.CartItems[i].ImageByteArray);
                    order.TotalPrice += order.CartItems[i].TotalPrice;
                }
                else
                {
                    order.CartItems[i].ImageToDisplay = _imageConverterService.GetImageStringFromByteArray(order.CartItems[i].ImageByteArray);
                    order.TotalPrice += order.CartItems[i].TotalPrice;
                }
            }
            return order;
        }

        public async Task<OrderListVM> GetPaginatedOrdersAsync(int pageSize, int pageNumber)
        {
            var orders = _orderRepository.GetOrders().ProjectTo<OrderForListVM>(_mapper.ConfigurationProvider);
            var paginatedVM = await _orderPaginatorService.CreateAsync(orders, pageNumber, pageSize);
            return _mapper.Map<OrderListVM>(paginatedVM);
        }

        public async Task<CustomerOrderListVM> GetPaginatedCustomerOrdersAsync(string appUserId, int pageSize, int pageNumber)
        {
            var orders = _orderRepository.GetOrders()
                .Where(x => x.Customer.AppUserId == appUserId)
                .ProjectTo<CustomerOrderForListVM>(_mapper.ConfigurationProvider);
            var paginatedVM = await _customerOrderService.CreateAsync(orders, pageNumber, pageSize);
            return _mapper.Map<CustomerOrderListVM>(paginatedVM);
        }

        public async Task<CustomerOrderDetailsVM> GetCustomerOrderDetailsAsync(string appUserId, int orderId)
        {
            var order = await _orderRepository.GetOrders()
                .Where(x => x.Id == orderId && x.Customer.AppUserId == appUserId)
                    .Include(oi => oi.OrderItems)
                        .ThenInclude(p => p.Product)
                .ProjectTo<CustomerOrderDetailsVM>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
            for (int i = 0; i < order.OrderItems.Count; i++)
            {
                order.OrderItems[i].ImageToDisplay = _imageConverterService.GetImageStringFromByteArray(order.OrderItems[i].ImageByteArray);
                order.Price = order.OrderItems[i].TotalPrice;
            }
            return order;
        }

        public async Task<OrderDetailsVM> GetOrderDetailsAsync(int id)
        {
            return await _orderRepository.GetOrders()
                .Where(x => x.Id == id)
                    .Include(x => x.OrderItems)
                        .ThenInclude(y => y.Product)
                .ProjectTo<OrderDetailsVM>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            await _orderRepository.DeleteOrderAsync(id);
        }
    }
}
