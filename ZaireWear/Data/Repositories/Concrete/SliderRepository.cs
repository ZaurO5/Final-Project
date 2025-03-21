using Core.Entities;
using Data.Contexts;
using Data.Repositories.Abstract;
using Data.Repositories.Base;

namespace Data.Repositories.Concrete;

public class SliderRepository : BaseRepository<Slider>, ISliderRepository
{
    private readonly AppDbContext _context;

    public SliderRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}