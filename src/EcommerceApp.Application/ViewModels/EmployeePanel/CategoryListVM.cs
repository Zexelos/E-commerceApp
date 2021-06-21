using System.Collections.Generic;
using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.EmployeePanel
{
    public class CategoryListVM : IMapFrom<PaginatedVM<CategoryForListVM>>
    {
        public List<CategoryForListVM> Categories { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<PaginatedVM<CategoryForListVM>, CategoryListVM>()
            .ForMember(x => x.Categories, y => y.MapFrom(src => src.Items));
        }
    }
}
