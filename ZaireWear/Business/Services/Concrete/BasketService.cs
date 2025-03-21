﻿using Business.Services.Abstract;
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
        if (user == null) return (401, "Authorize first");

        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null) return (404, "product not found");

        product.ProductColors ??= new List<ProductColors>();
        product.ProductSizes ??= new List<ProductSizes>();

        if (quantity > product.StockCount)
            return (400, "not enough product");

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
                return (400, "max quantity");

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
        return (200, "added to basket successfully");
    }

    public async Task<(int statusCode, string description)> UpdateCartAsync(List<BasketUpdateVM> updatedProducts)
    {
        var user = await GetCurrentUserAsync();
        if (user == null) return (401, "Authorize first");

        if (updatedProducts == null || !updatedProducts.Any())
            return (400, "list is empty");

        foreach (var item in updatedProducts)
        {
            var basketProduct = await _basketProductRepository.GetByIdAsync(
                item.BasketId,
                item.ProductId,
                item.ColorId,
                item.SizeId);

            if (basketProduct == null) continue;

            var product = await _productRepository.GetByIdAsync(basketProduct.ProductId);
            if (product == null) return (404, $"prodyct with ID {basketProduct.ProductId} not found");

            if (item.Count < 1)
                return (400, $"quantity for {product.Title} cannot be less then 1");
            if (item.Count > product.StockCount)
                return (400, $"quantity for {product.Title} exceeds available stock ({product.StockCount})");

            basketProduct.Count = item.Count;
            _basketProductRepository.Update(basketProduct);
        }

        await _unitOfWork.CommitAsync();
        return (200, "basket updated");
    }

    public async Task<(int statusCode, string description)> DeleteAsync(int basketId, int productId, int colorId, int sizeId)
    {
        var basketProduct = await _basketProductRepository.GetByIdAsync(basketId, productId, colorId, sizeId);
        if (basketProduct == null) return (404, "product is not found");

        _basketProductRepository.Delete(basketProduct);
        await _unitOfWork.CommitAsync();
        return (200, "product deleted from basket");
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