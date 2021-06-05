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
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;
        private readonly IImageConverterService _imageConverterService;

        public CategoryService(IMapper mapper, ICategoryRepository repository, IImageConverterService imageConverterService)
        {
            _mapper = mapper;
            _repository = repository;
            _imageConverterService = imageConverterService;
        }

        public async Task AddCategoryAsync(CategoryVM categoryVM)
        {
            var category = _mapper.Map<Category>(categoryVM);
            category.Image = await _imageConverterService.GetByteArrayFromImage(categoryVM.FormFileImage);
            await _repository.AddCategoryAsync(category);
        }

        public async Task<CategoryVM> GetCategoryAsync(int id)
        {
            var category = await _repository.GetCategoryAsync(id);
            var categoryVM = _mapper.Map<CategoryVM>(category);
            categoryVM.ImageToDisplay = _imageConverterService.GetImageStringFromByteArray(categoryVM.Image);
            return categoryVM;
        }

        public async Task<List<CategoryVM>> GetCategoriesAsync()
        {
            var categories = (await _repository.GetCategoriesAsync()).ToList();
            return _mapper.Map<List<CategoryVM>>(categories);
        }

        public async Task UpdateCategoryAsync(CategoryVM categoryVM)
        {
            var category = _mapper.Map<Category>(categoryVM);
            category.Image = await _imageConverterService.GetByteArrayFromImage(categoryVM.FormFileImage);
            await _repository.UpdateCategoryAsync(category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await _repository.DeleteCategoryAsync(id);
        }
    }
}
