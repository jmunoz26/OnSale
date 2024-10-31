using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using OnSale.Data.Entities;

namespace OnSale.Data;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<User>(options)
{
  public DbSet<Category> Categories { get; set; }

  public DbSet<City> Cities { get; set; }

  public DbSet<Country> Countries { get; set; }

  public DbSet<State> States { get; set; }
  public DbSet<Product> Products { get; set; }

  public DbSet<ProductCategory> ProductCategories { get; set; }

  public DbSet<ProductImage> ProductImages { get; set; }


  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.Entity<Category>().HasIndex(c => c.Name).IsUnique();
    modelBuilder.Entity<City>().HasIndex("Name", "StateId").IsUnique();
    modelBuilder.Entity<Country>().HasIndex(c => c.Name).IsUnique();
    modelBuilder.Entity<State>().HasIndex("Name", "CountryId").IsUnique();
    modelBuilder.Entity<Product>().HasIndex(c => c.Name).IsUnique();
    modelBuilder.Entity<ProductCategory>().HasIndex("ProductId", "CategoryId").IsUnique();

    DisableCascadingDelete(modelBuilder);

  }

  private static void DisableCascadingDelete(ModelBuilder modelBuilder)
  {
    var relationships = modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys());
    foreach (var relationship in relationships)
    {
      relationship.DeleteBehavior = DeleteBehavior.Restrict;
    }
  }
}
