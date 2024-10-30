using System;
using System.ComponentModel.DataAnnotations;

namespace OnSale.Models;

public class ChangePasswordViewModel
{
  [DataType(DataType.Password)]
  [Display(Name = "Current Password")]
  [StringLength(20, MinimumLength = 6, ErrorMessage = "The field {0} must have between {2} and {1} characters.")]
  [Required(ErrorMessage = "The field {0} is required.")]
  public string OldPassword { get; set; }

  [DataType(DataType.Password)]
  [Display(Name = "New Password")]
  [StringLength(20, MinimumLength = 6, ErrorMessage = "The field {0} must have between {2} and {1} characters.")]
  [Required(ErrorMessage = "The field {0} is required.")]
  public string NewPassword { get; set; }

  [Compare("NewPassword", ErrorMessage = "The new password and confirmation do not match.")]
  [DataType(DataType.Password)]
  [Display(Name = "New Password Confirmation")]
  [StringLength(20, MinimumLength = 6, ErrorMessage = "The field {0} must have between {2} and {1} characters.")]
  [Required(ErrorMessage = "The field {0} is required.")]
  public string Confirm { get; set; }
}
