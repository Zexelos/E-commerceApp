using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.EmployeePanel
{
    public class ProductForListVM : IMapFrom<Domain.Models.Product>
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Units In Stock")]
        public int UnitsInStock { get; set; }

        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Product, ProductForListVM>().ReverseMap();
        }
    }
}
