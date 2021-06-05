using AutoMapper;
using EcommerceApp.Application.Mapping;
using Microsoft.AspNetCore.Http;

namespace EcommerceApp.Application.ViewModels
{
    public class ProductVM : IMapFrom<Domain.Models.Product>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public byte[] Image { get; set; }
        public string ImageToDisplay { get; set; }
        public IFormFile FormFileImage { get; set; }
        public string CategoryName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Product, ProductVM>().ReverseMap();
        }
    }
}
