﻿using System.ComponentModel.DataAnnotations;

namespace Business.ViewModels.Account;

public class AccountLoginVM
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public string? ReturnUrl { get; set; }
}