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

        public async Task<int> GetCustomerIdAsync(string appUserId)
        {
            return (await _context.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.AppUserId == appUserId)).Id;
        }

        public IQueryable<Customer> GetCustomers()
        {
            return _context.Customers.AsQueryable();
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }
    }
}