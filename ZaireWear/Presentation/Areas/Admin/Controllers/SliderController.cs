using Business.ViewModels.Slider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SliderController : Controller
    {
        private readonly ISliderService _sliderService;

        public SliderController(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }

        #region Read
        public async Task<IActionResult> Index()
        {
            var model = await _sliderService.GetAllAsync();
            return View(model);
        }

        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SliderCreateVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _sliderService.CreateAsync(model);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Failed to create slider.");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var model = await _sliderService.UpdateAsync(id);
            if (model == null) return NotFound();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, SliderUpdateVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _sliderService.UpdateAsync(id, model);
            if (!result)
            {
                ModelState.AddModelError(string.Empty, "Failed to update slider.");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _sliderService.DeleteAsync(id);
            if (!result) return NotFound();

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}