using Business.Utilities.File;
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
    private readonly IFileService _fileService;
    private readonly ModelStateDictionary _modelState;

    public SliderService(ISliderRepository sliderRepository,
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
        return new SliderIndexVM { Sliders = await _sliderRepository.GetAllAsync() };
    }

    public Task<SliderCreateVM> CreateAsync() => Task.FromResult(new SliderCreateVM());

    public async Task<bool> CreateAsync(SliderCreateVM model)
    {
        if (!_modelState.IsValid) return false;

        var existingSliders = await _sliderRepository.GetAllAsync();
        if (existingSliders.Any(s => s.Order == model.Order))
        {
            _modelState.AddModelError("Order", "This order number is already taken.");
            return false;
        }

        var imagePath = _fileService.Upload(model.ImagePath, "assets/images/sliders");

        var slider = new Slider
        {
            Title = model.Title,
            Subtitle = model.Subtitle,
            ImagePath = "/assets/images/sliders/" + imagePath,
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

        if (model.ImagePath != null)
        {
            var oldFileName = Path.GetFileName(slider.ImagePath);
            _fileService.Delete("assets/images/sliders", oldFileName);

            var newFileName = _fileService.Upload(model.ImagePath, "assets/images/sliders");
            slider.ImagePath = "/assets/images/sliders/" + newFileName;
        }

        slider.Title = model.Title;
        slider.Subtitle = model.Subtitle;
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

        var fileName = Path.GetFileName(slider.ImagePath);
        _fileService.Delete("assets/images/sliders", fileName);

        _sliderRepository.Delete(slider);
        await _unitOfWork.CommitAsync();

        return true;
    }

}