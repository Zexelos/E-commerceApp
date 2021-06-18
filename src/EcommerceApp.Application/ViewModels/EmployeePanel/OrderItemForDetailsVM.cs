using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.EmployeePanel
{
    public class OrderItemForDetailsVM : IMapFrom<Domain.Models.OrderItem>
    {
        [Display(Name = "Product Id")]
        public int ProductId { get; set; }

        [Display(Name = "Product name")]
        public string ProductName { get; set; }
        public int Quantity { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.OrderItem, OrderItemForDetailsVM>().ForMember(x => x.ProductName, y => y.MapFrom(src => src.Product.Name));
        }
    }
}
