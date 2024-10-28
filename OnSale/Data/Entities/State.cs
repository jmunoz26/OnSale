using System;
using System.ComponentModel.DataAnnotations;

namespace OnSale.Data.Entities;

public class State
{
  public int Id { get; set; }

  [MaxLength(50)]
  [Required]
  public string Name { get; set; }

  public Country Country { get; set; }
  public ICollection<City> Cities { get; set; }


  [Display(Name = "Cities")]
  public int CitiesNumber => Cities == null ? 0 : Cities.Count;
}
