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

        public CustomerService(
            ICustomerRepository customerRepository,
            UserManager<AppUser> userManager,
            IMapper mapper,
            IPaginatorService<CustomerForListVM> paginatorService,
            IImageConverterService imageConverterService)
        {
            _customerRepository = customerRepository;
            _userManager = userManager;
            _mapper = mapper;
            _paginatorService = paginatorService;
            _imageConverterService = imageConverterService;
        }

        public async Task<int> GetCustomerIdByAppUserIdAsync(string appUserId)
        {
            return await _customerRepository.GetCustomerIdAsync(appUserId);
        }

        public async Task<CustomerDetailsVM> GetCustomerDetailsVMAsync(int id)
        {
            var qTest = _customerRepository.GetCustomers()
                .Where(x => x.Id == id)
                    .Include(o => o.Orders.OrderByDescending(x => x.Id).Take(2)).ToQueryString();
            Console.WriteLine(qTest);
            var test = await _customerRepository.GetCustomers()
                .Where(x => x.Id == id)
                    .Include(o => o.Orders.OrderByDescending(x => x.Id).Take(2)).FirstOrDefaultAsync();
            Console.WriteLine(test.Orders.Count);
            var qString = _customerRepository.GetCustomers()
                .Where(x => x.Id == id)
                    .Include(o => o.Orders.OrderByDescending(x => x.Id).Take(2))
                    .Include(c => c.Cart)
                        .ThenInclude(ci => ci.CartItems)
                            .ThenInclude(p => p.Product).ToQueryString();
            var customerDetailsVM = await _customerRepository.GetCustomers()
                .Where(x => x.Id == id)
                    .Include(o => o.Orders.OrderByDescending(x => x.Id).Take(2))
                    .Include(c => c.Cart)
                        .ThenInclude(ci => ci.CartItems)
                            .ThenInclude(p => p.Product)
                .ProjectTo<CustomerDetailsVM>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            for (int i = 0; i < customerDetailsVM.CartItems.Count; i++)
            {
                customerDetailsVM.CartItems[i].ImageToDisplay = _imageConverterService.GetImageStringFromByteArray(customerDetailsVM.CartItems[i].ImageByteArray);
            }
            //Console.WriteLine(qString);
            return customerDetailsVM;
        }

        public async Task<CustomerListVM> GetPaginatedCustomersAsync(int pageSize, int pageNumber)
        {
            var customersQuery = _customerRepository.GetCustomers()
                .Include(a => a.AppUser)
                .ProjectTo<CustomerForListVM>(_mapper.ConfigurationProvider);
            var paginatedVM = await _paginatorService.CreateAsync(customersQuery, pageNumber, pageSize);
            return new CustomerListVM
            {
                Customers = paginatedVM.Items,
                CurrentPage = paginatedVM.CurrentPage,
                TotalPages = paginatedVM.TotalPages
            };
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await _customerRepository.GetCustomerAsync(id);
            var user = await _userManager.FindByIdAsync(customer.AppUserId);
            await _userManager.DeleteAsync(user);
        }
    }
}
