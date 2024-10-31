using System;
using Microsoft.EntityFrameworkCore;
using OnSale.Common;
using OnSale.Data;
using OnSale.Data.Entities;
using OnSale.Respository.Interfaces;

namespace OnSale.Respository.Implementations;

public class CountriesRepository(DataContext context) : GenericRepository<Country>(context), ICountriesRepository
{
  private readonly DataContext _context = context;

  public async override Task<Response<IEnumerable<Country>>> GetAsync()
  {
    var country = await _context.Countries.Include(c => c.States).ToListAsync();

    return new Response<IEnumerable<Country>>
    {
      IsSuccess = true,
      Result = country
    };
  }

  public async override Task<Response<Country>> GetAsync(int id)
  {
    var country = await _context.Countries.Include(c => c.States).ThenInclude(s => s.Cities).FirstOrDefaultAsync(m => m.Id == id);

    if (country == null)
    {
      return new Response<Country>
      {
        IsSuccess = false,
        Message = "Record no found"
      };
    }

    return new Response<Country>
    {
      IsSuccess = true,
      Result = country
    };
  }

}
