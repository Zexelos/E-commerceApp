using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using EcommerceApp.Application.ViewModels.AdminPanel;

namespace EcommerceApp.Application.Interfaces
{
    public interface ISearchService
    {
        Task<CategoryListVM> SearchPaginatedCategoriesAsync(string selectedValue, string searchString, int pageSize, int pageNumber);
        Task<ProductListVM> SearchPaginatedProductsAsync(string selectedValue, string searchString, int pageSize, int pageNumber);
        Task<EmployeeListVM> SearchPaginatedEmployeesAsync(string selectedValue, string searchString, int pageSize, int pageNumber);
        Task<CustomerListVM> SearchPaginatedCustomersAsync(string selectedValue, string searchString, int pageSize, int pageNumber);
        Task<OrderListVM> SearchPaginatedOrdersAsync(string selectedValue, string searchString, int pageSize, int pageNumber);
    }
}
