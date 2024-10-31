using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnSale.Data;
using OnSale.Data.Entities;
using OnSale.Models;
using OnSale.Respository;
using OnSale.Respository.Interfaces;

namespace OnSale.Controllers;
[Authorize(Roles = "Admin")]
public class CountriesController(IGenericRepository<Country> repository, ICountriesRepository countriesRepository) : GenericControlle<Country>(repository)
{
    private readonly ICountriesRepository _countriesRepository = countriesRepository;

    public async Task<IActionResult> Index()
    {
        var country = await _countriesRepository.GetAsync();
        return View(country.Result);
    }

    public async Task<IActionResult> Details(int id)
    {
        var country = await _countriesRepository.GetAsync(id);

        if (country == null)
            return NotFound();

        return View(country.Result);
    }


    // public async Task<IActionResult> DetailsState(int? id)
    // {
    //     if (id == null)
    //         return NotFound();

    //     State state = await _context.States.Include(c => c.Cities).Include(s => s.Country).FirstOrDefaultAsync(m => m.Id == id);
    //     if (state == null)
    //         return NotFound();

    //     return View(state);
    // }

    // public async Task<IActionResult> DetailsCity(int? id)
    // {
    //     if (id == null)
    //         return NotFound();

    //     City city = await _context.Cities.Include(s => s.State).FirstOrDefaultAsync(m => m.Id == id);
    //     if (city == null)
    //         return NotFound();

    //     return View(city);
    // }

    // public IActionResult Create()
    // {
    //     Country country = new() { States = [] };
    //     return View(country);
    // }

    // [HttpPost]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> Create(Country country)
    // {
    //     if (ModelState.IsValid)
    //     {
    //         try
    //         {
    //             _context.Add(country);
    //             await _context.SaveChangesAsync();
    //             return RedirectToAction(nameof(Index));
    //         }
    //         catch (DbUpdateException dbUpdateException)
    //         {
    //             if (dbUpdateException.InnerException != null && dbUpdateException.InnerException.Message.Contains("duplicate"))
    //             {
    //                 ModelState.AddModelError(string.Empty, "A register with that name already exists.");
    //             }
    //             else if (dbUpdateException.InnerException != null)
    //             {
    //                 ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
    //             }
    //             else
    //             {
    //                 ModelState.AddModelError(string.Empty, "An error occurred while updating the database.");
    //             }
    //         }
    //         catch (Exception exception)
    //         {
    //             ModelState.AddModelError(string.Empty, exception.Message);
    //         }
    //     }
    //     return View(country);
    // }

    // public async Task<IActionResult> AddState(int? id)
    // {
    //     if (id == null)
    //         return NotFound();

    //     Country country = await _context.Countries.FindAsync(id);

    //     if (country == null)
    //         return NotFound();

    //     StateViewModel model = new()
    //     {
    //         CountryId = country.Id
    //     };
    //     return View(model);
    // }

    // [HttpPost]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> AddState(StateViewModel model)
    // {
    //     if (ModelState.IsValid)
    //     {
    //         try
    //         {
    //             State state = new()
    //             {
    //                 Cities = [],
    //                 Country = await _context.Countries.FindAsync(model.CountryId),
    //                 Name = model.Name
    //             };
    //             _context.Add(state);
    //             await _context.SaveChangesAsync();
    //             return RedirectToAction(nameof(Details), new { id = model.CountryId });
    //         }
    //         catch (DbUpdateException dbUpdateException)
    //         {
    //             if (dbUpdateException.InnerException != null && dbUpdateException.InnerException.Message.Contains("duplicate"))
    //             {
    //                 ModelState.AddModelError(string.Empty, "A register with that name already exists.");
    //             }
    //             else if (dbUpdateException.InnerException != null)
    //             {
    //                 ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
    //             }
    //             else
    //             {
    //                 ModelState.AddModelError(string.Empty, "An error occurred while updating the database.");
    //             }
    //         }
    //         catch (Exception exception)
    //         {
    //             ModelState.AddModelError(string.Empty, exception.Message);
    //         }
    //     }
    //     return View(model);
    // }

