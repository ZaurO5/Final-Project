using Business.ViewModels.Basket;
namespace Business.Services.Abstract;

public interface IBasketService
{
    Task<BasketIndexVM> GetAllAsync();
    Task<(int statusCode, string description)> AddProductAsync(int productId, int ColorId, int SizeId, int quantity);
    Task<(int statusCode, string description)> UpdateCartAsync(List<BasketUpdateVM> updatedProducts);
    Task<(int statusCode, string description)> DeleteAsync(int id);
}