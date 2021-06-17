using System;
using System.Linq;
using System.Threading.Tasks;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Infrastructure.Repositories
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly AppDbContext _context;

        public CartItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddCartItemAsync(CartItem cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task<CartItem> GetCartItemAsync(int id)
        {
            return await _context.CartItems.FindAsync(id);
        }

        public IQueryable<CartItem> GetCartItems()
        {
            return _context.CartItems.AsQueryable();
        }

        public IQueryable<CartItem> GetCartItemsByCartId(int cartId)
        {
            return _context.CartItems.Where(x => x.CartId == cartId).AsQueryable();
        }

        public async Task UpdateCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCartItemAsync(int id)
        {
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem != null)
            {
                _context.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteCartItemsByCartIdAsync(int cartId)
        {
            var cartItems = _context.CartItems.Where(x => x.CartId == cartId);
            if (cartItems != null)
            {
                _context.RemoveRange(cartItems);
                await _context.SaveChangesAsync();
            }
        }
    }
}
