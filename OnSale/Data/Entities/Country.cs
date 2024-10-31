using System;
using System.ComponentModel.DataAnnotations;

namespace OnSale.Data.Entities;

public class Country
{
  public int Id { get; set; }

  [MaxLength(50)]
  [Required]
  public string Name { get; set; } = null!;

  public ICollection<State>? States { get; set; }

  [Display(Name = "States")]
  public int StatesNumber => States == null ? 0 : States.Count;

}
