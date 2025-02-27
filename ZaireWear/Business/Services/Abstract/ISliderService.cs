using Business.ViewModels.Slider;

public interface ISliderService
{
    Task<SliderIndexVM> GetAllAsync();
    Task<SliderCreateVM> CreateAsync();
    Task<bool> CreateAsync(SliderCreateVM model);
    Task<SliderUpdateVM> UpdateAsync(int id);
    Task<bool> UpdateAsync(int id, SliderUpdateVM model);
    Task<bool> DeleteAsync(int id);
}