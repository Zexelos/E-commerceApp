using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.Cart
{
    public class CartItemListVM : IMapFrom<Domain.Models.Cart>
    {
        public int CustomerId { get; set; }

        public List<CartItemForListVM> CartItems { get; set; }

        [Display(Name = "Total Price")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalPrice { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Cart, CartItemListVM>()
            .ForMember(x => x.CartItems, y => y.MapFrom(src => src.CartItems));
        }
    }
}
