using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnSale.Data;
using OnSale.Data.Entities;

namespace OnSale.Controllers;

[Authorize(Roles = "Admin")]
public class CategoriesController(DataContext context) : Controller
{
  private readonly DataContext _context = context;
  public async Task<IActionResult> Index()
  {
    return View(await _context.Categories.ToListAsync());
  }

  public async Task<IActionResult> Details(int? id)
  {
    if (id == null)
      return NotFound();

    var category = await _context.Categories.FindAsync(id);
    if (category == null)
      return NotFound();

    return View(category);
  }
  public IActionResult Create()
  {
    return View();
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Create(Category category)
  {
    if (ModelState.IsValid)
    {
      try
      {
        _context.Add(category);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      catch (DbUpdateException dbUpdateException)
      {
        if (dbUpdateException.InnerException != null && dbUpdateException.InnerException.Message.Contains("duplicate"))
        {
          ModelState.AddModelError(string.Empty, "A register with that name already exists.");
        }
        else if (dbUpdateException.InnerException != null)
        {
          ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
        }
        else
        {
          ModelState.AddModelError(string.Empty, "An error occurred while updating the database.");
        }
      }
      catch (Exception exception)
      {
        ModelState.AddModelError(string.Empty, exception.Message);
      }
    }
    return View(category);
  }
  public async Task<IActionResult> Edit(int? id)
  {
    if (id == null)
      return NotFound();

    var country = await _context.Categories.FindAsync(id);
    if (country == null)
      return NotFound();
    return View(country);
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Edit(int id, Category category)
  {
    if (id != category.Id)
      return NotFound();

    if (ModelState.IsValid)
    {
      try
      {
        _context.Update(category);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      catch (DbUpdateException dbUpdateException)
      {
        if (dbUpdateException.InnerException != null && dbUpdateException.InnerException.Message.Contains("duplicate"))
        {
          ModelState.AddModelError(string.Empty, "A register with that name already exists.");
        }
        else if (dbUpdateException.InnerException != null)
        {
          ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
        }
        else
        {
          ModelState.AddModelError(string.Empty, "An error occurred while updating the database.");
        }
      }
      catch (Exception exception)
      {
        ModelState.AddModelError(string.Empty, exception.Message);
      }
    }
    return View(category);
  }
  public async Task<IActionResult> Delete(int? id)
  {
    if (id == null)
      return NotFound();

    var country = await _context.Categories.FindAsync(id);
    if (country == null)
      return NotFound();

    return View(country);
  }

  [HttpPost, ActionName("Delete")]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> DeleteConfirmed(int id)
  {
    var country = await _context.Categories.FindAsync(id);
    if (country != null)
    {
      _context.Categories.Remove(country);
    }

    await _context.SaveChangesAsync();
    return RedirectToAction(nameof(Index));
  }
}
