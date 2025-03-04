//using Business.Services.Abstract;
//using Data.UnitOfWork;
//using Microsoft.AspNetCore.Mvc.Infrastructure;
//using Microsoft.AspNetCore.Mvc.ModelBinding;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Business.Services.Concrete
//{
//    public class ColorService : IColorService
//    {
//        private readonly IColorRepository _colorRepository;
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly ModelStateDictionary _modelState;

//        public ColorService(IColorRepository colorRepository,
//                            IUnitOfWork unitOfWork,
//                            IActionContextAccessor actionContextAccessor)
//        {
//            _colorRepository = colorRepository;
//            _unitOfWork = unitOfWork;
//            _modelState = actionContextAccessor.ActionContext.ModelState;
//        }

//        public async Task<ColorIndexVM> GetAllAsync()
//        {
//            return new ColorIndexVM
//            {
//                Colors = await _colorRepository.GetAllAsync()
//            };
//        }

//        public async Task<Color> GetAsync(int id)
//        {
//            return await _colorRepository.GetAsync(id);
//        }

//        public async Task<bool> CreateAsync(ColorCreateVM model)
//        {
//            if (!_modelState.IsValid) return false;

//            var color = await _colorRepository.GetByNameAsync(model.Name);
//            if (color is not null)
//            {
//                _modelState.AddModelError("Name", "Color already exists");
//                return false;
//            }

//            color = new Color
//            {
//                Name = model.Name,
//                HexCode = model.HexCode,
//                CreatedAt = DateTime.Now
//            };

//            await _colorRepository.CreateAsync(color);
//            await _unitOfWork.CommitAsync();

//            return true;
//        }

//        public async Task<ColorUpdateVM> UpdateAsync(int id)
//        {
//            var color = await _colorRepository.GetAsync(id);
//            if (color is null) return null;

//            return new ColorUpdateVM
//            {
//                Name = color.Name,
//                HexCode = color.HexCode
//            };
//        }

//        public async Task<bool> UpdateAsync(int id, ColorUpdateVM model)
//        {
//            if (!_modelState.IsValid) return false;

//            var color = await _colorRepository.GetAsync(id);
//            if (color is null)
//            {
//                _modelState.AddModelError(string.Empty, "Color is unavailable");
//                return false;
//            }

//            var existColor = await _colorRepository.GetByNameAsync(model.Name);
//            if (existColor is not null && existColor.Id != id)
//            {
//                _modelState.AddModelError("Name", "Color already exists");
//                return false;
//            }

//            color.Name = model.Name;
//            color.HexCode = model.HexCode;
//            color.ModifiedAt = DateTime.Now;

//            _colorRepository.Update(color);
//            await _unitOfWork.CommitAsync();

//            return true;
//        }

//        public async Task<bool> DeleteAsync(int id)
//        {
//            var color = await _colorRepository.GetAsync(id);
//            if (color is null) return false;

//            _colorRepository.Delete(color);
//            await _unitOfWork.CommitAsync();

//            return true;
//        }
//    }
//}
