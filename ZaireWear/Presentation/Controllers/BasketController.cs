using Business.Services.Abstract;
using Business.Utilities.Stripe;
using Business.ViewModels.Basket;
using Business.ViewModels.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe.Checkout;

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

        // Для отладки
        Console.WriteLine($"Found {model.BasketProducts.Count} items in basket");

        return View(model);
    }

    [HttpPost("AddProduct")] // Явное указание маршрута
    public async Task<IActionResult> AddProduct(
         [FromForm] int productId,
         [FromForm] string color,
         [FromForm] string size,
         [FromForm] int quantity)
    {
        var result = await _basketService.AddProductAsync(
            productId,
            color,
            size,
            quantity);

        return Json(result);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateCart([FromBody] List<BasketUpdateVM> updatedProducts)
    {
        var result = await _basketService.UpdateCartAsync(updatedProducts);
        return Json(result);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _basketService.DeleteAsync(id);
        return Json(result);
    }
}