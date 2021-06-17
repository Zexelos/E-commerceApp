using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using EcommerceApp.Application.ViewModels.AdminPanel;
using EcommerceApp.Application.ViewModels;

namespace EcommerceApp.Application.Services
{
    public class SearchService : ISearchService
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IEmployeeService _employeeService;
        private readonly ICustomerService _customerService;
        private readonly IPaginatorService<EmployeeForListVM> _employeePaginatorService;
        private readonly IPaginatorService<CustomerForListVM> _customerPaginatorService;

        public SearchService(
            ICategoryService categoryService,
            IProductService productService,
            IEmployeeService employeeService,
            ICustomerService customerService,
            IPaginatorService<EmployeeForListVM> employeePaginatorService,
            IPaginatorService<CustomerForListVM> customerPaginatorService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _employeeService = employeeService;
            _customerService = customerService;
            _employeePaginatorService = employeePaginatorService;
            _customerPaginatorService = customerPaginatorService;
        }

        public async Task<CategoryListVM> CategorySearchAsync(string selectedValue, string searchString)
        {
            var model = new CategoryListVM();
            var parse = int.TryParse(searchString, out int id);
            model.Categories = selectedValue switch
            {
                "Id" => parse ? (await _categoryService.GetCategoriesAsync()).Categories.Where(x => x.Id == id).ToList() : model.Categories,
                "Name" => (await _categoryService.GetCategoriesAsync()).Categories.Where(x => x.Name.Contains(searchString)).ToList(),
                _ => model.Categories
            };
            return model;
        }

        public async Task<ProductListVM> ProductSearchAsync(string selectedValue, string searchString)
        {
            var model = new ProductListVM();
            var idParse = int.TryParse(searchString, out int id);
            var unitPriceParse = decimal.TryParse(searchString, out decimal unitPrice);
            var unitsInStockParse = int.TryParse(searchString, out int unitsInStock);
            model.Products = selectedValue switch
            {
                "Id" => idParse ? (await _productService.GetProductsAsync()).Products.Where(x => x.Id == id).ToList() : model.Products,
                "Name" => (await _productService.GetProductsAsync()).Products.Where(x => x.Name.Contains(searchString)).ToList(),
                "UnitPrice" => unitPriceParse ? (await _productService.GetProductsAsync()).Products.Where(x => x.UnitPrice == unitPrice).ToList() : model.Products,
                "UnitsInStock" => unitsInStockParse ? (await _productService.GetProductsAsync()).Products.Where(x => x.UnitsInStock == unitsInStock).ToList() : model.Products,
                "CategoryName" => (await _productService.GetProductsAsync()).Products.Where(x => x.Name.Contains(searchString)).ToList(),
                _ => model.Products
            };
            return model;
        }

        public async Task<EmployeeListVM> SearchPaginatedEmployeesAsync(string selectedValue, string searchString, int pageSize, int pageNumber)
        {
            var model = new EmployeeListVM();
            var idParse = int.TryParse(searchString, out int id);
            model.Employees = selectedValue switch
            {
                "Id" => idParse ? (await _employeeService.GetEmployeesAsync()).Employees.Where(x => x.Id == id).ToList() : model.Employees,
                "FirstName" => (await _employeeService.GetEmployeesAsync()).Employees.Where(x => x.FirstName.Contains(searchString)).ToList(),
                "LastName" => (await _employeeService.GetEmployeesAsync()).Employees.Where(x => x.LastName.Contains(searchString)).ToList(),
                "Email" => (await _employeeService.GetEmployeesAsync()).Employees.Where(x => x.Email.Contains(searchString)).ToList(),
                "Position" => (await _employeeService.GetEmployeesAsync()).Employees.Where(x => x.Position.Contains(searchString)).ToList(),
                _ => model.Employees
            };
            var paginatedVM = await _employeePaginatorService.CreateAsync(model.Employees.AsQueryable(), pageNumber, pageSize);
            model.Employees = paginatedVM.Items;
            model.CurrentPage = paginatedVM.CurrentPage;
            model.TotalPages = paginatedVM.TotalPages;
            return model;
        }

        public async Task<CustomerListVM> SearchPaginatedCustomersAsync(string selectedValue, string searchString, int pageSize, int pageNumber)
        {
            var model = new CustomerListVM();
            var idParse = int.TryParse(searchString, out int id);
            model.Customers = selectedValue switch
            {
                "Id" => idParse ? (await _customerService.GetCustomersAsync()).Customers.Where(x => x.Id == id).ToList() : model.Customers,
                "FirstName" => (await _customerService.GetCustomersAsync()).Customers.Where(x => x.FirstName == searchString).ToList(),
                "LastName" => (await _customerService.GetCustomersAsync()).Customers.Where(x => x.LastName == searchString).ToList(),
                "Email" => (await _customerService.GetCustomersAsync()).Customers.Where(x => x.Email == searchString).ToList(),
                "City" => (await _customerService.GetCustomersAsync()).Customers.Where(x => x.City == searchString).ToList(),
                "PostalCode" => (await _customerService.GetCustomersAsync()).Customers.Where(x => x.PostalCode == searchString).ToList(),
                "Address" => (await _customerService.GetCustomersAsync()).Customers.Where(x => x.Address == searchString).ToList(),
                "PhoneNumber" => (await _customerService.GetCustomersAsync()).Customers.Where(x => x.PhoneNumber == searchString).ToList(),
                _ => model.Customers
            };
            var paginatedVM = await _customerPaginatorService.CreateAsync(model.Customers.AsQueryable(), pageNumber, pageSize);
            model.Customers = paginatedVM.Items;
            model.CurrentPage = paginatedVM.CurrentPage;
            model.TotalPages = paginatedVM.TotalPages;
            return model;
        }
    }
}