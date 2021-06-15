using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Application.ViewModels.Cart
{
    public class ListCartItemForListVM
    {
        public int CartId { get; set; }

        public List<CartItemForListVM> CartItems { get; set; }

        [Display(Name = "Total Price")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalPrice
        {
            get
            {
                var result = 0m;
                {
                    foreach (var cartItem in CartItems)
                    {
                        result += cartItem.TotalPrice;
                    }
                }
                return result;
            }
        }
    }
}
