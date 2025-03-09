using Core.Constants.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities;
    public class Product : BaseEntity
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int StockCount { get; set; }
        public Gender Gender { get; set; }
        public ICollection<ProductCategories> ProductCategories { get; set; }
        public ICollection<ProductColors> ProductColors { get; set; }
        public ICollection<ProductSizes> ProductSizes { get; set; }
        public ICollection<BasketProduct> BasketProducts { get; set; }
        public ICollection<FavoriteProduct> FavoriteProducts { get; set; }
    }
