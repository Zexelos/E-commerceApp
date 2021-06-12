using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels;
using EcommerceApp.Application.ViewModels.AdminPanel;

namespace EcommerceApp.Application.Interfaces
{
    public interface ISearchService
    {
        Task<List<CategoryVM>> CategorySearchAsync(string selectedValue, string searchString);
        Task<List<ProductVM>> ProductSearchAsync(string selectedValue, string searchString);
        Task<EmployeeListVM> EmployeeSearchAsync(string selectedValue, string searchString);
        Task<CustomerListVM> CustomerSearchAsync(string selectedValue, string searchString);
    }
}
