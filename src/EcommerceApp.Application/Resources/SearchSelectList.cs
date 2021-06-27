using System.Collections.Generic;
using EcommerceApp.Application.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcommerceApp.Application.Resources
{
    public class SearchSelectList : ISearchSelectList
    {
        public List<SelectListItem> CategorySelectList { get; } = new()
        {
            new SelectListItem { Value = "Id", Text = "Id" },
            new SelectListItem { Value = "Name", Text = "Name" },
        };

        public List<SelectListItem> ProductSelectList { get; } = new()
        {
            new SelectListItem { Value = "Id", Text = "Id" },
            new SelectListItem { Value = "Name", Text = "Name" },
            new SelectListItem { Value = "UnitPrice", Text = "Unit price" },
            new SelectListItem { Value = "UnitsInStock", Text = "Units in stock" },
            new SelectListItem { Value = "CategoryName", Text = "Category name" }
        };

        public List<SelectListItem> EmployeeSelectList { get; } = new()
        {
            new SelectListItem { Value = "Id", Text = "Id" },
            new SelectListItem { Value = "FirstName", Text = "First name" },
            new SelectListItem { Value = "LastName", Text = "Last name" },
            new SelectListItem { Value = "Email", Text = "Email" },
            new SelectListItem { Value = "Position", Text = "Position" }
        };

        public List<SelectListItem> CustomerSelectList { get; } = new()
        {
            new SelectListItem { Value = "Id", Text = "Id" },
            new SelectListItem { Value = "FirstName", Text = "First name" },
            new SelectListItem { Value = "LastName", Text = "Last name" },
            new SelectListItem { Value = "Email", Text = "Email" },
            new SelectListItem { Value = "City", Text = "City" },
            new SelectListItem { Value = "PostalCode", Text = "Postal code" },
            new SelectListItem { Value = "Address", Text = "Address" },
            new SelectListItem { Value = "PhoneNumber", Text = "Phone number" }
        };

        public List<SelectListItem> OrdersSelectList { get; } = new()
        {
            new SelectListItem { Value = "Id", Text = "Id" },
            new SelectListItem { Value = "CustomerId", Text = "Customer Id" },
            new SelectListItem { Value = "Price", Text = "Price" },
            new SelectListItem { Value = "ShipFirstName", Text = "First name" },
            new SelectListItem { Value = "ShipLastName", Text = "Last name" },
            new SelectListItem { Value = "ShipCity", Text = "City" },
            new SelectListItem { Value = "ShipPostalCode", Text = "Postal code" },
            new SelectListItem { Value = "ShipAddress", Text = "Address" },
            new SelectListItem { Value = "ContactEmail", Text = "Email" },
            new SelectListItem { Value = "ContactPhoneNumber", Text = "Phone number" }
        };

        public List<SelectListItem> PageSizeSelectList { get; } = new()
        {
            new SelectListItem { Value = "5", Text = "5" },
            new SelectListItem { Value = "10", Text = "10" },
            new SelectListItem { Value = "20", Text = "20" },
            new SelectListItem { Value = "50", Text = "50" }
        };
    }
}