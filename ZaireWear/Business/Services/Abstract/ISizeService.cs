using Business.ViewModels.Size;
using Core.Entities;

namespace Business.Services.Abstract;

public interface ISizeService
{
    Task<SizeIndexVM> GetAllAsync();
    Task<Size> GetByIdAsync(int id);
    Task<bool> CreateAsync(SizeCreateVM model);
    Task<SizeUpdateVM> UpdateAsync(int id);
    Task<bool> UpdateAsync(int id, SizeUpdateVM model);
    Task<bool> DeleteAsync(int id);
}
