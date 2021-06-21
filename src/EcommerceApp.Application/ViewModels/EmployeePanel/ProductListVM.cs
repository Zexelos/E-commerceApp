using System.Collections.Generic;
using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.EmployeePanel
{
    public class ProductListVM : IMapFrom<PaginatedVM<ProductForListVM>>
    {
        public List<ProductForListVM> Products { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PaginatedVM<ProductForListVM>, ProductListVM>()
            .ForMember(x => x.Products, y => y.MapFrom(src => src.Items));
        }
    }
}