using Core.Entities;
using Data.Repositories.Base;

namespace Data.Repositories.Abstract;

public interface IBasketRepository : IBaseRepository<Basket>
{
    Task<Basket> GetBasketByUserId(string userId);
    Task CreateBasketForUserAsync(string userId);
    Task<Basket> GetBasketByUserIdWithDetails(string userId);
}