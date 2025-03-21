using Business.Services.Abstract;
using Business.ViewModels.Catalog;
using Core.Constants.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ZaireWear.Controllers;

public class CatalogController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private const int PageSize = 9;

    public CatalogController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? categoryId, string searchQuery, int? gender, int page = 1)
    {
        var products = await _productService.GetAllAsync();
        var categories = await _categoryService.GetAllAsync();

        if (categoryId.HasValue)
        {
            products.Products = products.Products
                .Where(p => p.ProductCategories.Any(pc => pc.CategoryId == categoryId))
                .ToList();
        }

        if (gender.HasValue)
        {
            products.Products = products.Products
                .Where(p => p.Gender == (Gender)gender)
                .ToList();
        }

        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            var query = searchQuery.Trim().ToLower();
            products.Products = products.Products
                .Where(p => p.Title.ToLower().Contains(query))
                .ToList();
        }

        var totalItems = products.Products.Count;
        var pagedProducts = products.Products
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .ToList();

        ViewBag.SearchQuery = searchQuery;
        ViewBag.Gender = gender;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
        ViewBag.CategoryId = categoryId;

        var model = new CatalogIndexVM
        {
            Products = pagedProducts,
            Categories = categories.Categories
        };

        return View(model);
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _productService.GetByIdWithDetailsAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }
}
