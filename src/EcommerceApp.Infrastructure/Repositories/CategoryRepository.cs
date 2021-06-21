using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task<Category> GetCategoryAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }
        
        public async Task<Category> GetCategoryAsync(string name)
        {
            return await _context.Categories.FirstOrDefaultAsync(x => x.Name == name);
        }

        public IQueryable<Category> GetCategories()
        {
            return _context.Categories.AsQueryable();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
            if (category.Image.Length == 0)
            {
                _context.Entry(category).Property(x => x.Image).IsModified = false;
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
    }
}
