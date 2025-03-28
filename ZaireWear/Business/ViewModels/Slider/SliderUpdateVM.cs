﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Business.ViewModels.Slider;

public class SliderUpdateVM
{
    [Required(ErrorMessage = "Title is required")]
    [MinLength(5, ErrorMessage = "At least 5 characters")]
    public string Title { get; set; }

    public string Subtitle { get; set; }

    public IFormFile? ImagePath { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Order must be at least 1")]
    public int Order { get; set; }
}