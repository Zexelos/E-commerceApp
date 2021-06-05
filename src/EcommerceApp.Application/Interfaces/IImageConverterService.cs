using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EcommerceApp.Application.Interfaces
{
    public interface IImageConverterService
    {
        Task<byte[]> GetByteArrayFromImage(IFormFile formFile);
        string GetImageStringFromByteArray(byte[] byteArray);
    }
}
