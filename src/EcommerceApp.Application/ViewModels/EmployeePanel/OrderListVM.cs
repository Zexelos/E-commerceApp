using System.Collections.Generic;
using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.EmployeePanel
{
    public class OrderListVM : IMapFrom<PaginatedVM<OrderForListVM>>
    {
        public List<OrderForListVM> Orders { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PaginatedVM<OrderForListVM>, OrderListVM>()
            .ForMember(x => x.Orders, y => y.MapFrom(src => src.Items));
        }
    }
}
