using Business.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace ZaireWear.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CatalogController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await _categoryService.GetAllAsync();
            return View(model);
        }
        public async Task<IActionResult> Details()
        {
            return View();
        }
    }
}
