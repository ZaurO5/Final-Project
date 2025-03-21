﻿using Core.Entities;

namespace Data.Repositories.Abstract;

public interface IBasketProductRepository 
{
    Task<List<BasketProduct>> GetBasketProductsWithProducts(int userBasketId);
    Task<BasketProduct> GetByIdAsync(int basketId, int productId, int colorId, int sizeId);
    Task<BasketProduct> GetByVariantAsync(int basketId, int productId, int ColorId, int SizeId);
    Task<List<BasketProduct>> GetAllAsync();
    Task CreateAsync(BasketProduct data);
    void Update(BasketProduct data);
    void Delete(BasketProduct data);
}