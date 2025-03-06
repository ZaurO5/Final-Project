using Core.Constants;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModels.Product
{
    public class ProductCreateVM
    {
        [Required(ErrorMessage = "Title is required")]
        [MinLength(5, ErrorMessage = "At least 5 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Image is required")]
        public IFormFile ImagePath { get; set; }

        [Required(ErrorMessage = "Stock count is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Stock count must be greater than 0")]
        public int StockCount { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "At least one category is required")]
        public List<int> CategoryIds { get; set; }

        [Required(ErrorMessage = "At least one color is required")]
        public List<int> ColorIds { get; set; }

        [Required(ErrorMessage = "At least one size is required")]
        public List<int> SizeIds { get; set; }
    }
}
