using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnSale.Data;
using OnSale.Data.Entities;
using OnSale.Data.Enums;
using OnSale.Helpers;
using OnSale.Models;

namespace OnSale.Controllers;

public class AccountController(IUserHelper userHelper, DataContext context, ICombosHelper combosHelper, IBlobHelper blobHelper) : Controller
{
  private readonly IUserHelper _userHelper = userHelper;
  private readonly DataContext _context = context;
  private readonly ICombosHelper _combosHelper = combosHelper;
  private readonly IBlobHelper _blobHelper = blobHelper;

  public IActionResult Login()
  {
    if (User.Identity.IsAuthenticated)
    {
      return RedirectToAction("Index", "Home");
    }

    return View(new LoginViewModel());
  }

  [HttpPost]
  public async Task<IActionResult> Login(LoginViewModel model)
  {
    if (ModelState.IsValid)
    {
      Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.LoginAsync(model);
      if (result.Succeeded)
      {
        return RedirectToAction("Index", "Home");
      }

      ModelState.AddModelError(string.Empty, "Incorrect email or password.");
    }

    return View(model);
  }

  public async Task<IActionResult> Register()
  {
    AddUserViewModel model = new()
    {
      Id = Guid.Empty.ToString(),
      Countries = await _combosHelper.GetComboCountriesAsync(),
      States = await _combosHelper.GetComboStatesAsync(0),
      Cities = await _combosHelper.GetComboCitiesAsync(0),
      UserType = UserType.User,
    };

    return View(model);
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Register(AddUserViewModel model)
  {
    if (ModelState.IsValid)
    {
      Guid imageId = Guid.Empty;

      if (model.ImageFile != null)
      {
        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
      }
      model.ImageId = imageId;
      User user = await _userHelper.AddUserAsync(model);
      if (user == null)
      {
        ModelState.AddModelError(string.Empty, "This email is already in use.");
        model.Countries = await _combosHelper.GetComboCountriesAsync();
        model.States = await _combosHelper.GetComboStatesAsync(model.CountryId);
        model.Cities = await _combosHelper.GetComboCitiesAsync(model.StateId);
        return View(model);
      }

      LoginViewModel loginViewModel = new()
      {
        Password = model.Password,
        RememberMe = false,
        Username = model.Username
      };

      var result2 = await _userHelper.LoginAsync(loginViewModel);

      if (result2.Succeeded)
      {
        return RedirectToAction("Index", "Home");
      }
    }
    model.Countries = await _combosHelper.GetComboCountriesAsync();
    model.States = await _combosHelper.GetComboStatesAsync(model.CountryId);
    model.Cities = await _combosHelper.GetComboCitiesAsync(model.StateId);
    return View(model);
  }
  public JsonResult GetStates(int countryId)
  {
    Country country = _context.Countries.Include(c => c.States).FirstOrDefault(c => c.Id == countryId);
    if (country == null)
      return null;

    return Json(country.States.OrderBy(d => d.Name));
  }

  public JsonResult GetCities(int stateId)
  {
    State state = _context.States.Include(s => s.Cities).FirstOrDefault(s => s.Id == stateId);
    if (state == null)
      return null;

    return Json(state.Cities.OrderBy(c => c.Name));
  }


  public IActionResult NotAuthorized()
  {
    return View();
  }

  public async Task<IActionResult> Logout()
  {
    await _userHelper.LogoutAsync();
    return RedirectToAction("Index", "Home");
  }

}
