using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using OnSale.Data.Enums;

namespace OnSale.Data.Entities;

public class User : IdentityUser
{
  [Display(Name = "Document")]
  [MaxLength(20, ErrorMessage = "The field {0} must have a maximum of {1} characters.")]
  [Required(ErrorMessage = "The field {0} is required.")]
  public string Document { get; set; } = null!;

  [Display(Name = "First Name")]
  [MaxLength(50, ErrorMessage = "The field {0} must have a maximum of {1} characters.")]
  [Required(ErrorMessage = "The field {0} is required.")]
  public string FirstName { get; set; } = null!;

  [Display(Name = "Last Name")]
  [MaxLength(50, ErrorMessage = "The field {0} must have a maximum of {1} characters.")]
  [Required(ErrorMessage = "The field {0} is required.")]
  public string LastName { get; set; } = null!;

  [Display(Name = "Address")]
  [MaxLength(200, ErrorMessage = "The field {0} must have a maximum of {1} characters.")]
  [Required(ErrorMessage = "The field {0} is required.")]
  public string Address { get; set; } = null!;

  [Display(Name = "Photo")]
  public Guid ImageId { get; set; }

  [Display(Name = "Photo")]
  public string ImageFullPath => ImageId == Guid.Empty
      ? $"https://localhost:5051/images/noimage.png"
      : $"https://orderszulu2024.blob.core.windows.net/users/{ImageId}";

  [Display(Name = "User Type")]
  public UserType UserType { get; set; }

  [Display(Name = "City")]
  public City City { get; set; } = null!;

  [Display(Name = "User")]
  public string FullName => $"{FirstName} {LastName}";

  [Display(Name = "User")]
  public string FullNameWithDocument => $"{FirstName} {LastName} - {Document}";
}

