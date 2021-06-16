using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels.Cart;

namespace EcommerceApp.Application.Interfaces
{
    public interface ICartItemService
    {
        Task AddCartItem(int productId, int quantity, string appUserId);
        Task<int> GetCartIdByAppUserIdAsync(string appUserId);
        Task<ListCartItemForListVM> GetListCartItemForListVMAsync(string appUserId);
        Task IncreaseCartItemQuantityByOneAsync(int cartItemId);
        Task DecreaseCartItemQuantityByOneAsync(int cartItemId);
        Task DeleteCartItemAsync(int cartItemId);
    }
}
