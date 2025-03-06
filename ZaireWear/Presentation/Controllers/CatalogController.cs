using Business.Services.Abstract;
using Business.ViewModels.Catalog;
using Core.Constants;
using MailKit.Search;
using Microsoft.AspNetCore.Mvc;

namespace ZaireWear.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public CatalogController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? categoryId, string searchQuery, int? gender)
        {
            var products = await _productService.GetAllAsync();
            var categories = await _categoryService.GetAllAsync();

            // Фильтрация по категории
            if (categoryId.HasValue)
            {
                products.Products = products.Products
                    .Where(p => p.ProductCategories.Any(pc => pc.CategoryId == categoryId))
                    .ToList();
            }

            // Фильтрация по гендеру
            if (gender.HasValue)
            {
                products.Products = products.Products
                    .Where(p => p.Gender == (Gender)gender)
                    .ToList();
            }

            ViewBag.SearchQuery = searchQuery; // Сохраняем поисковый запрос для отображения в форме
            ViewBag.Gender = gender; // Сохраняем выбранный гендер для отображения в навбаре

            var model = new CatalogIndexVM
            {
                Products = products.Products,
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
}
