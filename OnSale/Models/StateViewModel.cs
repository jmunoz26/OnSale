using System;
using System.ComponentModel.DataAnnotations;
using OnSale.Data.Entities;

namespace OnSale.Models;

public class StateViewModel
{
  public int Id { get; set; }

  [MaxLength(50)]
  [Required]
  public string Name { get; set; }

  public int CountryId { get; set; }

}
