using Business.Services.Abstract;
using Business.ViewModels.Favorite;
using Core.Entities;
using Data.Repositories.Abstract;
using Data.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Business.Services.Concrete;

public class FavoritesService : IFavoritesService
{
    private readonly IFavoriteProductRepository _favoriteProductRepository;
    private readonly IProductRepository _productRepository;
    private readonly UserManager<User> _userManager;
    private readonly IActionContextAccessor _actionContextAccessor;
    private readonly IUnitOfWork _unitOfWork;

    public FavoritesService(
        IFavoriteProductRepository favoriteProductRepository,
        IProductRepository productRepository,
        UserManager<User> userManager,
        IActionContextAccessor actionContextAccessor,
        IUnitOfWork unitOfWork)
    {
        _favoriteProductRepository = favoriteProductRepository;
        _productRepository = productRepository;
        _userManager = userManager;
        _actionContextAccessor = actionContextAccessor;
        _unitOfWork = unitOfWork;
    }

    public async Task<(int statusCode, string message)> AddToFavoritesAsync(int productId)
    {
        var user = await GetCurrentUserAsync();
        if (user == null) return (401, "Authorize first");

        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null) return (404, "product not found");

        var existingFavorite = await _favoriteProductRepository.GetByUserAndProductAsync(user.Id, productId);
        if (existingFavorite != null) return (400, "product is already in favorites");

        var favoriteProduct = new FavoriteProduct
        {
            UserId = user.Id,
            ProductId = productId
        };

        await _favoriteProductRepository.AddAsync(favoriteProduct);
        await _unitOfWork.CommitAsync();

        return (200, "added to favorites successfully");
    }

    public async Task<(int statusCode, string message)> RemoveFromFavoritesAsync(int productId)
    {
        var user = await GetCurrentUserAsync();
        if (user == null) return (401, "Authorize first");

        var favoriteProduct = await _favoriteProductRepository.GetByUserAndProductAsync(user.Id, productId);
        if (favoriteProduct == null) return (404, "product is not found in favorites");

        _favoriteProductRepository.Remove(favoriteProduct);
        await _unitOfWork.CommitAsync();

        return (200, "product deleted from favorites");
    }

    public async Task<FavoritesIndexVM> GetFavoritesAsync()
    {
        var user = await GetCurrentUserAsync();
        if (user == null) return new FavoritesIndexVM();

        var favorites = await _favoriteProductRepository.GetByUserIdAsync(user.Id);
        return new FavoritesIndexVM { FavoriteProducts = favorites };
    }

    private async Task<User> GetCurrentUserAsync()
    {
        return await _userManager.GetUserAsync(_actionContextAccessor.ActionContext.HttpContext.User);
    }
}
