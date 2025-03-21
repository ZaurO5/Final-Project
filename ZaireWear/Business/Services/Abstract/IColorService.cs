using Business.ViewModels.Color;
using Core.Entities;

namespace Business.Services.Abstract;

public interface IColorService
{
    Task<ColorIndexVM> GetAllAsync();
    Task<Color> GetByIdAsync(int id);
    Task<bool> CreateAsync(ColorCreateVM model);
    Task<ColorUpdateVM> UpdateAsync(int id);
    Task<bool> UpdateAsync(int id, ColorUpdateVM model);
    Task<bool> DeleteAsync(int id);
}
