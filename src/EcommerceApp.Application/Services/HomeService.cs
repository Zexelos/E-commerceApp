using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.Home;

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
            return await _productService.GetRandomProductsWithImageAsync(8);
        }
    }
}
