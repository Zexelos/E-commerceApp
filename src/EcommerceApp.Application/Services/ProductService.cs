using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;

namespace EcommerceApp.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IImageConverterService _imageConverterService;

        public ProductService(IMapper mapper, IProductRepository repository, ICategoryRepository categoryRepository, IImageConverterService imageConverterService)
        {
            _mapper = mapper;
            _productRepository = repository;
            _categoryRepository = categoryRepository;
            _imageConverterService = imageConverterService;
        }

        public async Task AddProductAsync(ProductVM productVM)
        {
            var product = _mapper.Map<Product>(productVM);
            var category = await _categoryRepository.GetCategoryAsync(product.CategoryName);
            product.CategoryId = category.Id;
            product.Image = await _imageConverterService.GetByteArrayFromImage(productVM.FormFileImage);
            await _productRepository.AddProductAsync(product);
        }

        public async Task<ProductVM> GetProductAsync(int id)
        {
            var product = await _productRepository.GetProductAsync(id);
            var productVM = _mapper.Map<ProductVM>(product);
            productVM.ImageToDisplay = _imageConverterService.GetImageStringFromByteArray(productVM.Image);
            return productVM;
        }

        public async Task<List<ProductVM>> GetProductsAsync()
        {
            var products = (await _productRepository.GetProductsAsync()).ToList();
            return _mapper.Map<List<ProductVM>>(products);
        }

        public async Task UpdateProductAsync(ProductVM productVM)
        {
            var product = _mapper.Map<Product>(productVM);
            var category = await _categoryRepository.GetCategoryAsync(product.CategoryName);
            product.CategoryId = category.Id;
            await _productRepository.UpdateProductAsync(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            await _productRepository.DeleteProductAsync(id);
        }
    }
}
