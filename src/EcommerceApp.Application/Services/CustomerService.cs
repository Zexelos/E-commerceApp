using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.AdminPanel;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IPaginatorService<CustomerForListVM> _paginatorService;
        private readonly IImageConverterService _imageConverterService;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IOrderRepository _orderRepository;

        public CustomerService(
            ICustomerRepository customerRepository,
            UserManager<AppUser> userManager,
            IMapper mapper,
            IPaginatorService<CustomerForListVM> paginatorService,
            IImageConverterService imageConverterService,
            ICartItemRepository cartItemRepository,
            IOrderRepository orderRepository)
        {
            _customerRepository = customerRepository;
            _userManager = userManager;
            _mapper = mapper;
            _paginatorService = paginatorService;
            _imageConverterService = imageConverterService;
            _cartItemRepository = cartItemRepository;
            _orderRepository = orderRepository;
        }

        public async Task<int> GetCustomerIdByAppUserIdAsync(string appUserId)
        {
            return await _customerRepository.GetCustomerIdAsync(appUserId);
        }

        public async Task<CustomerDetailsVM> GetCustomerDetailsAsync(int id)
        {
            var customerDetailsVM = await _customerRepository.GetCustomers()
                .Where(x => x.Id == id)
                    .Include(a => a.AppUser)
                .ProjectTo<CustomerDetailsVM>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
            var cartItemsForCustomerDetailsVM = await _cartItemRepository.GetCartItems()
                .Where(x => x.Cart.CustomerId == id)
                .ProjectTo<CartItemForCustomerDetailsVM>(_mapper.ConfigurationProvider)
            .ToListAsync();
            var ordersForCustomerDetailsVM = await _orderRepository.GetOrders()
                .Where(x => x.CustomerId == id)
                    .OrderByDescending(o => o.Id).Take(5)
                .ProjectTo<OrderForCustomerDetailsVM>(_mapper.ConfigurationProvider)
            .ToListAsync();
            customerDetailsVM.CartItems = cartItemsForCustomerDetailsVM;
            customerDetailsVM.Orders = ordersForCustomerDetailsVM;
            for (int i = 0; i < customerDetailsVM.CartItems.Count; i++)
            {
                customerDetailsVM.CartItems[i].ImageToDisplay = _imageConverterService.GetImageStringFromByteArray(customerDetailsVM.CartItems[i].ImageByteArray);
            }
            return customerDetailsVM;
        }

        public async Task<CustomerListVM> GetPaginatedCustomersAsync(int pageSize, int pageNumber)
        {
            var customersQuery = _customerRepository.GetCustomers()
                .Include(a => a.AppUser)
                .ProjectTo<CustomerForListVM>(_mapper.ConfigurationProvider);
            var paginatedVM = await _paginatorService.CreateAsync(customersQuery, pageNumber, pageSize);
            return _mapper.Map<CustomerListVM>(paginatedVM);
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await _customerRepository.GetCustomerAsync(id);
            var user = await _userManager.FindByIdAsync(customer.AppUserId);
            await _userManager.DeleteAsync(user);
        }
    }
}
