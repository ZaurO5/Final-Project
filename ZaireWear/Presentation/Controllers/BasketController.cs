using Business.Services.Abstract;
using Business.ViewModels.Basket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ZaireWear.Controllers;

[Route("Basket")]
[ApiController]
[Authorize]
public class BasketController : Controller
{
    private readonly IBasketService _basketService;

    public BasketController(IBasketService basketService)
    {
        _basketService = basketService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var model = await _basketService.GetAllAsync();
        Console.WriteLine($"Found {model.BasketProducts.Count} items in basket");
        return View(model);
    }

    [HttpPost("AddProduct")]
    public async Task<IActionResult> AddProduct(
        [FromForm] int productId,
        [FromForm] int colorId,
        [FromForm] int sizeId,
        [FromForm] int quantity)
    {
        try
        {
            var result = await _basketService.AddProductAsync(productId, colorId, sizeId, quantity);
            return Json(new { statusCode = result.statusCode, description = result.description });
        }
        catch (Exception ex)
        {
            return BadRequest(new { statusCode = 500, description = "error on server: " + ex.Message });
        }
    }

    [HttpPost("UpdateCart")]
    public async Task<IActionResult> UpdateCart([FromBody] List<BasketUpdateVM> updatedProducts)
    {
        try
        {
            var result = await _basketService.UpdateCartAsync(updatedProducts);
            return Json(new { statusCode = result.statusCode, description = result.description });
        }
        catch (Exception ex)
        {
            return BadRequest(new { statusCode = 500, description = "error while updating: " + ex.Message });
        }
    }

    [HttpPost("Delete")]
    public async Task<IActionResult> Delete([FromBody] BasketDeleteVM deleteModel)
    {
        try
        {
            var result = await _basketService.DeleteAsync(deleteModel.BasketId, deleteModel.ProductId, deleteModel.ColorId, deleteModel.SizeId);
            return Json(new { statusCode = result.statusCode, description = result.description });
        }
        catch (Exception ex)
        {
            return BadRequest(new { statusCode = 500, description = "error while deleting: " + ex.Message });
        }
    }

}