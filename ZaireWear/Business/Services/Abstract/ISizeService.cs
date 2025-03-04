using Business.ViewModels.Size;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Abstract
{
    public interface ISizeService
    {
        Task<SizeIndexVM> GetAllAsync();
        Task<Size> GetAsync(int id);
        Task<bool> CreateAsync(SizeCreateVM model);
        Task<SizeUpdateVM> UpdateAsync(int id);
        Task<bool> UpdateAsync(int id, SizeUpdateVM model);
        Task<bool> DeleteAsync(int id);
    }
}
