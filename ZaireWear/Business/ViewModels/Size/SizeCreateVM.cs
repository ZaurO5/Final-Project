using System.ComponentModel.DataAnnotations;

namespace Business.ViewModels.Size;

public class SizeCreateVM
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }
}
