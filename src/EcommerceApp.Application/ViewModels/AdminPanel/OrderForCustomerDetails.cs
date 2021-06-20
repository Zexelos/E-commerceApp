using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using EcommerceApp.Application.Mapping;

namespace EcommerceApp.Application.ViewModels.AdminPanel
{
    public class OrderForCustomerDetails : IMapFrom<Domain.Models.Order>
    {
        public int Id { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime CreateDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Price { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Order, OrderForCustomerDetails>();
        }
    }
}
