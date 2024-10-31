using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnSale.Data;
using OnSale.Data.Entities;
using OnSale.Data.Enums;
using OnSale.Models;

namespace OnSale.Helpers;

public class UserHelper(DataContext context,
  UserManager<User> userManager,
  RoleManager<IdentityRole> roleManager,
  SignInManager<User> signInManager,
  ICombosHelper combosHelper,
  IBlobHelper blobHelper) : IUserHelper
{
  private readonly DataContext _context = context;
  private readonly UserManager<User> _userManager = userManager;
  private readonly RoleManager<IdentityRole> _roleManager = roleManager;
  private readonly SignInManager<User> _signInManager = signInManager;
  private readonly ICombosHelper _combosHelper = combosHelper;
  private readonly IBlobHelper _blobHelper = blobHelper;

  public async Task<IdentityResult> AddUserAsync(User user, string password)
  {
    return await _userManager.CreateAsync(user, password);
  }

  public async Task<User> AddUserAsync(AddUserViewModel model)
  {
    User user = new()
    {
      Address = model.Address,
      Document = model.Document,
      Email = model.Username,
      FirstName = model.FirstName,
      LastName = model.LastName,
      ImageId = model.ImageId,
      PhoneNumber = model.PhoneNumber,
      City = await _context.Cities.FindAsync(model.CityId),
      UserName = model.Username,
      UserType = model.UserType
    };

    IdentityResult result = await _userManager.CreateAsync(user, model.Password);
    if (result != IdentityResult.Success)
    {
      return null;
    }

    User newUser = await GetUserAsync(model.Username);
    await AddUserToRoleAsync(newUser, user.UserType.ToString());
    return newUser;
  }

  public async Task AddUserToRoleAsync(User user, string roleName)
  {
    await _userManager.AddToRoleAsync(user, roleName);
  }
  public async Task CheckRoleAsync(string roleName)
  {
    bool roleExists = await _roleManager.RoleExistsAsync(roleName);
    if (!roleExists)
    {
      await _roleManager.CreateAsync(new IdentityRole
      {
        Name = roleName
      });
    }
  }

  public Microsoft.AspNetCore.Mvc.JsonResult GetCities(int stateId)
  {
    State state = _context.States.Include(s => s.Cities).FirstOrDefault(s => s.Id == stateId);
    if (state == null)
      return null;

    return new Microsoft.AspNetCore.Mvc.JsonResult(state.Cities.OrderBy(c => c.Name));
  }

  public Microsoft.AspNetCore.Mvc.JsonResult GetStates(int countryId)
  {
    Country country = _context.Countries.Include(c => c.States).FirstOrDefault(c => c.Id == countryId);
    if (country == null)
      return null;

    return new Microsoft.AspNetCore.Mvc.JsonResult(country.States.OrderBy(d => d.Name));
  }

  public async Task<User> GetUserAsync(string email)
  {
    return await _context.Users
        .Include(u => u.City)
        .ThenInclude(c => c.State)
        .ThenInclude(s => s.Country)
        .FirstOrDefaultAsync(u => u.Email == email);
  }

  public async Task<User> GetUserAsync(Guid userId)
  {
    return await _context.Users
        .Include(u => u.City)
        .ThenInclude(c => c.State)
        .ThenInclude(s => s.Country)
        .FirstOrDefaultAsync(u => u.Id == userId.ToString());
  }

  public async Task<bool> IsUserInRoleAsync(User user, string roleName)
  {
    return await _userManager.IsInRoleAsync(user, roleName);
  }

  public async Task LogoutAsync()
  {
    await _signInManager.SignOutAsync();
  }

  public async Task PopulateDropdownsAsync(AddUserViewModel model)
  {
    model.Countries = await _combosHelper.GetComboCountriesAsync();
    model.States = await _combosHelper.GetComboStatesAsync(model.CountryId);
    model.Cities = await _combosHelper.GetComboCitiesAsync(model.StateId);
  }

  public async Task<AddUserViewModel> PrepareAddUserViewModelAsync()
  {
    return new AddUserViewModel
    {
      Id = Guid.Empty.ToString(),
      Countries = await _combosHelper.GetComboCountriesAsync(),
      States = await _combosHelper.GetComboStatesAsync(0),
      Cities = await _combosHelper.GetComboCitiesAsync(0),
      UserType = UserType.Admin,
    };
  }

  public async Task<User?> RegisterUserAsync(AddUserViewModel model)
  {
    if (model.ImageFile != null)
    {
      model.ImageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
    }

    return await AddUserAsync(model); ;
  }

  public async Task<IdentityResult> UpdateUserAsync(User user)
  {
    return await _userManager.UpdateAsync(user);
  }
  public async Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword)
  {
    return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
  }

  public async Task<SignInResult> LoginAsync(LoginViewModel model)
  {
    return await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, true);
  }

  public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
  {
    return await _userManager.ConfirmEmailAsync(user, token);
  }

  public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
  {
    return await _userManager.GenerateEmailConfirmationTokenAsync(user);
  }
}
