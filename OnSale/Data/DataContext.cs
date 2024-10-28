using System;
using Microsoft.EntityFrameworkCore;
using OnSale.Data.Entities;

namespace OnSale.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
  public DbSet<Category> Categories { get; set; }

  public DbSet<City> Cities { get; set; }

  public DbSet<Country> Countries { get; set; }

  public DbSet<State> States { get; set; }


  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<Category>().HasIndex(c => c.Name).IsUnique();
    modelBuilder.Entity<City>().HasIndex("Name", "StateId").IsUnique();
    modelBuilder.Entity<Country>().HasIndex(c => c.Name).IsUnique();
    modelBuilder.Entity<State>().HasIndex("Name", "CountryId").IsUnique();

  }

}
