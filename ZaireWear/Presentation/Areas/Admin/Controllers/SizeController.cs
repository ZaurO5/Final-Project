﻿using Business.Services.Abstract;
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
            var model = await _sizeService.GetAllAsync();
            var paginatedSizes = model.Sizes
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(model.Sizes.Count / (double)PageSize);

            return View(new SizeIndexVM { Sizes = paginatedSizes });
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
            var isSucceeded = await _sizeService.CreateAsync(model);
            if (isSucceeded) return RedirectToAction(nameof(Index));

            return View(model);
        }

        #endregion

        #region Update

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var model = await _sizeService.UpdateAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, SizeUpdateVM model)
        {
            var isSucceeded = await _sizeService.UpdateAsync(id, model);
            if (isSucceeded) return RedirectToAction(nameof(Index));

            return View(model);
        }

        #endregion

        #region Delete

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var isSucceeded = await _sizeService.DeleteAsync(id);
            if (isSucceeded) return RedirectToAction(nameof(Index));

            return NotFound();
        }

        #endregion
    }
}
