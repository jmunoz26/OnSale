using System;
using Microsoft.AspNetCore.Identity;
using OnSale.Data.Entities;
using OnSale.Models;

namespace OnSale.Helpers;

public interface IUserHelper
{
  Task<User> GetUserAsync(string email);
  Task<User> GetUserAsync(Guid userId);

  Task<IdentityResult> AddUserAsync(User user, string password);
  Task<User> AddUserAsync(AddUserViewModel model);

  Task CheckRoleAsync(string roleName);

  Task AddUserToRoleAsync(User user, string roleName);

  Task<bool> IsUserInRoleAsync(User user, string roleName);

  Task<SignInResult> LoginAsync(LoginViewModel model);
  Task LogoutAsync();
  Task<AddUserViewModel> PrepareAddUserViewModelAsync();
  Task<User?> RegisterUserAsync(AddUserViewModel model);
  Task PopulateDropdownsAsync(AddUserViewModel model);
  Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);
  Task<IdentityResult> UpdateUserAsync(User user);
  Microsoft.AspNetCore.Mvc.JsonResult GetStates(int countryId);
  Microsoft.AspNetCore.Mvc.JsonResult GetCities(int stateId);
}

