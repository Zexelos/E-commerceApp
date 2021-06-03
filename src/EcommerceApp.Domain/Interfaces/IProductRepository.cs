using System.Threading.Tasks;
using EcommerceApp.Domain.Models;
using System.Collections.Generic;

namespace EcommerceApp.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task AddProductAsync(Product product);

        Task<Product> GetProductAsync(int id);

        Task<List<Product>> GetProductsAsync();

        Task UpdateProductAsync(Product product);

        Task DeleteProductAsync(int id);
    }
}
