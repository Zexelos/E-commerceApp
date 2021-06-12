using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.AdminPanel;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.AspNetCore.Identity;

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
            customerVM.Email = user.Email;
            customerVM.PhoneNumber = user.PhoneNumber;
            return customerVM;
        }

        public async Task<CustomerListVM> GetCustomersAsync()
        {
            var customers = (await _customerRepository.GetCustomersAsync()).ToList();
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

        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await _customerRepository.GetCustomerAsync(id);
            var user = await _userManager.FindByIdAsync(customer.AppUserId);
            await _userManager.DeleteAsync(user);
        }
    }
}
