using Business.Services.Abstract;
using Business.ViewModels.Color;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ColorController : Controller
    {
        private readonly IColorService _colorService;
        private const int PageSize = 10;

        public ColorController(IColorService colorService)
        {
            _colorService = colorService;
        }

        #region Read

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            var model = await _colorService.GetAllAsync();
            var paginatedColors = model.Colors
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(model.Colors.Count / (double)PageSize);

            return View(new ColorIndexVM { Colors = paginatedColors });
        }

        #endregion

        #region Create

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ColorCreateVM model)
        {
            var isSucceeded = await _colorService.CreateAsync(model);
            if (isSucceeded) return RedirectToAction(nameof(Index));

            return View(model);
        }

        #endregion

        #region Update

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var model = await _colorService.UpdateAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, ColorUpdateVM model)
        {
            var isSucceeded = await _colorService.UpdateAsync(id, model);
            if (isSucceeded) return RedirectToAction(nameof(Index));

            return View(model);
        }

        #endregion

        #region Delete

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var isSucceeded = await _colorService.DeleteAsync(id);
            if (isSucceeded) return RedirectToAction(nameof(Index));

            return NotFound();
        }

        #endregion
    }
}
