using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EcommerceApp.Application.Mapping;
using EcommerceApp.Application.ViewModels.Cart;

namespace EcommerceApp.Application.ViewModels.Order
{
    public class OrderCheckoutVM : IMapFrom<Domain.Models.Customer>
    {
        public int CartId { get; set; }

        public int CustomerId { get; set; }
        public List<CartItemForOrderCheckoutVM> CartItems { get; set; }

        [Display(Name = "Total Price")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalPrice
        {
            get
            {
                var result = 0m;
                foreach (var item in CartItems)
                {
                    result += item.TotalPrice;
                }
                return result;
            }
        }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Postal code")]
        public string PostalCode { get; set; }

        [Display(Name = "Adress")]
        public string Address { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Phone number")]
        [Phone]
        public string PhoneNumber { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Customer, OrderCheckoutVM>()
            .ForMember(x => x.CartItems, y => y.MapFrom(src => src.Cart.CartItems))
            .ForMember(x => x.CustomerId, y => y.MapFrom(src => src.Id))
            .ForMember(x => x.CartId, y => y.MapFrom(src => src.Cart.Id))
            .ForMember(x => x.Email, y => y.MapFrom(src => src.AppUser.Email))
            .ForMember(x => x.PhoneNumber, y => y.MapFrom(src => src.AppUser.PhoneNumber));
        }
    }
}
