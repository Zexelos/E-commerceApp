using System.Collections.Generic;

namespace EcommerceApp.Application.ViewModels.AdminPanel
{
    public class CustomerListVM
    {
        public List<CustomerForListVM> Customers { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
