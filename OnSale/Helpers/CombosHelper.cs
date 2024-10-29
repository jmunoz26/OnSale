using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnSale.Data;

namespace OnSale.Helpers;

public class CombosHelper(DataContext context) : ICombosHelper
{
  private readonly DataContext _context = context;
  public async Task<IEnumerable<SelectListItem>> GetComboCategoriesAsync()
  {
    List<SelectListItem> list = await _context.Categories.Select(x => new SelectListItem
    {
      Text = x.Name,
      Value = $"{x.Id}"
    }).OrderBy(x => x.Text).ToListAsync();

    list.Insert(0, new SelectListItem { Text = "[Select a category...]", Value = "0" });
    return list;
  }

  public async Task<IEnumerable<SelectListItem>> GetComboCitiesAsync(int stateId)
  {
    List<SelectListItem> list = await _context.Cities.Where(x => x.State.Id == stateId).Select(x => new SelectListItem
    {
      Text = x.Name,
      Value = $"{x.Id}"
    }).OrderBy(x => x.Text).ToListAsync();

    list.Insert(0, new SelectListItem { Text = "[Select a city...]", Value = "0" });
    return list;
  }

  public async Task<IEnumerable<SelectListItem>> GetComboCountriesAsync()
  {
    List<SelectListItem> list = await _context.Countries.Select(x => new SelectListItem
    {
      Text = x.Name,
      Value = $"{x.Id}"
    }).OrderBy(x => x.Text).ToListAsync();

    list.Insert(0, new SelectListItem { Text = "[Select a country...]", Value = "0" });
    return list;
  }

  public async Task<IEnumerable<SelectListItem>> GetComboStatesAsync(int countryId)
  {
    List<SelectListItem> list = await _context.States.Where(x => x.Country.Id == countryId).Select(x => new SelectListItem
    {
      Text = x.Name,
      Value = $"{x.Id}"
    }).OrderBy(x => x.Text).ToListAsync();

    list.Insert(0, new SelectListItem { Text = "[Select a state...]", Value = "0" });
    return list;
  }
}
