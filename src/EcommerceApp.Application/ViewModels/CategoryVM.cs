using AutoMapper;
using EcommerceApp.Application.Mapping;
using Microsoft.AspNetCore.Http;

namespace EcommerceApp.Application.ViewModels
{
    public class CategoryVM : IMapFrom<Domain.Models.Category>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }
        public string ImageToDisplay { get; set; }
        public IFormFile FormFileImage { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Category, CategoryVM>().ReverseMap();
        }
    }
}
