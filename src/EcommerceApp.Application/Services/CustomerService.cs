using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BenchmarkDotNet.Attributes;
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

        public CustomerService(
            ICustomerRepository customerRepository,
            UserManager<AppUser> userManager,
            IMapper mapper,
            IPaginatorService<CustomerForListVM> paginatorService)
        {
            _customerRepository = customerRepository;
            _userManager = userManager;
            _mapper = mapper;
            _paginatorService = paginatorService;
        }

        public async Task<CustomerForListVM> GetCustomerAsync(int id)
        {
            var customer = await _customerRepository.GetCustomerAsync(id);
            var customerVM = _mapper.Map<CustomerForListVM>(customer);
            var user = await _userManager.FindByIdAsync(customer.AppUserId);
            customerVM.Email = user.Email;
            customerVM.PhoneNumber = user.PhoneNumber;
            return customerVM;
        }

        public async Task<CustomerListVM> GetCustomersAsync()
        {
            var customers = await _customerRepository.GetCustomers().ToListAsync();
            var customerForListVMs = _mapper.Map<List<CustomerForListVM>>(customers);
            for (int i = 0; i < customers.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(customers[i].AppUserId);
                customerForListVMs[i].Email = user.Email;
                customerForListVMs[i].PhoneNumber = user.PhoneNumber;
            }
            return new CustomerListVM
            {
                Customers = customerForListVMs
            };
        }

        [Benchmark]
        public async Task<CustomerListVM> GetPaginatedCustomersAsync(int pageSize, int pageNumber)
        {
            var customers = await _customerRepository.GetCustomers().ToListAsync();
            var customerForListVMs = _mapper.Map<List<CustomerForListVM>>(customers);
            for (int i = 0; i < customers.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(customers[i].AppUserId);
                customerForListVMs[i].Email = user.Email;
                customerForListVMs[i].PhoneNumber = user.PhoneNumber;
            }
            var paginatedVM = await _paginatorService.CreateAsync(customerForListVMs.AsQueryable(), pageNumber, pageSize);
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
