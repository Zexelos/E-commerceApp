using System.Net.Mime;
using System.IO;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;

namespace EcommerceApp.Application.Services
{
    public class ImageConverterService : IImageConverterService
    {
        public async Task<byte[]> GetByteArrayFromImage(IFormFile formFile)
        {
            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream);
                return stream.ToArray();
            }
        }

        public string GetImageStringFromByteArray(byte[] byteArray)
        {
            if (byteArray.Length > 0)
            {
                string imageData = Convert.ToBase64String(byteArray);
                return string.Format("data:image/jpg;base64,{0}", imageData);
            }
            else
            {
                return "No photo available";
            }
        }
    }
}
