using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels;

namespace EcommerceApp.Application.Interfaces
{
    public interface ISearchService
    {
        Task<List<CategoryVM>> CategorySearchAsync(string selectedValue, string searchString);
        Task<List<ProductVM>> ProductSearchAsync(string selectedValue, string searchString);
    }
}
