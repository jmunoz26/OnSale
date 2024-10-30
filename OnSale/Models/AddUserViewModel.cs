using System;
using System.ComponentModel.DataAnnotations;
using OnSale.Data.Enums;

namespace OnSale.Models;

public class AddUserViewModel : EditUserViewModel
{
  [Display(Name = "Email")]
  [EmailAddress(ErrorMessage = "You must enter a valid email.")]
  [MaxLength(100, ErrorMessage = "The field {0} must have a maximum of {1} characters.")]
  [Required(ErrorMessage = "The field {0} is required.")]
  public string Username { get; set; }

  [DataType(DataType.Password)]
  [Display(Name = "Password")]
  [Required(ErrorMessage = "The field {0} is required.")]
  [StringLength(20, MinimumLength = 6, ErrorMessage = "The field {0} must have between {2} and {1} characters.")]
  public string Password { get; set; }

  [Compare("Password", ErrorMessage = "The password and confirmation do not match.")]
  [Display(Name = "Password Confirmation")]
  [DataType(DataType.Password)]
  [Required(ErrorMessage = "The field {0} is required.")]
  [StringLength(20, MinimumLength = 6, ErrorMessage = "The field {0} must have between {2} and {1} characters.")]
  public string PasswordConfirm { get; set; }

  [Display(Name = "User Type")]
  public UserType UserType { get; set; }

}
