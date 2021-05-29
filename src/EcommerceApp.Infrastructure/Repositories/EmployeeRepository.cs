using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddEmplyeeAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        public async Task<Employee> GetEmployeeAsync(int id)
        {
            return await _context.Employees.AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IQueryable<Employee>> GetEmployeesAsync()
        {
            return (await _context.Employees.ToListAsync()).AsQueryable();
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            _context.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var employeeToDelete = await _context.Employees.FindAsync(id);
            if (employeeToDelete != null)
            {
                _context.Remove(employeeToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}