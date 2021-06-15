using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EcommerceApp.Application.ViewModels.Cart;

namespace EcommerceApp.Application.ViewModels.Order
{
    public class OrderCheckoutVM
    {
        public int CartId { get; set; }

        public int CustomerId { get; set; }

        public List<CartItemForListVM> CartItems { get; set; }

        [Display(Name = "Total Price")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalPrice { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Postal code")]
        public string PostalCode { get; set; }

        [Display(Name = "Adress")]
        public string Address { get; set; }

        [Display(Name = "Phone number")]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