    // public async Task<IActionResult> AddCity(int? id)
    // {
    //     if (id == null)
    //         return NotFound();

    //     State state = await _context.States.FindAsync(id);

    //     if (state == null)
    //         return NotFound();

    //     CityViewModel model = new()
    //     {
    //         StateId = state.Id
    //     };
    //     return View(model);
    // }

    // [HttpPost]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> AddCity(CityViewModel model)
    // {
    //     if (ModelState.IsValid)
    //     {
    //         try
    //         {
    //             City city = new()
    //             {
    //                 State = await _context.States.FindAsync(model.StateId),
    //                 Name = model.Name
    //             };
    //             _context.Add(city);
    //             await _context.SaveChangesAsync();
    //             return RedirectToAction(nameof(DetailsState), new { id = model.StateId });
    //         }
    //         catch (DbUpdateException dbUpdateException)
    //         {
    //             if (dbUpdateException.InnerException != null && dbUpdateException.InnerException.Message.Contains("duplicate"))
    //             {
    //                 ModelState.AddModelError(string.Empty, "A register with that name already exists.");
    //             }
    //             else if (dbUpdateException.InnerException != null)
    //             {
    //                 ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
    //             }
    //             else
    //             {
    //                 ModelState.AddModelError(string.Empty, "An error occurred while updating the database.");
    //             }
    //         }
    //         catch (Exception exception)
    //         {
    //             ModelState.AddModelError(string.Empty, exception.Message);
    //         }
    //     }
    //     return View(model);
    // }


    // public async Task<IActionResult> Edit(int? id)
    // {
    //     if (id == null)
    //         return NotFound();

    //     Country country = await _context.Countries.Include(s => s.States).FirstOrDefaultAsync(c => c.Id == id);
    //     if (country == null)
    //         return NotFound();
    //     return View(country);
    // }

    // [HttpPost]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> Edit(int id, Country country)
    // {
    //     if (id != country.Id)
    //         return NotFound();

    //     if (ModelState.IsValid)
    //     {
    //         try
    //         {
    //             _context.Update(country);
    //             await _context.SaveChangesAsync();
    //             return RedirectToAction(nameof(Index));
    //         }
    //         catch (DbUpdateException dbUpdateException)
    //         {
    //             if (dbUpdateException.InnerException != null && dbUpdateException.InnerException.Message.Contains("duplicate"))
    //             {
    //                 ModelState.AddModelError(string.Empty, "A register with that name already exists.");
    //             }
    //             else if (dbUpdateException.InnerException != null)
    //             {
    //                 ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
    //             }
    //             else
    //             {
    //                 ModelState.AddModelError(string.Empty, "An error occurred while updating the database.");
    //             }
    //         }
    //         catch (Exception exception)
    //         {
    //             ModelState.AddModelError(string.Empty, exception.Message);
    //         }
    //     }
    //     return View(country);
    // }

    // public async Task<IActionResult> EditState(int? id)
    // {
    //     if (id == null)
    //         return NotFound();

    //     State state = await _context.States.Include(s => s.Country).FirstOrDefaultAsync(c => c.Id == id);
    //     if (state == null)
    //         return NotFound();

    //     StateViewModel model = new()
    //     {
    //         CountryId = state.Country.Id,
    //         Id = state.Id,
    //         Name = state.Name,
    //     };
    //     return View(model);
    // }

    // [HttpPost]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> EditState(int id, StateViewModel model)
    // {
    //     if (id != model.Id)
    //         return NotFound();

