using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels.EmployeePanel;

namespace EcommerceApp.Application.Interfaces
{
    public interface ICategoryService
    {
        Task AddCategoryAsync(CategoryVM categoryVM);
        Task<CategoryVM> GetCategoryAsync(int id);
        Task<CategoryListVM> GetCategoriesAsync();
        Task UpdateCategoryAsync(CategoryVM categoryVM);
        Task DeleteCategoryAsync(int id);
    }
}
