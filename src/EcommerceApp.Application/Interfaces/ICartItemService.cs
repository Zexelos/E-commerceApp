using System.Threading.Tasks;

namespace EcommerceApp.Application.Interfaces
{
    public interface ICartItemService
    {
        Task AddCartItem(int productId, int quantity, string appUserId);
    }
}
