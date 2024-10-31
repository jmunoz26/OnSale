using Microsoft.AspNetCore.Mvc;
using OnSale.Respository;

namespace OnSale.Controllers;

public class GenericControlle<T>(IGenericRepository<T> repository) : Controller where T : class
{
  private readonly IGenericRepository<T> _repository = repository;

  public async Task<IActionResult> Delete(int id)
  {
    var row = await _repository.GetAsync(id);
    if (row == null)
      return NotFound();

    return View(row.Result);
  }

  [HttpPost, ActionName("Delete")]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> DeleteConfirmed(int id)
  {
    var row = await _repository.DeleteAsync(id);
    return RedirectToAction(nameof(Index));
  }
  // public async Task<IActionResult> DetailsCity(int? id)
  // {
  //   if (id == null)
  //     return NotFound();

  //   var row = await _repository.DeleteAsync(id);
  //   if (row == null)
  //     return NotFound();

  //   return View(row);
  // }
}
