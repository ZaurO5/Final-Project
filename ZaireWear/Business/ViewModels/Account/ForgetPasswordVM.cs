using System.ComponentModel.DataAnnotations;

namespace Business.ViewModels.Account;

public class ForgetPasswordVM
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string Email { get; set; }
}