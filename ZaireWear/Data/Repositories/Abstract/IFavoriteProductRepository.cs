using Core.Entities;

namespace Data.Repositories.Abstract;

public interface IFavoriteProductRepository
{
    Task<FavoriteProduct> GetByUserAndProductAsync(string userId, int productId);
    Task AddAsync(FavoriteProduct favoriteProduct);
    void Remove(FavoriteProduct favoriteProduct);
    Task<List<FavoriteProduct>> GetByUserIdAsync(string userId);
}
