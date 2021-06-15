using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Application.ViewModels.Cart
{
    public class CartItemForListVM
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        [Display(Name = "Image")]
        public string ImageToDisplay { get; set; }

        [Display(Name = "Total Price")]
        public decimal TotalPrice { get { return Price * Quantity; } }
    }
}
