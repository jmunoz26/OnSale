using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnSale.Data;
using OnSale.Helpers;

namespace OnSale.Controllers;

[Authorize(Roles = "Admin")]
public class ProductsController(DataContext context, ICombosHelper combosHelper, IBlobHelper blobHelper) : Controller
{
  private readonly DataContext _context = context;
  private readonly ICombosHelper _combosHelper = combosHelper;
  private readonly IBlobHelper _blobHelper = blobHelper;

  public async Task<IActionResult> Index()
  {
    return View(await _context.Products
        .Include(p => p.ProductImages)
        .Include(p => p.ProductCategories)
        .ThenInclude(pc => pc.Category)
        .ToListAsync());
  }
}

