using Business.Services.Abstract;
using Business.ViewModels.Product;
using Data.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IColorRepository _colorRepository;
        private readonly ISizeRepository _sizeRepository;
        private const int PageSize = 9;

        public ProductController(IProductService productService,
                                 ICategoryRepository categoryRepository,
                                 IColorRepository colorRepository,
                                 ISizeRepository sizeRepository)
        {
            _productService = productService;
            _categoryRepository = categoryRepository;
            _colorRepository = colorRepository;
            _sizeRepository = sizeRepository;
        }

        #region Read
        public async Task<IActionResult> Index(int page = 1)
        {
            var model = await _productService.GetAllAsync();
            var paginatedProducts = model.Products
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(model.Products.Count / (double)PageSize);

            return View(new ProductIndexVM { Products = paginatedProducts });
        }

        #endregion

        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = await _productService.CreateAsync();
            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            ViewBag.Colors = await _colorRepository.GetAllAsync();
            ViewBag.Sizes = await _sizeRepository.GetAllAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryRepository.GetAllAsync();
                ViewBag.Colors = await _colorRepository.GetAllAsync();
                ViewBag.Sizes = await _sizeRepository.GetAllAsync();
                TempData["Error"] = "Please correct the form errors.";
                return View(model);
            }

            var result = await _productService.CreateAsync(model);
            if (!result)
            {
                ViewBag.Categories = await _categoryRepository.GetAllAsync();
                ViewBag.Colors = await _colorRepository.GetAllAsync();
                ViewBag.Sizes = await _sizeRepository.GetAllAsync();
                TempData["Error"] = "Error occurred while creating product.";
                return View(model);
            }

            TempData["Success"] = "Product created successfully!";
            return RedirectToAction(nameof(Index));
        }


        #endregion

        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var model = await _productService.UpdateAsync(id);
            if (model == null) return NotFound();

            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            ViewBag.Colors = await _colorRepository.GetAllAsync();
            ViewBag.Sizes = await _sizeRepository.GetAllAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, ProductUpdateVM model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryRepository.GetAllAsync();
                ViewBag.Colors = await _colorRepository.GetAllAsync();
                ViewBag.Sizes = await _sizeRepository.GetAllAsync();
                TempData["Error"] = "Please correct the form errors.";
                return View(model);
            }

            var result = await _productService.UpdateAsync(id, model);
            if (!result)
            {
                ViewBag.Categories = await _categoryRepository.GetAllAsync();
                ViewBag.Colors = await _colorRepository.GetAllAsync();
                ViewBag.Sizes = await _sizeRepository.GetAllAsync();
                TempData["Error"] = "Error occurred while updating product.";
                return View(model);
            }

            TempData["Success"] = "Product updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.DeleteAsync(id);
            if (!result)
            {
                TempData["Error"] = "Error occurred while deleting product.";
            }
            else
            {
                TempData["Success"] = "Product deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
