using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Domain.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        [InverseProperty(nameof(CartItem.Cart))]
        public ICollection<CartItem> CartItems { get; set; }
    }
}