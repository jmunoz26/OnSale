using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OnSale.Data.Entities;

public class State
{
  public int Id { get; set; }

  [MaxLength(50)]
  [Required]
  public string Name { get; set; } = null!;

  [JsonIgnore]
  public Country Country { get; set; } = null!;
  public ICollection<City>? Cities { get; set; }


  [Display(Name = "Cities")]
  public int CitiesNumber => Cities == null ? 0 : Cities.Count;
}
