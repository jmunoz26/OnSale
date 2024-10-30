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
    AddUserViewModel model = await _userHelper.PrepareAddUserViewModelAsync();
    return View(model);
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Register(AddUserViewModel model)
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

      LoginViewModel loginViewModel = new()
      {
        Password = model.Password,
        RememberMe = false,
        Username = model.Username
      };

      var loginSuccessful = await _userHelper.LoginAsync(loginViewModel);
      if (loginSuccessful.Succeeded)
      {
        return RedirectToAction("Index", "Home");
      }
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

  public async Task<IActionResult> ChangeUser()
  {
    User user = await _userHelper.GetUserAsync(User.Identity.Name);
    if (user == null)
      return NotFound();

    EditUserViewModel model = new()
    {
      Address = user.Address,
      FirstName = user.FirstName,
      LastName = user.LastName,
      PhoneNumber = user.PhoneNumber,
      ImageId = user.ImageId,
      Cities = await _combosHelper.GetComboCitiesAsync(user.City.State.Id),
      CityId = user.City.Id,
      Countries = await _combosHelper.GetComboCountriesAsync(),
      CountryId = user.City.State.Country.Id,
      StateId = user.City.State.Id,
      States = await _combosHelper.GetComboStatesAsync(user.City.State.Country.Id),
      Id = user.Id,
      Document = user.Document
    };

    return View(model);
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> ChangeUser(EditUserViewModel model)
  {
    if (ModelState.IsValid)
    {
      Guid imageId = model.ImageId;

      if (model.ImageFile != null)
      {
        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
      }

      User user = await _userHelper.GetUserAsync(User.Identity.Name);

      user.FirstName = model.FirstName;
      user.LastName = model.LastName;
      user.Address = model.Address;
      user.PhoneNumber = model.PhoneNumber;
      user.ImageId = imageId;
      user.City = await _context.Cities.FindAsync(model.CityId);
      user.Document = model.Document;

      await _userHelper.PrepareAddUserViewModelAsync();
      await _userHelper.UpdateUserAsync(user);
      return RedirectToAction("Index", "Home");
    }

    return View(model);
  }

  public IActionResult ChangePassword()
  {
    return View();
  }

  [HttpPost]
  public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
  {
    if (ModelState.IsValid)
    {
      if (model.OldPassword == model.NewPassword)
      {
        ModelState.AddModelError(string.Empty, "You must provide a different password");
        return View(model);
      }
      User user = await _userHelper.GetUserAsync(User.Identity.Name);
      if (user != null)
      {
        var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
        if (result.Succeeded)
        {
          return RedirectToAction("ChangeUser");
        }
        else
        {
          ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
        }
      }
      else
      {
        ModelState.AddModelError(string.Empty, "User no found.");
      }
    }

    return View(model);
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
