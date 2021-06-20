using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.AdminPanel
{
    public class CustomerForListVM : IMapFrom<Domain.Models.Customer>
    {
        public int Id { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        public string City { get; set; }

        [Display(Name = "Postal code")]
        public string PostalCode { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Customer, CustomerForListVM>()
            .ForMember(x => x.Email, y => y.MapFrom(src => src.AppUser.Email))
            .ForMember(x => x.PhoneNumber, y => y.MapFrom(src => src.AppUser.PhoneNumber));
        }
    }
}
