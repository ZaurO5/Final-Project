﻿using Core.Entities;
using Data.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Abstract
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<Category> GetByNameAsync(string name);
        Task<Category> GetByIdAsync(int id);
    }
}
