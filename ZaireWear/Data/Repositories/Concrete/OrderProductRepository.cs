using Core.Entities;
using Data.Contexts;
using Data.Repositories.Abstract;
using Data.Repositories.Base;

namespace Data.Repositories.Concrete;

public class OrderProductRepository : BaseRepository<OrderProduct>, IOrderProductRepository
{
    public OrderProductRepository(AppDbContext context) : base(context)
    {
        
    }
}
