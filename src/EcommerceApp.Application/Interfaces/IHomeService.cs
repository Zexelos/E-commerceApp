using System.Threading.Tasks;
using EcommerceApp.Application.ViewModels;

namespace EcommerceApp.Application.Interfaces
{
    public interface IHomeService
    {
        Task<HomeVM> GetHomeVMAsync();
    }
}
