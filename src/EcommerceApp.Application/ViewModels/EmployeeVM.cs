using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels
{
    public class EmployeeVM : IMapFrom<Domain.Model.Employee>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Model.Employee, EmployeeVM>().ReverseMap();
        }
    }
}