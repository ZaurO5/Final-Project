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
        private const int PageSize = 3;

        public SliderController(ISliderService sliderService)
        {
            _sliderService = sliderService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var model = await _sliderService.GetAllAsync();
            var paginatedSliders = model.Sliders
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(model.Sliders.Count / (double)PageSize);

            return View(new SliderIndexVM { Sliders = paginatedSliders });
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(SliderCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Fix validation errors.";
                return View(model);
            }

            var result = await _sliderService.CreateAsync(model);
            if (!result)
            {
                TempData["Error"] = ModelState.IsValid ? "Unknown error." : "Check fields.";
                return View(model);
            }

            TempData["Success"] = "Slider created!";
            return RedirectToAction(nameof(Index));
        }

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
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Fix validation errors.";
                return View(model);
            }

            var result = await _sliderService.UpdateAsync(id, model);
            if (!result)
            {
                TempData["Error"] = ModelState.IsValid ? "Slider not found." : "Check fields.";
                return View(model);
            }

            TempData["Success"] = "Slider updated!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _sliderService.DeleteAsync(id);
            if (!result) TempData["Error"] = "Slider not found or error.";
            else TempData["Success"] = "Slider deleted!";
            return RedirectToAction(nameof(Index));
        }
    }
}