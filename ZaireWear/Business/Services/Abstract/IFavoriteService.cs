using Business.ViewModels.Favorite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Abstract
{
    public interface IFavoritesService
    {
        Task<(int statusCode, string message)> AddToFavoritesAsync(int productId);
        Task<(int statusCode, string message)> RemoveFromFavoritesAsync(int productId);
        Task<FavoritesIndexVM> GetFavoritesAsync();
    }
}
