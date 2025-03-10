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

        public SizeService(
            ISizeRepository sizeRepository,
            IUnitOfWork unitOfWork,
            IActionContextAccessor actionContextAccessor)
        {
            _sizeRepository = sizeRepository;
            _unitOfWork = unitOfWork;
            _modelState = actionContextAccessor.ActionContext.ModelState;
        }

        public async Task<SizeIndexVM> GetAllAsync()
        {
            try
            {
                var sizes = await _sizeRepository.GetAllAsync();
                return new SizeIndexVM { Sizes = sizes };
            }
            catch (Exception ex)
            {
                _modelState.AddModelError(string.Empty, "Error retrieving sizes");
                return new SizeIndexVM { Sizes = new List<Size>() };
            }
        }

        public async Task<Size> GetByIdAsync(int id)
        {
            try
            {
                return await _sizeRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _modelState.AddModelError(string.Empty, "Error retrieving size");
                return null;
            }
        }

        public async Task<bool> CreateAsync(SizeCreateVM model)
        {
            if (!_modelState.IsValid) return false;

            try
            {
                var existingSize = await _sizeRepository.GetByNameAsync(model.Name);
                if (existingSize != null)
                {
                    _modelState.AddModelError("Name", "Size already exists");
                    return false;
                }

                var newSize = new Size
                {
                    Name = model.Name,
                    CreatedAt = DateTime.Now
                };

                await _sizeRepository.CreateAsync(newSize);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _modelState.AddModelError(string.Empty, "Error creating size");
                return false;
            }
        }

        public async Task<SizeUpdateVM> UpdateAsync(int id)
        {
            try
            {
                var size = await _sizeRepository.GetByIdAsync(id);
                if (size == null)
                {
                    _modelState.AddModelError(string.Empty, "Size not found");
                    return null;
                }

                return new SizeUpdateVM { Name = size.Name };
            }
            catch (Exception ex)
            {
                _modelState.AddModelError(string.Empty, "Error retrieving size");
                return null;
            }
        }

        public async Task<bool> UpdateAsync(int id, SizeUpdateVM model)
        {
            if (!_modelState.IsValid) return false;

            try
            {
                var size = await _sizeRepository.GetByIdAsync(id);
                if (size == null)
                {
                    _modelState.AddModelError(string.Empty, "Size not found");
                    return false;
                }

                var existingSize = await _sizeRepository.GetByNameAsync(model.Name);
                if (existingSize != null && existingSize.Id != id)
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
            catch (Exception ex)
            {
                _modelState.AddModelError(string.Empty, "Error updating size");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var size = await _sizeRepository.GetByIdAsync(id);
                if (size == null)
                {
                    _modelState.AddModelError(string.Empty, "Size not found");
                    return false;
                }

                _sizeRepository.Delete(size);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _modelState.AddModelError(string.Empty, "Error deleting size");
                return false;
            }
        }
    }
}
