using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OnSale.Data.Entities;

public class City
{
  public int Id { get; set; }

  [MaxLength(50)]
  [Required]
  public string Name { get; set; } = null!;

  [JsonIgnore]
  public State State { get; set; } = null!;
  public ICollection<User>? Users { get; set; }
}
