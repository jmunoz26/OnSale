using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnSale.Data;
using OnSale.Data.Entities;
using OnSale.Helpers;
using OnSale.Models;

namespace OnSale.Controllers;
[Authorize(Roles = "Admin")]
public class UsersController(DataContext context, IUserHelper userHelper) : Controller
{
  private readonly DataContext _context = context;
  private readonly IUserHelper _userHelper = userHelper;

  public async Task<IActionResult> Index()
  {
    return View(await _context.Users.Include(u => u.City).ThenInclude(c => c.State).ThenInclude(s => s.Country).ToListAsync());
  }

  public async Task<IActionResult> Create()
  {
    AddUserViewModel model = await _userHelper.PrepareAddUserViewModelAsync();
    return View(model);
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Create(AddUserViewModel model)
  {
    if (ModelState.IsValid)
    {
      User? user = await _userHelper.RegisterUserAsync(model);
      if (user == null)
      {
        ModelState.AddModelError(string.Empty, "This email is already in use.");
        await _userHelper.PopulateDropdownsAsync(model);
        return View(model);
      }
      return RedirectToAction("Index", "Home");
    }

    await _userHelper.PopulateDropdownsAsync(model);
    return View(model);
  }

  public JsonResult GetStates(int countryId)
  {
    return _userHelper.GetStates(countryId);
  }

  public JsonResult GetCities(int stateId)
  {
    return _userHelper.GetCities(stateId);
  }

}
