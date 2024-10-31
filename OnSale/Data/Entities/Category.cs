using System;
using System.ComponentModel.DataAnnotations;

namespace OnSale.Data.Entities;

public class Category
{
  public int Id { get; set; }

  [Display]
  [MaxLength(50)]
  [Required]
  public string Name { get; set; } = null!;
}
