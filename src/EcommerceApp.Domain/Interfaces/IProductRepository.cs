using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task AddProductAsync(Product product);

        Task<Product> GetProductAsync(int id);

        IQueryable<Product> GetProducts();

        Task UpdateProductAsync(Product product);

        Task DeleteProductAsync(int id);
    }
}
