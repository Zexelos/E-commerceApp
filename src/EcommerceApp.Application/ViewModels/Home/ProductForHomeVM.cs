using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.Home
{
    public class ProductForHomeVM : IMapFrom<Domain.Models.Product>
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Image")]
        public string ImageToDisplay { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Product, ProductForHomeVM>().ReverseMap();
        }
    }
}
