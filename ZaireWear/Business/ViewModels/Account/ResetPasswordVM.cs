using System.ComponentModel.DataAnnotations;

namespace Business.ViewModels.Account;

public class ResetPasswordVM
{
    [Required(ErrorMessage = "New Password is required")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "Confirm Password is required")]
    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
    public string ConfirmNewPassword { get; set; }

    public string Email { get; set; }
    public string Token { get; set; }
}