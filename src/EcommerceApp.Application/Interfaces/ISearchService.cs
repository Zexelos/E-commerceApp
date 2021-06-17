using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using EcommerceApp.Application.ViewModels.AdminPanel;

namespace EcommerceApp.Application.Interfaces
{
    public interface ISearchService
    {
        Task<CategoryListVM> CategorySearchAsync(string selectedValue, string searchString);
        Task<ProductListVM> ProductSearchAsync(string selectedValue, string searchString);
        Task<EmployeeListVM> SearchPaginatedEmployeesAsync(string selectedValue, string searchString, int pageSize, int pageNumber);
        Task<CustomerListVM> SearchPaginatedCustomersAsync(string selectedValue, string searchString, int pageSize, int pageNumber);
    }
}
