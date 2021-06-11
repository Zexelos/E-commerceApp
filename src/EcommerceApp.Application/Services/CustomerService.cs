using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels;
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

        public CustomerService(ICustomerRepository customerRepository, UserManager<AppUser> userManager, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<CustomerVM> GetCustomerAsync(int id)
        {
            var customer = await _customerRepository.GetCustomerAsync(id);
            var customerVM = _mapper.Map<CustomerVM>(customer);
            var user = await _userManager.FindByIdAsync(customer.AppUserId);
            customerVM.Email = await _userManager.GetEmailAsync(user);
            customerVM.PhoneNumber = await _userManager.GetPhoneNumberAsync(user);
            return customerVM;
        }

        public async Task<List<CustomerVM>> GetCustomersAsync()
        {
            var customers = (await _customerRepository.GetCustomersAsync()).ToList();
            var customerVMs = _mapper.Map<List<CustomerVM>>(customers);
            for (int i = 0; i < customers.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(customers[i].AppUserId);
                customerVMs[i].Email = await _userManager.GetEmailAsync(user);
                customerVMs[i].PhoneNumber = await _userManager.GetPhoneNumberAsync(user);
            }
            return customerVMs;
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await _customerRepository.GetCustomerAsync(id);
            var user = await _userManager.FindByIdAsync(customer.AppUserId);
            await _userManager.DeleteAsync(user);
        }
    }
}
