using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnSale.Models;

public class EditUserViewModel
{
  public string Id { get; set; }

  [Display(Name = "Document")]
  [MaxLength(20, ErrorMessage = "The field {0} must have a maximum of {1} characters.")]
  [Required(ErrorMessage = "The field {0} is required.")]
  public string Document { get; set; }

  [Display(Name = "First Name")]
  [MaxLength(50, ErrorMessage = "The field {0} must have a maximum of {1} characters.")]
  [Required(ErrorMessage = "The field {0} is required.")]
  public string FirstName { get; set; }

  [Display(Name = "Last Name")]
  [MaxLength(50, ErrorMessage = "The field {0} must have a maximum of {1} characters.")]
  [Required(ErrorMessage = "The field {0} is required.")]
  public string LastName { get; set; }

  [Display(Name = "Address")]
  [MaxLength(200, ErrorMessage = "The field {0} must have a maximum of {1} characters.")]
  [Required(ErrorMessage = "The field {0} is required.")]
  public string Address { get; set; }

  [Display(Name = "Phone Number")]
  [MaxLength(20, ErrorMessage = "The field {0} must have a maximum of {1} characters.")]
  [Required(ErrorMessage = "The field {0} is required.")]
  public string PhoneNumber { get; set; }

  [Display(Name = "Photo")]
  public Guid ImageId { get; set; }

  [Display(Name = "Photo")]
  public string ImageFullPath => ImageId == Guid.Empty
      ? $"https://localhost:7057/images/noimage.png"
      : $"https://orderszulu2024.blob.core.windows.net/users/{ImageId}";

  [Display(Name = "Image")]
  public IFormFile? ImageFile { get; set; }

  [Display(Name = "Country")]
  [Range(1, int.MaxValue, ErrorMessage = "You must select a country.")]
  [Required(ErrorMessage = "The field {0} is required.")]
  public int CountryId { get; set; }

  public IEnumerable<SelectListItem> Countries { get; set; }

  [Display(Name = "State/Department")]
  [Range(1, int.MaxValue, ErrorMessage = "You must select a state/department.")]
  [Required(ErrorMessage = "The field {0} is required.")]
  public int StateId { get; set; }

  public IEnumerable<SelectListItem> States { get; set; }

  [Display(Name = "City")]
  [Range(1, int.MaxValue, ErrorMessage = "You must select a city.")]
  public int CityId { get; set; }

  public IEnumerable<SelectListItem> Cities { get; set; }

}
