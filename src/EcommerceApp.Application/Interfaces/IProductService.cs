using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using EcommerceApp.Application.ViewModels.Product;

namespace EcommerceApp.Application.Interfaces
{
    public interface IProductService
    {
        Task AddProductAsync(ProductVM product);
        Task<ProductVM> GetProductAsync(int id);
        Task<ProductDetailsForUserVM> GetProductDetailsForUserAsync(int id);
        Task<ProductListVM> GetProductsAsync();
        Task<ProductListVM> GetPaginatedProductsAsync(int pageSize, int pageNumber);
        Task<ListProductDetailsForUserVM> GetProductsWithImageAsync();
        Task<List<ProductVM>> GetProductsByCategoryNameAsync(string name);
        Task<ListProductDetailsForUserVM> GetListProductDetailsForUserVMByCategoryNameAsync(string name);
        Task UpdateProductAsync(ProductVM product);
        Task DeleteProductAsync(int id);
    }
}
