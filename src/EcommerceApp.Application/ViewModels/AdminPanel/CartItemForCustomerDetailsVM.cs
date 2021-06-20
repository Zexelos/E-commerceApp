using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.AdminPanel
{
    public class CartItemForCustomerDetailsVM : IMapFrom<Domain.Models.CartItem>
    {
        public int ProductId { get; set; }
        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        [Display(Name = "Image")]
        public string ImageToDisplay { get; set; }
        public byte[] ImageByteArray { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalPrice { get { return Price * Quantity; } }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.CartItem, CartItemForCustomerDetailsVM>()
            .ForMember(x => x.Name, y => y.MapFrom(src => src.Product.Name))
            .ForMember(x => x.Price, y => y.MapFrom(src => src.Product.UnitPrice))
            .ForMember(x => x.ImageByteArray, y => y.MapFrom(src => src.Product.Image));
        }
    }
}