    //     if (ModelState.IsValid)
    //     {
    //         try
    //         {
    //             State state = new()
    //             {
    //                 Name = model.Name,
    //                 Id = model.Id,
    //             };
    //             _context.Update(state);
    //             await _context.SaveChangesAsync();
    //             return RedirectToAction(nameof(Details), new { Id = model.CountryId });
    //         }
    //         catch (DbUpdateException dbUpdateException)
    //         {
    //             if (dbUpdateException.InnerException != null && dbUpdateException.InnerException.Message.Contains("duplicate"))
    //             {
    //                 ModelState.AddModelError(string.Empty, "A register with that name already exists.");
    //             }
    //             else if (dbUpdateException.InnerException != null)
    //             {
    //                 ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
    //             }
    //             else
    //             {
    //                 ModelState.AddModelError(string.Empty, "An error occurred while updating the database.");
    //             }
    //         }
    //         catch (Exception exception)
    //         {
    //             ModelState.AddModelError(string.Empty, exception.Message);
    //         }
    //     }
    //     return View(model);
    // }


    // public async Task<IActionResult> EditCity(int? id)
    // {
    //     if (id == null)
    //         return NotFound();

    //     City city = await _context.Cities.Include(s => s.State).FirstOrDefaultAsync(c => c.Id == id);
    //     if (city == null)
    //         return NotFound();

    //     CityViewModel model = new()
    //     {
    //         StateId = city.State.Id,
    //         Id = city.Id,
    //         Name = city.Name,
    //     };
    //     return View(model);
    // }

    // [HttpPost]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> EditCity(int id, CityViewModel model)
    // {
    //     if (id != model.Id)
    //         return NotFound();

    //     if (ModelState.IsValid)
    //     {
    //         try
    //         {
    //             City city = new()
    //             {
    //                 Name = model.Name,
    //                 Id = model.Id,
    //             };
    //             _context.Update(city);
    //             await _context.SaveChangesAsync();
    //             return RedirectToAction(nameof(DetailsState), new { Id = model.StateId });
    //         }
    //         catch (DbUpdateException dbUpdateException)
    //         {
    //             if (dbUpdateException.InnerException != null && dbUpdateException.InnerException.Message.Contains("duplicate"))
    //             {
    //                 ModelState.AddModelError(string.Empty, "A register with that name already exists.");
    //             }
    //             else if (dbUpdateException.InnerException != null)
    //             {
    //                 ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
    //             }
    //             else
    //             {
    //                 ModelState.AddModelError(string.Empty, "An error occurred while updating the database.");
    //             }
    //         }
    //         catch (Exception exception)
    //         {
    //             ModelState.AddModelError(string.Empty, exception.Message);
    //         }
    //     }
    //     return View(model);
    // }



    // public async Task<IActionResult> DeleteState(int? id)
    // {
    //     if (id == null)
    //         return NotFound();

    //     State state = await _context.States.Include(c => c.Country).FirstOrDefaultAsync(s => s.Id == id);
    //     if (state == null)
    //         return NotFound();

    //     return View(state);
    // }

    // [HttpPost, ActionName("DeleteState")]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> DeleteStateConfirmed(int id)
    // {
    //     State state = await _context.States.Include(c => c.Country).FirstOrDefaultAsync(s => s.Id == id);
    //     if (state != null)
    //     {
    //         _context.States.Remove(state);
    //     }

    //     await _context.SaveChangesAsync();
    //     return RedirectToAction(nameof(Details), new { Id = state.Country.Id });
    // }
    // public async Task<IActionResult> DeleteCity(int? id)
    // {
    //     if (id == null)
    //         return NotFound();

    //     City city = await _context.Cities.Include(c => c.State).FirstOrDefaultAsync(s => s.Id == id);
    //     if (city == null)
    //         return NotFound();

    //     return View(city);
    // }

    // [HttpPost, ActionName("DeleteCity")]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> DeleteCityConfirmed(int id)
    // {
    //     City city = await _context.Cities.Include(c => c.State).FirstOrDefaultAsync(s => s.Id == id);
    //     if (city != null)
    //     {
    //         _context.Cities.Remove(city);
    //     }

    //     await _context.SaveChangesAsync();
    //     return RedirectToAction(nameof(DetailsState), new { Id = city.State.Id });
    // }

}
