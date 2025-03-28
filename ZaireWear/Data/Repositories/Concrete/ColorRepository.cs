﻿using Core.Entities;
using Data.Contexts;
using Data.Repositories.Abstract;
using Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Concrete;

public class ColorRepository : BaseRepository<Color>, IColorRepository
{
    private readonly AppDbContext _context;

    public ColorRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public new async Task<Color> GetByIdAsync(int id)
    {
        return await _context.Colors.FindAsync(id);
    }

    public async Task<Color> GetByNameAsync(string name)
    {
        return await _context.Colors.FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
    }
}
