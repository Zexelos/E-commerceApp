using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Domain.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Product Product { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int CartId { get; set; }

        [ForeignKey(nameof(CartId))]
        [InverseProperty("CartItems")]
        public Cart Cart { get; set; }
    }
}