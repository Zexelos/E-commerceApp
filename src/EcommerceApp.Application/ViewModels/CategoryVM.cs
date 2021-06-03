using System;
using AutoMapper;
using EcommerceApp.Application.Mapping;
using FluentValidation;

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

    public class CategoryVMValidator : AbstractValidator<CategoryVM>
    {
        public CategoryVMValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Name).NotEmpty().MinimumLength(2).MaximumLength(50);
            RuleFor(x => x.Description).NotEmpty().MinimumLength(2).MaximumLength(200);
        }
    }
}
