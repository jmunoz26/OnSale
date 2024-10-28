using System;
using System.ComponentModel.DataAnnotations;

namespace OnSale.Models;

public class CityViewModel
{
  public int Id { get; set; }

  [MaxLength(50)]
  [Required]
  public string Name { get; set; }

  public int StateId { get; set; }

}
