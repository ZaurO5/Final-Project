using Business.Services.Abstract;
using Business.ViewModels.Size;
using Core.Entities;
using Data.Repositories.Abstract;
using Data.UnitOfWork;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Concrete
{
    public class SizeService : ISizeService
    {
        private readonly ISizeRepository _sizeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ModelStateDictionary _modelState;

        public SizeService(ISizeRepository sizeRepository,
                            IUnitOfWork unitOfWork,
                            IActionContextAccessor actionContextAccessor)
        {
            _sizeRepository = sizeRepository;
            _unitOfWork = unitOfWork;
            _modelState = actionContextAccessor.ActionContext.ModelState;
        }

        public async Task<SizeIndexVM> GetAllAsync()
        {
            return new SizeIndexVM
            {
                Sizes = await _sizeRepository.GetAllAsync()
            };
        }

        public async Task<Size> GetByIdAsync(int id)
        {
            return await _sizeRepository.GetByIdAsync(id);
        }

        public async Task<bool> CreateAsync(SizeCreateVM model)
        {
            if (!_modelState.IsValid) return false;

            var size = await _sizeRepository.GetByNameAsync(model.Name);
            if (size is not null)
            {
                _modelState.AddModelError("Name", "Size already exists");
                return false;
            }

            size = new Size
            {
                Name = model.Name,
                CreatedAt = DateTime.Now
            };

            await _sizeRepository.CreateAsync(size);
            await _unitOfWork.CommitAsync();

            return true;
        }

        public async Task<SizeUpdateVM> UpdateAsync(int id)
        {
            var size = await _sizeRepository.GetByIdAsync(id);
            if (size is null) return null;

            return new SizeUpdateVM
            {
                Name = size.Name
            };
        }

        public async Task<bool> UpdateAsync(int id, SizeUpdateVM model)
        {
            if (!_modelState.IsValid) return false;

            var size = await _sizeRepository.GetByIdAsync(id);
            if (size is null)
            {
                _modelState.AddModelError(string.Empty, "Size is unavailable");
                return false;
            }

            var existSize = await _sizeRepository.GetByNameAsync(model.Name);
            if (existSize is not null && existSize.Id != id)
            {
                _modelState.AddModelError("Name", "Size already exists");
                return false;
            }

            size.Name = model.Name;
            size.ModifiedAt = DateTime.Now;

            _sizeRepository.Update(size);
            await _unitOfWork.CommitAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var size = await _sizeRepository.GetByIdAsync(id);
            if (size is null) return false;

            _sizeRepository.Delete(size);
            await _unitOfWork.CommitAsync();

            return true;
        }
    }

}
