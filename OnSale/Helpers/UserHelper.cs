using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnSale.Data;
using OnSale.Data.Entities;
using OnSale.Models;

namespace OnSale.Helpers;

public class UserHelper(DataContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager) : IUserHelper
{
  private readonly DataContext _context = context;
  private readonly UserManager<User> _userManager = userManager;
  private readonly RoleManager<IdentityRole> _roleManager = roleManager;
  private readonly SignInManager<User> _signInManager = signInManager;

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

  public async Task<User> GetUserAsync(string email)
  {
    return await _context.Users.Include(u => u.City).FirstOrDefaultAsync(u => u.Email == email);
  }

  public async Task<bool> IsUserInRoleAsync(User user, string roleName)
  {
    return await _userManager.IsInRoleAsync(user, roleName);
  }

  public async Task<SignInResult> LoginAsync(LoginViewModel model)
  {
    return await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);
  }

  public async Task LogoutAsync()
  {
    await _signInManager.SignOutAsync();
  }

}
