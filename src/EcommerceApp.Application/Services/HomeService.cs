using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.Home;
using EcommerceApp.Application.ViewModels.Product;

namespace EcommerceApp.Application.Services
{
    public class HomeService : IHomeService
    {
        private readonly IProductService _productService;

        public HomeService(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<HomeVM> GetHomeVMAsync()
        {
            return new HomeVM
            {
                Products = await _productService.GetRandomProductsWithImageAsync(8)
            };
        }

    }
}
