using Business.Services.Abstract;
using Business.ViewModels.Color;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ColorController : Controller
    {
        private readonly IColorService _colorService;

        public ColorController(IColorService colorService)
        {
            _colorService = colorService;
        }

        #region Read

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await _colorService.GetAllAsync();
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
