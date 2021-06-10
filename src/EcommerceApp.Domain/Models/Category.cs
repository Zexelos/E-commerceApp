using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApp.Domain.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(200, MinimumLength = 2)]
        public string Description { get; set; }

        public byte[] Image { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
