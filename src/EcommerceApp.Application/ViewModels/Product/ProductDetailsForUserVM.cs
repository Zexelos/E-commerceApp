using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.Product
{
    public class ProductDetailsForUserVM : IMapFrom<Domain.Models.Product>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [Display(Name = "Price")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Units in stock")]
        public int UnitsInStock { get; set; }

        [Display(Name = "Image")]
        public string ImageToDisplay { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Product, ProductDetailsForUserVM>().ReverseMap();
        }
    }
}
