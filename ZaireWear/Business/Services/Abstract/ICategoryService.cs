using Business.ViewModels.Category;
using Core.Entities;

namespace Business.Services.Abstract;

public interface ICategoryService
{
    Task<CategoryIndexVM> GetAllAsync();
    Task<Category> GetByIdAsync(int id);
    Task<bool> CreateAsync(CategoryCreateVM model);
    Task<CategoryUpdateVM> UpdateAsync(int id);
    Task<bool> UpdateAsync(int id, CategoryUpdateVM model);
    Task<bool> DeleteAsync(int id);
}
