using System.Collections.Generic;
using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.Order
{
    public class CustomerOrderListVM : IMapFrom<PaginatedVM<CustomerOrderForListVM>>
    {
        public List<CustomerOrderForListVM> Orders { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PaginatedVM<CustomerOrderForListVM>, CustomerOrderListVM>()
            .ForMember(x => x.Orders, y => y.MapFrom(src => src.Items));
        }
    }
}