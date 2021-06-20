using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels.EmployeePanel;

namespace EcommerceApp.Application.Interfaces
{
    public interface ICategoryService
    {
        Task AddCategoryAsync(CategoryVM categoryVM);
        Task<CategoryVM> GetCategoryAsync(int id);
        Task<List<string>> GetCategoriesNamesAsync();
        Task<CategoryListVM> GetPaginatedCategoriesAsync(int pageSize, int pageNumber);
        Task UpdateCategoryAsync(CategoryVM categoryVM);
        Task DeleteCategoryAsync(int id);
    }
}
