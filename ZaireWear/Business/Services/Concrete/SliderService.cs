using Business.ViewModels.Slider;
using Core.Entities;
using Data.Repositories.Abstract;
using Data.UnitOfWork;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;

public class SliderService : ISliderService
{
    private readonly ISliderRepository _sliderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ModelStateDictionary _modelState;

    public SliderService(ISliderRepository sliderRepository,
                         IUnitOfWork unitOfWork,
                         IActionContextAccessor actionContextAccessor)
    {
        _sliderRepository = sliderRepository;
        _unitOfWork = unitOfWork;
        _modelState = actionContextAccessor.ActionContext.ModelState;
    }

    public async Task<SliderIndexVM> GetAllAsync()
    {
        return new SliderIndexVM { Sliders = await _sliderRepository.GetAllAsync() };
    }

    public async Task<SliderCreateVM> CreateAsync() => new();

    public async Task<bool> CreateAsync(SliderCreateVM model)
    {
        if (!_modelState.IsValid) return false;

        var existingSliders = await _sliderRepository.GetAllAsync();
        if (existingSliders.Any(s => s.Order == model.Order))
        {
            _modelState.AddModelError("Order", "This order number is already taken.");
            return false;
        }

        var slider = new Slider
        {
            Title = model.Title,
            Subtitle = model.Subtitle,
            ImagePath = model.ImagePath,
            Order = model.Order,
            IsActive = true,
            CreatedAt = DateTime.Now
        };

        await _sliderRepository.CreateAsync(slider);
        await _unitOfWork.CommitAsync();

        return true;
    }



    public async Task<SliderUpdateVM> UpdateAsync(int id)
    {
        var slider = await _sliderRepository.GetAsync(id);
        if (slider == null) return null;

        return new SliderUpdateVM
        {
            Title = slider.Title,
            Subtitle = slider.Subtitle,
            ImagePath = slider.ImagePath,
            Order = slider.Order
        };
    }

    public async Task<bool> UpdateAsync(int id, SliderUpdateVM model)
    {
        if (!_modelState.IsValid) return false;

        var slider = await _sliderRepository.GetAsync(id);
        if (slider == null) return false;

        var existingSliders = await _sliderRepository.GetAllAsync();
        if (existingSliders.Any(s => s.Order == model.Order && s.Id != id))
        {
            _modelState.AddModelError("Order", "This order number is already taken.");
            return false;
        }

        slider.Title = model.Title;
        slider.Subtitle = model.Subtitle;
        slider.ImagePath = model.ImagePath;
        slider.Order = model.Order;
        slider.ModifiedAt = DateTime.Now;

        _sliderRepository.Update(slider);
        await _unitOfWork.CommitAsync();

        return true;
    }


    public async Task<bool> DeleteAsync(int id)
    {
        var slider = await _sliderRepository.GetAsync(id);
        if (slider == null) return false;

        _sliderRepository.Delete(slider);
        await _unitOfWork.CommitAsync();

        return true;
    }
}
