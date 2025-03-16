using Business.Services.Abstract;
using Business.ViewModels.Basket;
using Core.Entities;
using Data.Repositories.Abstract;
using Data.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Business.Services.Concrete;

public class BasketService : IBasketService
{
    private readonly IBasketRepository _basketRepository;
    private readonly IProductRepository _productRepository;
    private readonly IBasketProductRepository _basketProductRepository;
    private readonly UserManager<User> _userManager;
    private readonly IActionContextAccessor _actionContextAccessor;
    private readonly IUnitOfWork _unitOfWork;

    public BasketService(
        IBasketRepository basketRepository,
        IProductRepository productRepository,
        IBasketProductRepository basketProductRepository,
        UserManager<User> userManager,
        IActionContextAccessor actionContextAccessor,
        IUnitOfWork unitOfWork)
    {
        _basketRepository = basketRepository;
        _productRepository = productRepository;
        _basketProductRepository = basketProductRepository;
        _userManager = userManager;
        _actionContextAccessor = actionContextAccessor;
        _unitOfWork = unitOfWork;
    }

    public async Task<BasketIndexVM> GetAllAsync()
    {
        var user = await GetCurrentUserAsync();
        if (user == null) return new BasketIndexVM();

        var basket = await _basketRepository.GetBasketByUserId(user.Id);

        return new BasketIndexVM
        {
            BasketProducts = basket?.BasketProducts?.ToList() ?? new List<BasketProduct>()
        };
    }

    public async Task<(int statusCode, string description)> AddProductAsync(
     int productId,
     int ColorId,
     int SizeId,
     int quantity)
    {
        var user = await GetCurrentUserAsync();
        if (user == null) return (401, "Требуется авторизация");

        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null) return (404, "Товар не найден");

        product.ProductColors ??= new List<ProductColors>();
        product.ProductSizes ??= new List<ProductSizes>();

        if (quantity > product.StockCount)
            return (400, "Недостаточно товара на складе");

        var basket = await _basketRepository.GetBasketByUserId(user.Id)
            ?? await CreateNewBasketAsync(user.Id);

        var existingItem = await _basketProductRepository.GetByVariantAsync(
            basket.Id,
            productId,
            ColorId,
            SizeId);

        if (existingItem != null)
        {
            if (existingItem.Count + quantity > product.StockCount)
                return (400, "Достигнуто максимальное количество");

            existingItem.Count += quantity;
            _basketProductRepository.Update(existingItem);
        }
        else
        {
            var newItem = new BasketProduct
            {
                BasketId = basket.Id,
                ProductId = productId,
                ColorId = ColorId,
                SizeId = SizeId,
                Count = quantity
            };
            await _basketProductRepository.CreateAsync(newItem);
        }

        await _unitOfWork.CommitAsync();
        return (200, "Товар добавлен в корзину");
    }

    public async Task<(int statusCode, string description)> UpdateCartAsync(List<BasketUpdateVM> updatedProducts)
    {
        var user = await GetCurrentUserAsync();
        if (user == null) return (401, "Требуется авторизация");

        foreach (var item in updatedProducts)
        {
            var basketProduct = await _basketProductRepository.GetByIdAsync(item.BasketProductId);
            if (basketProduct == null) continue;

            var product = await _productRepository.GetByIdAsync(basketProduct.ProductId);
            if (product == null) return (404, "Товар не найден");

            if (item.Count < 1 || item.Count > product.StockCount)
                return (400, $"Некорректное количество для {product.Title}");

            basketProduct.Count = item.Count;
            _basketProductRepository.Update(basketProduct);
        }

        await _unitOfWork.CommitAsync();
        return (200, "Корзина обновлена");
    }

    public async Task<(int statusCode, string description)> DeleteAsync(int id)
    {
        var basketProduct = await _basketProductRepository.GetByIdAsync(id);
        if (basketProduct == null) return (404, "Позиция не найдена");

        _basketProductRepository.Delete(basketProduct);
        await _unitOfWork.CommitAsync();
        return (200, "Позиция удалена из корзины");
    }

    private async Task<User> GetCurrentUserAsync()
    {
        return await _userManager.GetUserAsync(_actionContextAccessor.ActionContext.HttpContext.User);
    }

    private async Task<Basket> CreateNewBasketAsync(string userId)
    {
        var basket = new Basket { UserId = userId };
        await _basketRepository.CreateAsync(basket);
        await _unitOfWork.CommitAsync();
        return basket;
    }
}