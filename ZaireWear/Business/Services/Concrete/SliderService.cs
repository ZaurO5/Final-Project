using Business.Services.Abstract;
using Business.Utilities.File;
using Business.ViewModels.Slider;
using Core.Entities;
using Data.Repositories.Abstract;
using Data.UnitOfWork;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Business.Services.Concrete;

public class SliderService : ISliderService
{
    private readonly ISliderRepository _sliderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;
    private readonly ModelStateDictionary _modelState;

    public SliderService(
        ISliderRepository sliderRepository,
        IUnitOfWork unitOfWork,
        IFileService fileService,
        IActionContextAccessor actionContextAccessor)
    {
        _sliderRepository = sliderRepository;
        _unitOfWork = unitOfWork;
        _fileService = fileService;
        _modelState = actionContextAccessor.ActionContext.ModelState;
    }

    public async Task<SliderIndexVM> GetAllAsync()
    {
        try
        {
            var sliders = await _sliderRepository.GetAllAsync();
            return new SliderIndexVM { Sliders = sliders };
        }
        catch (Exception)
        {
            _modelState.AddModelError(string.Empty, "Error retrieving sliders.");
            return new SliderIndexVM { Sliders = new List<Slider>() };
        }
    }

    public Task<SliderCreateVM> CreateAsync() => Task.FromResult(new SliderCreateVM());

    public async Task<bool> CreateAsync(SliderCreateVM model)
    {
        if (!_modelState.IsValid) return false;

        try
        {
            var existingSliders = await _sliderRepository.GetAllAsync();
            if (existingSliders.Any(s => s.Order == model.Order))
            {
                _modelState.AddModelError("Order", "Order number is taken.");
                return false;
            }

            var imagePath = _fileService.Upload(model.ImagePath, "assets/images/sliders");
            var slider = new Slider
            {
                Title = model.Title,
                Subtitle = model.Subtitle,
                ImagePath = imagePath,
                Order = model.Order,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _sliderRepository.CreateAsync(slider);
            await _unitOfWork.CommitAsync();
            return true;
        }
        catch (Exception)
        {
            _modelState.AddModelError(string.Empty, "Error creating slider.");
            return false;
        }
    }

    public async Task<SliderUpdateVM> UpdateAsync(int id)
    {
        try
        {
            var slider = await _sliderRepository.GetByIdAsync(id);
            if (slider == null)
            {
                _modelState.AddModelError(string.Empty, "Slider not found.");
                return null;
            }

            return new SliderUpdateVM
            {
                Title = slider.Title,
                Subtitle = slider.Subtitle,
                Order = slider.Order
            };
        }
        catch (Exception)
        {
            _modelState.AddModelError(string.Empty, "Error retrieving slider.");
            return null;
        }
    }

    public async Task<bool> UpdateAsync(int id, SliderUpdateVM model)
    {
        if (!_modelState.IsValid) return false;

        try
        {
            var slider = await _sliderRepository.GetByIdAsync(id);
            if (slider == null)
            {
                _modelState.AddModelError(string.Empty, "Slider not found.");
                return false;
            }

            var existingSliders = await _sliderRepository.GetAllAsync();
            if (existingSliders.Any(s => s.Order == model.Order && s.Id != id))
            {
                _modelState.AddModelError("Order", "Order number is taken.");
                return false;
            }

            if (model.ImagePath != null)
            {
                var oldFileName = Path.GetFileName(slider.ImagePath);
                _fileService.Delete("assets/images/sliders", oldFileName);

                var newImagePath = _fileService.Upload(model.ImagePath, "assets/images/sliders");
                slider.ImagePath = newImagePath;
            }

            slider.Title = model.Title;
            slider.Subtitle = model.Subtitle;
            slider.Order = model.Order;
            slider.ModifiedAt = DateTime.UtcNow;

            _sliderRepository.Update(slider);
            await _unitOfWork.CommitAsync();
            return true;
        }
        catch (Exception)
        {
            _modelState.AddModelError(string.Empty, "Error updating slider.");
            return false;
        }
    }


    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var slider = await _sliderRepository.GetByIdAsync(id);
            if (slider == null)
            {
                _modelState.AddModelError(string.Empty, "Slider not found.");
                return false;
            }

            var fileName = Path.GetFileName(slider.ImagePath);
            _fileService.Delete("assets/images/sliders", fileName);

            _sliderRepository.Delete(slider);
            await _unitOfWork.CommitAsync();
            return true;
        }
        catch (Exception)
        {
            _modelState.AddModelError(string.Empty, "Error deleting slider.");
            return false;
        }
    }

}