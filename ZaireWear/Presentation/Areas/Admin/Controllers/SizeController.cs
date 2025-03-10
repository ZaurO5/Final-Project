using Business.Services.Abstract;
using Business.ViewModels.Size;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SizeController : Controller
    {
        private readonly ISizeService _sizeService;
        private const int PageSize = 5;

        public SizeController(ISizeService sizeService)
        {
            _sizeService = sizeService;
        }

        #region Read

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            try
            {
                var model = await _sizeService.GetAllAsync();
                var paginatedSizes = model.Sizes
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();

                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = (int)Math.Ceiling(model.Sizes.Count / (double)PageSize);

                return View(new SizeIndexVM { Sizes = paginatedSizes });
            }
            catch
            {
                TempData["Error"] = "Error loading sizes";
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
        public async Task<IActionResult> Create(SizeCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please correct the errors";
                return View(model);
            }

            var result = await _sizeService.CreateAsync(model);
            if (result)
            {
                TempData["Success"] = "Size created successfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Error creating size";
            return View(model);
        }

        #endregion

        #region Update

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var model = await _sizeService.UpdateAsync(id);
            if (model == null)
            {
                TempData["Error"] = "Size not found";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, SizeUpdateVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please correct the errors";
                return View(model);
            }

            var result = await _sizeService.UpdateAsync(id, model);
            if (result)
            {
                TempData["Success"] = "Size updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Error updating size";
            return View(model);
        }

        #endregion

        #region Delete

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _sizeService.DeleteAsync(id);
            if (result)
            {
                TempData["Success"] = "Size deleted successfully!";
            }
            else
            {
                TempData["Error"] = "Error deleting size";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
