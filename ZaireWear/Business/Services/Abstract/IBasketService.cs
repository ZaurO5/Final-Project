using Business.ViewModels.Basket;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Abstract;

public interface IBasketService
{
    Task<BasketIndexVM> GetAllAsync();
    Task<(int statusCode, string description)> AddProductAsync(int productId, string color, string size, int quantity);
    Task<(int statusCode, string description)> UpdateCartAsync(List<BasketUpdateVM> updatedProducts);
    Task<(int statusCode, string description)> DeleteAsync(int id);
}