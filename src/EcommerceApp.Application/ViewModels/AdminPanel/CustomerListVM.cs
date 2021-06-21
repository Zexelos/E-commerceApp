using System.Collections.Generic;
using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.AdminPanel
{
    public class CustomerListVM : IMapFrom<PaginatedVM<CustomerForListVM>>
    {
        public List<CustomerForListVM> Customers { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PaginatedVM<CustomerForListVM>, CustomerListVM>()
            .ForMember(x => x.Customers, y => y.MapFrom(src => src.Items));
        }
    }
}
