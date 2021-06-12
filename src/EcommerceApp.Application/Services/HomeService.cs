using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.Home;
using EcommerceApp.Application.ViewModels.EmployeePanel;

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

        private static List<ProductForHomeVM> GetRandomAmountOfProductVMsFromList(List<ProductForHomeVM> list, int itemAmount)
        {
            var random = new Random();
            var result = new List<ProductForHomeVM>();
            var checkList = new List<int>();
            if (list.Count <= itemAmount)
            {
                return list;
            }
            while (result.Count <= itemAmount)
            {
                var currentRandom = random.Next(list.Count);
                if (!checkList.Contains(currentRandom))
                {
                    result.Add(list[currentRandom]);
                    checkList.Add(currentRandom);
                }
            }
            return result;
        }
    }
}
