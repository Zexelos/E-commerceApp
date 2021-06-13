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
                Products = GetRandomAmountOfProductVMsFromList(await _productService.GetProductsWithImageAsync(), 8)
            };
        }

        private static ListProductDetailsForUserVM GetRandomAmountOfProductVMsFromList(ListProductDetailsForUserVM list, int itemAmount)
        {
            var random = new Random();
            var result = new ListProductDetailsForUserVM();
            var checkList = new List<int>();
            if (list.Products.Count <= itemAmount)
            {
                return list;
            }
            while (result.Products.Count <= itemAmount)
            {
                var currentRandom = random.Next(list.Products.Count);
                if (!checkList.Contains(currentRandom))
                {
                    result.Products.Add(list.Products[currentRandom]);
                    checkList.Add(currentRandom);
                }
            }
            return result;
        }
    }
}
