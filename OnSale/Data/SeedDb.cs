using System;
using OnSale.Data.Entities;
using OnSale.Data.Enums;
using OnSale.Helpers;

namespace OnSale.Data;

public class SeedDb(DataContext context, IUserHelper userHelper)
{
  private readonly DataContext _context = context;
  private readonly IUserHelper _userHelper = userHelper;

  public async Task SeedAsync()
  {
    await _context.Database.EnsureCreatedAsync();
    await CheckCategoryAsync();
    await CheckCountriesAsync();
    await CheckRolesAsync();
    await CheckUserAsync("1010", "Jairo", "Munoz", "jmunoz@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "noimage.jpeg", UserType.Admin);
    await CheckUserAsync("2020", "Ledys", "Bedoya", "ledys@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", "noimage.jpeg", UserType.User);
  }

  private async Task<User> CheckUserAsync(
            string document,
            string firstName,
            string lastName,
            string email,
            string phone,
            string address,
            string image,
            UserType userType)
  {
    User user = await _userHelper.GetUserAsync(email);
    if (user == null)
    {
      user = new User
      {
        FirstName = firstName,
        LastName = lastName,
        Email = email,
        UserName = email,
        PhoneNumber = phone,
        Address = address,
        Document = document,
        City = _context.Cities.FirstOrDefault(),
        UserType = userType,
      };

      await _userHelper.AddUserAsync(user, "123456");
      await _userHelper.AddUserToRoleAsync(user, userType.ToString());

      string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
      await _userHelper.ConfirmEmailAsync(user, token);

    }

    return user;
  }

  private async Task CheckRolesAsync()
  {
    await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
    await _userHelper.CheckRoleAsync(UserType.User.ToString());
  }

  private async Task CheckCountriesAsync()
  {
    if (!_context.Countries.Any())
    {
      _context.Countries.Add(new Country
      {
        Name = "Dominican Republic",
        States =
        [
          new() {
            Name = "Santo Domingo",
            Cities = [
              new City { Name =  "Lotes y Servicio"},
              new City { Name =  "Los tres brasos"}
            ]
          }
        ]

      });

    }

    await _context.SaveChangesAsync();

  }

  private async Task CheckCategoryAsync()
  {
    if (!_context.Categories.Any())
    {
      _context.Categories.Add(new Category { Name = "Technology" });
      _context.Categories.Add(new Category { Name = "Clothes" });
      _context.Categories.Add(new Category { Name = "Beauty" });
      _context.Categories.Add(new Category { Name = "Shoes" });
      _context.Categories.Add(new Category { Name = "Sports" });
      _context.Categories.Add(new Category { Name = "Pets" });
      _context.Categories.Add(new Category { Name = "Apple" });
      await _context.SaveChangesAsync();
    }
  }
}
