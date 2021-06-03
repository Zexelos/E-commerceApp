using System;
using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels
{
    public class CategoryVM : IMapFrom<Domain.Models.Employee>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Employee, EmployeeVM>().ReverseMap();
        }
    }
}
