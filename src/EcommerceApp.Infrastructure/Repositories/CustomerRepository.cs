using System;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task<Customer> GetCustomerAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<IQueryable<Customer>> GetCustomersAsync()
        {
            return (await _context.Customers.ToListAsync()).AsQueryable();
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }
    }
}