using Business.Services.Abstract;
using Business.ViewModels.Favorite;
using Core.Entities;
using Data.Repositories.Abstract;
using Data.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Concrete
{
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
            if (user == null) return (401, "Требуется авторизация");

            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null) return (404, "Товар не найден");

            var existingFavorite = await _favoriteProductRepository.GetByUserAndProductAsync(user.Id, productId);
            if (existingFavorite != null) return (400, "Товар уже в избранном");

            var favoriteProduct = new FavoriteProduct
            {
                UserId = user.Id,
                ProductId = productId
            };

            await _favoriteProductRepository.AddAsync(favoriteProduct);
            await _unitOfWork.CommitAsync();

            return (200, "Товар добавлен в избранное");
        }

        public async Task<(int statusCode, string message)> RemoveFromFavoritesAsync(int productId)
        {
            var user = await GetCurrentUserAsync();
            if (user == null) return (401, "Требуется авторизация");

            var favoriteProduct = await _favoriteProductRepository.GetByUserAndProductAsync(user.Id, productId);
            if (favoriteProduct == null) return (404, "Товар не найден в избранном");

            _favoriteProductRepository.Remove(favoriteProduct);
            await _unitOfWork.CommitAsync();

            return (200, "Товар удален из избранного");
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
}
