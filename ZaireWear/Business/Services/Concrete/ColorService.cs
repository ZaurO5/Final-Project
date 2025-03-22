using Business.Services.Abstract;
using Business.ViewModels.Color;
using Core.Entities;
using Data.Repositories.Abstract;
using Data.UnitOfWork;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Business.Services.Concrete;

public class ColorService : IColorService
{
    private readonly IColorRepository _colorRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ModelStateDictionary _modelState;

    public ColorService(
        IColorRepository colorRepository,
        IUnitOfWork unitOfWork,
        IActionContextAccessor actionContextAccessor)
    {
        _colorRepository = colorRepository;
        _unitOfWork = unitOfWork;
        _modelState = actionContextAccessor.ActionContext.ModelState;
    }

    public async Task<ColorIndexVM> GetAllAsync()
    {
        try
        {
            var colors = await _colorRepository.GetAllAsync();
            return new ColorIndexVM { Colors = colors };
        }
        catch (Exception ex)
        {
            _modelState.AddModelError(string.Empty, "Error retrieving colors");
            return new ColorIndexVM { Colors = new List<Color>() };
        }
    }

    public async Task<Color> GetByIdAsync(int id)
    {
        try
        {
            return await _colorRepository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            _modelState.AddModelError(string.Empty, "Error retrieving color");
            return null;
        }
    }

    public async Task<bool> CreateAsync(ColorCreateVM model)
    {
        if (!_modelState.IsValid) return false;

        try
        {
            var existingColor = await _colorRepository.GetByNameAsync(model.Name);
            if (existingColor != null)
            {
                _modelState.AddModelError("Name", "Color already exists");
                return false;
            }

            var newColor = new Color
            {
                Name = model.Name,
                HexCode = model.HexCode,
                CreatedAt = DateTime.UtcNow
            };

            await _colorRepository.CreateAsync(newColor);
            await _unitOfWork.CommitAsync();
            return true;
        }
        catch (Exception ex)
        {
            _modelState.AddModelError(string.Empty, "Error creating color");
            return false;
        }
    }

    public async Task<ColorUpdateVM> UpdateAsync(int id)
    {
        try
        {
            var color = await _colorRepository.GetByIdAsync(id);
            if (color == null)
            {
                _modelState.AddModelError(string.Empty, "Color not found");
                return null;
            }

            return new ColorUpdateVM
            {
                Name = color.Name,
                HexCode = color.HexCode
            };
        }
        catch (Exception ex)
        {
            _modelState.AddModelError(string.Empty, "Error retrieving color");
            return null;
        }
    }

    public async Task<bool> UpdateAsync(int id, ColorUpdateVM model)
    {
        if (!_modelState.IsValid) return false;

        try
        {
            var color = await _colorRepository.GetByIdAsync(id);
            if (color == null)
            {
                _modelState.AddModelError(string.Empty, "Color not found");
                return false;
            }

            var existingColor = await _colorRepository.GetByNameAsync(model.Name);
            if (existingColor != null && existingColor.Id != id)
            {
                _modelState.AddModelError("Name", "Color already exists");
                return false;
            }

            color.Name = model.Name;
            color.HexCode = model.HexCode;
            color.ModifiedAt = DateTime.UtcNow;

            _colorRepository.Update(color);
            await _unitOfWork.CommitAsync();
            return true;
        }
        catch (Exception ex)
        {
            _modelState.AddModelError(string.Empty, "Error updating color");
            return false;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var color = await _colorRepository.GetByIdAsync(id);
            if (color == null)
            {
                _modelState.AddModelError(string.Empty, "Color not found");
                return false;
            }

            _colorRepository.Delete(color);
            await _unitOfWork.CommitAsync();
            return true;
        }
        catch (Exception ex)
        {
            _modelState.AddModelError(string.Empty, "Error deleting color");
            return false;
        }
    }
}
