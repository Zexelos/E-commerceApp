using System.Linq;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Domain.Interfaces
{
    public interface ICartRepository
    {
        IQueryable<Cart> GetCarts();
    }
}
