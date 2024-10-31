using System;
using OnSale.Common;
using OnSale.Data.Entities;

namespace OnSale.Respository.Interfaces;

public interface ICountriesRepository
{
  Task<Response<Country>> GetAsync(int id);

  Task<Response<IEnumerable<Country>>> GetAsync();

}
