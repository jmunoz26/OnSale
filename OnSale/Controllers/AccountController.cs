using System;
using Microsoft.AspNetCore.Mvc;
using OnSale.Data;
using OnSale.Helpers;
using OnSale.Models;

namespace OnSale.Controllers;

public class AccountController(IUserHelper userHelper) : Controller
{
  private readonly IUserHelper _userHelper = userHelper;

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
