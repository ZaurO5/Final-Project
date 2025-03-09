using Business.Services.Abstract;
using Business.Utilities.File;
using Business.ViewModels.Product;
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
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IColorRepository _colorRepository;
        private readonly ISizeRepository _sizeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        private readonly ModelStateDictionary _modelState;

        public ProductService(IProductRepository productRepository,
                              ICategoryRepository categoryRepository,
                              IColorRepository colorRepository,
                              ISizeRepository sizeRepository,
                              IUnitOfWork unitOfWork,
                              IFileService fileService,
                              IActionContextAccessor actionContextAccessor)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _colorRepository = colorRepository;
            _sizeRepository = sizeRepository;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _modelState = actionContextAccessor.ActionContext.ModelState;
        }

        public async Task<ProductIndexVM> GetAllAsync()
        {
            try
            {
                var products = await _productRepository.GetAllWithDetailsAsync();
                return new ProductIndexVM { Products = products };
            }
            catch (Exception)
            {
                _modelState.AddModelError(string.Empty, "Error occurred while retrieving products.");
                return new ProductIndexVM { Products = new List<Product>() };
            }
        }

        public async Task<Product> GetByIdWithDetailsAsync(int id)
        {
            try
            {
                return await _productRepository.GetByIdWithDetailsAsync(id);
            }
            catch (Exception)
            {
                _modelState.AddModelError(string.Empty, "Error occurred while retrieving the product.");
                return null;
            }
        }

        public async Task<ProductCreateVM> CreateAsync()
        {
            try
            {
                var categories = await _categoryRepository.GetAllAsync();
                var colors = await _colorRepository.GetAllAsync();
                var sizes = await _sizeRepository.GetAllAsync();

                return new ProductCreateVM
                {
                    CategoryIds = new List<int>(),
                    ColorIds = new List<int>(),
                    SizeIds = new List<int>()
                };
            }
            catch (Exception)
            {
                _modelState.AddModelError(string.Empty, "Error occurred while preparing the creation form.");
                return new ProductCreateVM();
            }
        }

        public async Task<bool> CreateAsync(ProductCreateVM model)
        {
            if (!_modelState.IsValid) return false;
            try
            {
                var imagePath = _fileService.Upload(model.ImagePath, "assets/images/products");

                var product = new Product
                {
                    Title = model.Title,
                    Price = model.Price,
                    Description = model.Description,
                    ImagePath = "/assets/images/products/" + imagePath,
                    StockCount = model.StockCount,
                    Gender = model.Gender,
                    ProductCategories = new List<ProductCategories>(),
                    ProductColors = new List<ProductColors>(),
                    ProductSizes = new List<ProductSizes>()
                };

                foreach (var categoryId in model.CategoryIds)
                {
                    var category = await _categoryRepository.GetByIdAsync(categoryId);
                    if (category != null)
                    {
                        product.ProductCategories.Add(new ProductCategories { CategoryId = categoryId, Product = product });
                    }
                }

                foreach (var colorId in model.ColorIds)
                {
                    var color = await _colorRepository.GetByIdAsync(colorId);
                    if (color != null)
                    {
                        product.ProductColors.Add(new ProductColors { ColorId = colorId, Product = product });
                    }
                }

                foreach (var sizeId in model.SizeIds)
                {
                    var size = await _sizeRepository.GetByIdAsync(sizeId);
                    if (size != null)
                    {
                        product.ProductSizes.Add(new ProductSizes { SizeId = sizeId, Product = product });
                    }
                }

                await _productRepository.AddAsync(product);
                await _unitOfWork.CommitAsync();

                return true;
            }
            catch (Exception)
            {

                _modelState.AddModelError(string.Empty, "Error occurred while saving the product.");
                return false;
            }
        }

        public async Task<ProductUpdateVM> UpdateAsync(int id)
        {
            try
            {
                var product = await _productRepository.GetByIdWithDetailsAsync(id);
                if (product == null)
                {
                    _modelState.AddModelError(string.Empty, "Product not found.");
                    return null;
                }

                return new ProductUpdateVM
                {
                    Id = product.Id,
                    Title = product.Title,
                    Price = product.Price,
                    Description = product.Description,
                    StockCount = product.StockCount,
                    Gender = product.Gender,
                    CategoryIds = product.ProductCategories.Select(pc => pc.CategoryId).ToList(),
                    ColorIds = product.ProductColors.Select(pc => pc.ColorId).ToList(),
                    SizeIds = product.ProductSizes.Select(ps => ps.SizeId).ToList()
                };

            }
            catch (Exception)
            {

                _modelState.AddModelError(string.Empty, "Error occurred while preparing the edit form.");
                return null;
            }
           
        }

        public async Task<bool> UpdateAsync(int id, ProductUpdateVM model)
        {
            if (!_modelState.IsValid) return false;

            try
            {
                var product = await _productRepository.GetByIdWithDetailsAsync(id);
                if (product == null) return false;

                if (model.ImagePath != null)
                {
                    var oldFileName = Path.GetFileName(product.ImagePath);
                    _fileService.Delete("assets/images/products", oldFileName);

                    var newFileName = _fileService.Upload(model.ImagePath, "assets/images/products");
                    product.ImagePath = "/assets/images/products/" + newFileName;
                }

                product.Title = model.Title;
                product.Price = model.Price;
                product.Description = model.Description;
                product.StockCount = model.StockCount;
                product.Gender = model.Gender;

                product.ProductCategories = model.CategoryIds.Select(id => new ProductCategories { ProductId = product.Id, CategoryId = id }).ToList();
                product.ProductColors = model.ColorIds.Select(id => new ProductColors { ProductId = product.Id, ColorId = id }).ToList();
                product.ProductSizes = model.SizeIds.Select(id => new ProductSizes { ProductId = product.Id, SizeId = id }).ToList();

                _productRepository.Update(product);
                await _unitOfWork.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                _modelState.AddModelError(string.Empty, "Error occurred while updating the product.");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {

            try
            {
                var product = await _productRepository.GetByIdWithDetailsAsync(id);
                if (product == null) return false;

                var fileName = Path.GetFileName(product.ImagePath);
                _fileService.Delete("assets/images/products", fileName);

                _productRepository.Delete(product);
                await _unitOfWork.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                _modelState.AddModelError(string.Empty, "Error occurred while deleting the product.");
                return false;
            }
        }
    }
}
