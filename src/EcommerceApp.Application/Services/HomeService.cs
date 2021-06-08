using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels;

namespace EcommerceApp.Application.Services
{
    public class HomeService : IHomeService
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public HomeService(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        public async Task<HomeVM> GetHomeVMAsync()
        {
            return new HomeVM
            {
                Categories = await _categoryService.GetCategoriesAsync(),
                Products = GetRandomAmountOfProductVMsFromList(await _productService.GetProductsWithImageAsync(), 8)
            };
        }

        private List<ProductVM> GetRandomAmountOfProductVMsFromList(List<ProductVM> list, int itemAmount)
        {
            var random = new Random();
            var result = new List<ProductVM>();
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
