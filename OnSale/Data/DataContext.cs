using System;
using Microsoft.EntityFrameworkCore;
using OnSale.Data.Entities;

namespace OnSale.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
  public DbSet<Country> Countries { get; set; }
  public DbSet<Category> Categories { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<Country>().HasIndex(c => c.Name).IsUnique();
    modelBuilder.Entity<Category>().HasIndex(c => c.Name).IsUnique();
  }

}
