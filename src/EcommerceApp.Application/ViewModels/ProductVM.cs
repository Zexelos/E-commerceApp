using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EcommerceApp.Application.Mapping;
using Microsoft.AspNetCore.Http;

namespace EcommerceApp.Application.ViewModels
{
    public class ProductVM : IMapFrom<Domain.Models.Product>
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Units In Stock")]
        public int UnitsInStock { get; set; }

        public byte[] Image { get; set; }

        [Display(Name = "Image")]
        public string ImageToDisplay { get; set; }

        [Display(Name = "Image")]
        public IFormFile FormFileImage { get; set; }

        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Product, ProductVM>().ReverseMap();
        }
    }
}
