using System.Collections.Generic;

namespace EcommerceApp.Application.ViewModels
{
    public class HomeVM
    {
        public List<CategoryVM> Categories { get; set; }
        public List<ProductVM> Products { get; set; }
    }
}
