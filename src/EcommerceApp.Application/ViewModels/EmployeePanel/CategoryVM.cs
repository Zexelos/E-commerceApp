using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EcommerceApp.Application.Mapping;
using Microsoft.AspNetCore.Http;

namespace EcommerceApp.Application.ViewModels.EmployeePanel
{
    public class CategoryVM : IMapFrom<Domain.Models.Category>
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Image")]
        public string ImageToDisplay { get; set; }

        [Display(Name = "Image")]
        public IFormFile FormFileImage { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Category, CategoryVM>().ReverseMap();
        }
    }
}
