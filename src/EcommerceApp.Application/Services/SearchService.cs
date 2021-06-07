using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels;

namespace EcommerceApp.Application.Services
{
    public class SearchService : ISearchService
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IEmployeeService _employeeService;

        public SearchService(ICategoryService categoryService, IProductService productService, IEmployeeService employeeService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _employeeService = employeeService;
        }

        public async Task<List<CategoryVM>> CategorySearchAsync(string selectedValue, string searchString)
        {
            var model = new List<CategoryVM>();
            var parse = int.TryParse(searchString, out int id);
            model = selectedValue switch
            {
                "Id" => parse ? (await _categoryService.GetCategoriesAsync()).Where(x => x.Id == id).ToList() : model,
                "Name" => (await _categoryService.GetCategoriesAsync()).Where(x => x.Name.Contains(searchString)).ToList(),
                _ => model
            };
            return model;
        }

        public async Task<List<ProductVM>> ProductSearchAsync(string selectedValue, string searchString)
        {
            var model = new List<ProductVM>();
            var idParse = int.TryParse(searchString, out int id);
            var unitPriceParse = decimal.TryParse(searchString, out decimal unitPrice);
            var unitsInStockParse = int.TryParse(searchString, out int unitsInStock);
            model = selectedValue switch
            {
                "Id" => idParse ? (await _productService.GetProductsAsync()).Where(x => x.Id == id).ToList() : model,
                "Name" => (await _productService.GetProductsAsync()).Where(x => x.Name.Contains(searchString)).ToList(),
                "UnitPrice" => unitPriceParse ? (await _productService.GetProductsAsync()).Where(x => x.UnitPrice == unitPrice).ToList() : model,
                "UnitsInStock" => unitsInStockParse ? (await _productService.GetProductsAsync()).Where(x => x.UnitsInStock == unitsInStock).ToList() : model,
                "CategoryName" => (await _productService.GetProductsAsync()).Where(x => x.Name.Contains(searchString)).ToList(),
                _ => model
            };
            return model;
        }

        public async Task<List<EmployeeVM>> EmployeeSearchAsync(string selectedValue, string searchString)
        {
            var model = new List<EmployeeVM>();
            var idParse = int.TryParse(searchString, out int id);
            model = selectedValue switch
            {
                "Id" => idParse ? (await _employeeService.GetEmployeesAsync()).Where(x => x.Id == id).ToList() : model,
                "FirstName" => (await _employeeService.GetEmployeesAsync()).Where(x => x.FirstName.Contains(searchString)).ToList(),
                "LastName" => (await _employeeService.GetEmployeesAsync()).Where(x => x.LastName.Contains(searchString)).ToList(),
                "Email" => (await _employeeService.GetEmployeesAsync()).Where(x => x.Email.Contains(searchString)).ToList(),
                "Position" => (await _employeeService.GetEmployeesAsync()).Where(x => x.Position.Contains(searchString)).ToList(),
                _ => model
            };
            return model;
        }
    }
}