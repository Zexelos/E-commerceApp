using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels;

namespace EcommerceApp.Application.Interfaces
{
    public interface ICategoryService
    {
        Task AddCategoryAsync(CategoryVM categoryVM);
        Task<CategoryVM> GetCategoryAsync(int id);
        Task<List<CategoryVM>> GetCategoriesAsync();
        Task UpdateCategoryAsync(CategoryVM categoryVM);
        Task DeleteCategoryAsync(int id);
    }
}
