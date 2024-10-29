using System;
using Microsoft.AspNetCore.Identity;
using OnSale.Data.Entities;
using OnSale.Models;

namespace OnSale.Helpers;

public interface IUserHelper
{
  Task<User> GetUserAsync(string email);

  Task<IdentityResult> AddUserAsync(User user, string password);

  Task CheckRoleAsync(string roleName);

  Task AddUserToRoleAsync(User user, string roleName);

  Task<bool> IsUserInRoleAsync(User user, string roleName);

  Task<SignInResult> LoginAsync(LoginViewModel model);

  Task LogoutAsync();

}

