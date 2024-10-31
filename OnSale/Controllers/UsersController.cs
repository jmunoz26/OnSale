using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnSale.Common;
using OnSale.Data;
using OnSale.Data.Entities;
using OnSale.Helpers;
using OnSale.Models;

namespace OnSale.Controllers;
[Authorize(Roles = "Admin")]
public class UsersController(DataContext context, IUserHelper userHelper, IMailHelper mailHelper) : Controller
{
  private readonly DataContext _context = context;
  private readonly IUserHelper _userHelper = userHelper;
  private readonly IMailHelper _mailHelper = mailHelper;

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
      string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
      string tokenLink = Url.Action("ConfirmEmail", "Account", new
      {
        userid = user.Id,
        token = myToken
      }, protocol: HttpContext.Request.Scheme);

      Response<bool> response = _mailHelper.SendMail(
          $"{model.FirstName} {model.LastName}",
          model.Username,
          "OnSale - Email Confirmation",
          $"<h1>OnSale - Email Confirmation</h1>" +
              $"To enable your account, please click on the following link: " +
              $"<hr/><br/><p><a href = \"{tokenLink}\">Confirm Email</a></p>");
      if (response.IsSuccess)
      {
        ViewBag.Message = "The instructions to enable the account have been sent to the email.";
        return View(model);
      }

      ModelState.AddModelError(string.Empty, response.Message);
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
