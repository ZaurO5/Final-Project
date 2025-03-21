using System.ComponentModel.DataAnnotations;

namespace Business.ViewModels.Category;

public class CategoryCreateVM
{
    [Required(ErrorMessage = "Name is required")]
    [MinLength(3, ErrorMessage = "At least 3 characters")]
    public string Name { get; set; }
}
