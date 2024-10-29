using System;
using System.ComponentModel.DataAnnotations;

namespace OnSale.Data.Entities;

public class City
{
  public int Id { get; set; }

  [MaxLength(50)]
  [Required]
  public string Name { get; set; }

  public State State { get; set; }
  public ICollection<User> Users { get; set; }
}
