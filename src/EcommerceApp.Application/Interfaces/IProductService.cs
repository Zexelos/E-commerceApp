using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels;

namespace EcommerceApp.Application.Interfaces
{
    public interface IProductService
    {
        Task AddProductAsync(ProductVM product);
        Task<ProductVM> GetProductAsync(int id);
        Task<List<ProductVM>> GetProductsAsync();
        Task UpdateProductAsync(ProductVM product);
        Task DeleteProductAsync(int id);
    }
}
