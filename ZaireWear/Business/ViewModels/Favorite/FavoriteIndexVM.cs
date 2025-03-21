using Core.Entities;

namespace Business.ViewModels.Favorite;

public class FavoritesIndexVM
{
    public List<FavoriteProduct> FavoriteProducts { get; set; } = new List<FavoriteProduct>();
}
