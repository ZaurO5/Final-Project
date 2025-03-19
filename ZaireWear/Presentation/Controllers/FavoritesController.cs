using Business.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Authorize]
    [Route("Favorites")]
    public class FavoritesController : Controller
    {
        private readonly IFavoritesService _favoritesService;

        public FavoritesController(IFavoritesService favoritesService)
        {
            _favoritesService = favoritesService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await _favoritesService.GetFavoritesAsync();
            return View(model);
        }

        [HttpPost("AddToFavorites")]
        public async Task<IActionResult> AddToFavorites([FromForm] int productId)
        {
            var result = await _favoritesService.AddToFavoritesAsync(productId);
            return Json(new { statusCode = result.statusCode, message = result.message });
        }

        [HttpPost("RemoveFromFavorites")]
        public async Task<IActionResult> RemoveFromFavorites([FromForm] int productId)
        {
            var result = await _favoritesService.RemoveFromFavoritesAsync(productId);
            return Json(new { statusCode = result.statusCode, message = result.message });
        }
    }
}
