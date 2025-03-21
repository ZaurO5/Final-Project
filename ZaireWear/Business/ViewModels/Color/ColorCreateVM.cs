using System.ComponentModel.DataAnnotations;

namespace Business.ViewModels.Color;

public class ColorCreateVM
{
    [Required(ErrorMessage = "Color name is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Name must be between 3-50 characters")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Hex code is required")]
    [RegularExpression("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "Invalid hex color format")]
    public string HexCode { get; set; }
}
