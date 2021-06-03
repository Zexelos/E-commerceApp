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
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public ProductService(IMapper mapper, IProductRepository repository, ICategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _productRepository = repository;
            _categoryRepository = categoryRepository;
        }

        public async Task AddProductAsync(ProductVM productVM)
        {
            var product = _mapper.Map<Product>(productVM);
            await _productRepository.AddProductAsync(product);
        }

        public async Task<ProductVM> GetProductAsync(int id)
        {
            var product = await _productRepository.GetProductAsync(id);
            var categories = await _categoryRepository.GetCategoriesAsync();
            var categoryMap = _mapper.Map<List<CategoriesVM>>(categories);
            var productMap = _mapper.Map<ProductVM>(product);
            productMap.Categories = categoryMap;
            return productMap;
        }

        public async Task<List<ProductVM>> GetProductsAsync()
        {
            var products = (await _productRepository.GetProductsAsync()).ToList();
            return _mapper.Map<List<ProductVM>>(products);
        }

        public async Task UpdateProductAsync(ProductVM productVM)
        {
            var product = _mapper.Map<Product>(productVM);
            await _productRepository.UpdateProductAsync(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            await _productRepository.DeleteProductAsync(id);
        }
    }
}
