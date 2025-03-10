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
            try
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
            catch
            {
                TempData["Error"] = "Error loading colors";
                return RedirectToAction("Index", "Home");
            }
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
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please correct the errors";
                return View(model);
            }

            var result = await _colorService.CreateAsync(model);
            if (result)
            {
                TempData["Success"] = "Color created successfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Error creating color";
            return View(model);
        }

        #endregion

        #region Update

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var model = await _colorService.UpdateAsync(id);
            if (model == null)
            {
                TempData["Error"] = "Color not found";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, ColorUpdateVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please correct the errors";
                return View(model);
            }

            var result = await _colorService.UpdateAsync(id, model);
            if (result)
            {
                TempData["Success"] = "Color updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Error updating color";
            return View(model);
        }

        #endregion

        #region Delete

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _colorService.DeleteAsync(id);
            if (result)
            {
                TempData["Success"] = "Color deleted successfully!";
            }
            else
            {
                TempData["Error"] = "Error deleting color";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
