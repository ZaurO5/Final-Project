using Core.Entities;
using Data.Repositories.Base;

namespace Data.Repositories.Abstract;

public interface IOrderRepository : IBaseRepository<Order>
{
	Task<Order> GetOrderWithOrderProductsAsync(Guid token, string userId);
}
