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

        public CategoryService(IMapper mapper, ICategoryRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task AddCategoryAsync(CategoryVM categoryVM)
        {
            var category = _mapper.Map<Category>(categoryVM);
            await _repository.AddCategoryAsync(category);
        }

        public async Task<CategoryVM> GetCategoryAsync(int id)
        {
            var category = await _repository.GetCategoryAsync(id);
            return _mapper.Map<CategoryVM>(category);
        }

        public async Task<List<CategoryVM>> GetCategoriesAsync()
        {
            var categories = (await _repository.GetCategoriesAsync()).ToList();
            return _mapper.Map<List<CategoryVM>>(categories);
        }

        public async Task UpdateCategoryAsync(CategoryVM categoryVM)
        {
            var category = _mapper.Map<Category>(categoryVM);
            await _repository.UpdateCategoryAsync(category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await _repository.DeleteCategoryAsync(id);
        }
    }
}
