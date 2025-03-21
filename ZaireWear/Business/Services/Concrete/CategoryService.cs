using Business.Services.Abstract;
using Business.ViewModels.Category;
using Core.Entities;
using Data.Repositories.Abstract;
using Data.UnitOfWork;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Business.Services.Concrete;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ModelStateDictionary _modelState;

    public CategoryService(ICategoryRepository categoryRepository,
                           IUnitOfWork unitOfWork,
                           IActionContextAccessor actionContextAccessor)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _modelState = actionContextAccessor.ActionContext.ModelState;
    }

        public async Task<CategoryIndexVM> GetAllAsync()
        {
            try
            {
                var categories = await _categoryRepository.GetAllAsync();
                return new CategoryIndexVM { Categories = categories };
            }
            catch (Exception ex)
            {
                _modelState.AddModelError(string.Empty, "Error retrieving categories");
                return new CategoryIndexVM { Categories = new List<Category>() };
            }
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            try
            {
                return await _categoryRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _modelState.AddModelError(string.Empty, "Error retrieving category");
                return null;
            }
        }

    public async Task<bool> CreateAsync(CategoryCreateVM model)
    {
        if (!_modelState.IsValid) return false;

        try
        {
            var existingCategory = await _categoryRepository.GetByNameAsync(model.Name);
            if (existingCategory != null)
            {
                _modelState.AddModelError("Name", "Category already exists");
                return false;
            }

            var newCategory = new Category
            {
                Name = model.Name,
                CreatedAt = DateTime.Now
            };

            await _categoryRepository.CreateAsync(newCategory);
            await _unitOfWork.CommitAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating category: {ex.Message}");
            _modelState.AddModelError(string.Empty, "Error creating category");
            return false;
        }
    }

    public async Task<CategoryUpdateVM> UpdateAsync(int id)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    _modelState.AddModelError(string.Empty, "Category not found");
                    return null;
                }

                return new CategoryUpdateVM { Name = category.Name };
            }
            catch (Exception ex)
            {
                _modelState.AddModelError(string.Empty, "Error retrieving category");
                return null;
            }
    }

        public async Task<bool> UpdateAsync(int id, CategoryUpdateVM model)
        {
            if (!_modelState.IsValid) return false;

            try
            {
                var category = await _categoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    _modelState.AddModelError(string.Empty, "Category not found");
                    return false;
                }

                var existingCategory = await _categoryRepository.GetByNameAsync(model.Name);
                if (existingCategory != null && existingCategory.Id != id)
                {
                    _modelState.AddModelError("Name", "Category already exists");
                    return false;
                }

                category.Name = model.Name;
                category.ModifiedAt = DateTime.Now;

                _categoryRepository.Update(category);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _modelState.AddModelError(string.Empty, "Error updating category");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    _modelState.AddModelError(string.Empty, "Category not found");
                    return false;
                }

                _categoryRepository.Delete(category);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _modelState.AddModelError(string.Empty, "Error deleting category");
                return false;
            }
        }
}

