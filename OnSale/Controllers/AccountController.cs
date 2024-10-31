using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnSale.Common;
using OnSale.Data;
using OnSale.Data.Entities;
using OnSale.Helpers;
using OnSale.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
namespace OnSale.Controllers;

public class AccountController(IUserHelper userHelper, DataContext context, ICombosHelper combosHelper, IBlobHelper blobHelper, IMailHelper mailHelper) : Controller
{
  private readonly IUserHelper _userHelper = userHelper;
  private readonly DataContext _context = context;
  private readonly ICombosHelper _combosHelper = combosHelper;
  private readonly IBlobHelper _blobHelper = blobHelper;
  private readonly IMailHelper _mailHelper = mailHelper;

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
      SignInResult result = await _userHelper.LoginAsync(model);
      if (result.Succeeded)
      {
        return RedirectToAction("Index", "Home");
      }

      if (result.IsLockedOut)
      {
        ModelState.AddModelError(string.Empty, "You have exceeded the maximum number of attempts, your account is blocked, please try again in 5 minutes.");
      }
      else if (result.IsNotAllowed)
      {
        ModelState.AddModelError(string.Empty, "The user has not been enabled, you must follow the instructions in the email sent to enable the user.");
      }

      else
      {
        ModelState.AddModelError(string.Empty, "Incorrect email or password.");
      }

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

      string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
      string tokenLink = Url.Action("ConfirmEmail", "Account", new
      {
        userid = user.Id,
        token = myToken
      }, protocol: HttpContext.Request.Scheme);

      Response response = _mailHelper.SendMail(
          $"{model.FirstName} {model.LastName}",
          model.Username,
          "OnSale - Email Confirmation",
          $"<h1>OnSale - Email Confirmation</h1>" +
              $"To enable your account, please click on the following link: " +
              $"<hr/><br/><p><a href = \"{tokenLink}\">Confirm Email</a></p>");
      if (response.IsSuccess)
      {
        ViewBag.Message = "The instructions to enable your account have been sent to your email.";
        return View(model);
      }

      ModelState.AddModelError(string.Empty, response.Message);
    }
    await _userHelper.PopulateDropdownsAsync(model);
    return View(model);

    // LoginViewModel loginViewModel = new()
    // {
    //   Password = model.Password,
    //   RememberMe = false,
    //   Username = model.Username
    // };

    // var loginSuccessful = await _userHelper.LoginAsync(loginViewModel);
    // if (loginSuccessful.Succeeded)
    // {
    //   return RedirectToAction("Index", "Home");
    // }
  }

  public async Task<IActionResult> ConfirmEmail(string userId, string token)
  {
    if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
    {
      return NotFound();
    }

    User user = await _userHelper.GetUserAsync(new Guid(userId));
    if (user == null)
    {
      return NotFound();
    }

    IdentityResult result = await _userHelper.ConfirmEmailAsync(user, token);
    if (!result.Succeeded)
    {
      return NotFound();
    }

    return View();
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
