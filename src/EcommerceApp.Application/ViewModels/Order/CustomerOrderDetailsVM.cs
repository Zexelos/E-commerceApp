using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.Order
{
    public class CustomerOrderDetailsVM : IMapFrom<Domain.Models.Order>
    {
        public List<OrderItemForCustomerOrderDetailVM> OrderItems { get; set; }

        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Price { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "First name")]
        public string ShipFirstName { get; set; }

        [Display(Name = "Last name")]
        public string ShipLastName { get; set; }

        [Display(Name = "City")]
        public string ShipCity { get; set; }

        [Display(Name = "Postal code")]
        public string ShipPostalCode { get; set; }

        [Display(Name = "Address")]
        public string ShipAddress { get; set; }

        [Display(Name = "Email")]
        public string ContactEmail { get; set; }

        [Display(Name = "Phone number")]
        public string ContactPhoneNumber { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Order, CustomerOrderDetailsVM>()
            .ForMember(x => x.OrderItems, y => y.MapFrom(src => src.OrderItems));
        }
    }
}
