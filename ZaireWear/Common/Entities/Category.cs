﻿namespace Core.Entities;
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<ProductCategories> ProductCategories { get; set; }
    }
