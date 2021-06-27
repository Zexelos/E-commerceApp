using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EcommerceApp.Application.Interfaces;
using EcommerceApp.Application.ViewModels.EmployeePanel;
using EcommerceApp.Domain.Interfaces;
using EcommerceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;
        private readonly IMapper _mapper;
        private readonly IImageConverterService _imageConverterService;
        private readonly IPaginatorService<CategoryForListVM> _paginatorService;

        public CategoryService(
            IMapper mapper,
            ICategoryRepository repository,
            IImageConverterService imageConverterService,
            IPaginatorService<CategoryForListVM> paginatorService)
        {
            _mapper = mapper;
            _repository = repository;
            _imageConverterService = imageConverterService;
            _paginatorService = paginatorService;
        }

        public async Task AddCategoryAsync(CategoryVM categoryVM)
        {
            var category = _mapper.Map<Category>(categoryVM);
            category.Image = await _imageConverterService.GetByteArrayFromFormFile(categoryVM.FormFileImage);
            await _repository.AddCategoryAsync(category);
        }

        public async Task<CategoryVM> GetCategoryAsync(int id)
        {
            var category = await _repository.GetCategoryAsync(id);
            var categoryVM = _mapper.Map<CategoryVM>(category);
            categoryVM.ImageToDisplay = _imageConverterService.GetImageStringFromByteArray(category.Image);
            return categoryVM;
        }

        public async Task<Dictionary<int, string>> GetCategoriesNamesAsync()
        {
            return await _repository.GetCategories().ToDictionaryAsync(x => x.Id, x => x.Name);
        }

        public async Task<CategoryListVM> GetPaginatedCategoriesAsync(int pageSize, int pageNumber)
        {
            var categories = _repository.GetCategories().ProjectTo<CategoryForListVM>(_mapper.ConfigurationProvider);
            var paginatedVM = await _paginatorService.CreateAsync(categories, pageNumber, pageSize);
            return _mapper.Map<CategoryListVM>(paginatedVM);
        }

        public async Task UpdateCategoryAsync(CategoryVM categoryVM)
        {
            var category = _mapper.Map<Category>(categoryVM);
            category.Image = await _imageConverterService.GetByteArrayFromFormFile(categoryVM.FormFileImage);
            await _repository.UpdateCategoryAsync(category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await _repository.DeleteCategoryAsync(id);
        }
    }
}
