using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.AdminPanel
{
    public class CustomerDetailsVM : IMapFrom<Domain.Models.Customer>
    {
        public List<CartItemForCustomerDetailsVM> CartItems { get; set; }

        public List<OrderForCustomerDetailsVM> Orders { get; set; }

        public int Id { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        public string City { get; set; }

        [Display(Name = "Postal code")]
        public string PostalCode { get; set; }

        public string Address { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Customer, CustomerDetailsVM>()
            .ForMember(x => x.Email, y => y.MapFrom(src => src.AppUser.Email))
            .ForMember(x => x.PhoneNumber, y => y.MapFrom(src => src.AppUser.PhoneNumber));
        }
    }
}
