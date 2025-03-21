using Business.ViewModels.Favorite;

namespace Business.Services.Abstract;

public interface IFavoritesService
{
    Task<(int statusCode, string message)> AddToFavoritesAsync(int productId);
    Task<(int statusCode, string message)> RemoveFromFavoritesAsync(int productId);
    Task<FavoritesIndexVM> GetFavoritesAsync();
}
