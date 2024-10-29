using System;
using System.ComponentModel.DataAnnotations;

namespace OnSale.Models;

public class LoginViewModel
{
  [Display(Name = "Email")]
  [Required(ErrorMessage = "The field {0} is required.")]
  [EmailAddress(ErrorMessage = "You must enter a valid email.")]
  public string Username { get; set; }

  [Display(Name = "Password")]
  [Required(ErrorMessage = "The field {0} is required.")]
  [MinLength(6, ErrorMessage = "The field {0} must have at least {1} characters.")]
  public string Password { get; set; }

  [Display(Name = "Remember me on this browser")]
  public bool RememberMe { get; set; }
}
