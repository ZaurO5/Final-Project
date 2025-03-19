using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Abstract
{
    public interface IFavoriteProductRepository
    {
        Task<FavoriteProduct> GetByUserAndProductAsync(string userId, int productId);
        Task AddAsync(FavoriteProduct favoriteProduct);
        void Remove(FavoriteProduct favoriteProduct);
        Task<List<FavoriteProduct>> GetByUserIdAsync(string userId);
    }
}
