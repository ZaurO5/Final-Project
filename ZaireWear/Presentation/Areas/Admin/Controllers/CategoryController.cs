using Business.Services.Abstract;
using Business.ViewModels.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;
    private const int PageSize = 5;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    #region Read

    [HttpGet]
    public async Task<IActionResult> Index(int page = 1)
    {
        try
        {
            var model = await _categoryService.GetAllAsync();
            var paginatedCategories = model.Categories
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(model.Categories.Count / (double)PageSize);

            return View(new CategoryIndexVM { Categories = paginatedCategories });
        }
        catch
        {
            TempData["Error"] = "Error loading categories";
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
    public async Task<IActionResult> Create(CategoryCreateVM model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Please correct the errors";
            return View(model);
        }

        var result = await _categoryService.CreateAsync(model);
        if (result)
        {
            TempData["Success"] = "Category created successfully!";
            return RedirectToAction(nameof(Index));
        }

        var errors = ModelState.Values.SelectMany(v => v.Errors)
                                     .Select(e => e.ErrorMessage);
        TempData["Error"] = string.Join("; ", errors);

        return View(model);
    }

    #endregion

    #region Update

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var model = await _categoryService.UpdateAsync(id);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Update(int id, CategoryUpdateVM model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Please correct the errors";
            return View(model);
        }

        var result = await _categoryService.UpdateAsync(id, model);
        if (result)
        {
            TempData["Success"] = "Category updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        TempData["Error"] = "Error updating category";
        return View(model);
    }

    #endregion

    #region Delete

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _categoryService.DeleteAsync(id);
        if (result)
        {
            TempData["Success"] = "Category deleted successfully!";
        }
        else
        {
            TempData["Error"] = "Error deleting category";
        }
        return RedirectToAction(nameof(Index));
    }

    #endregion
}
