using System;
using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels
{
    public class CategoriesVM : IMapFrom<Domain.Models.Category>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Category, CategoriesVM>();
        }
    }
}
