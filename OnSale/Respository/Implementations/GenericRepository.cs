using System;
using Microsoft.EntityFrameworkCore;
using OnSale.Common;
using OnSale.Data;

namespace OnSale.Respository.Implementations;

public class GenericRepository<T>(DataContext context) : IGenericRepository<T> where T : class
{
  private readonly DataContext _context = context;
  private readonly DbSet<T> _entity = context.Set<T>();
  public virtual async Task<Response<T>> AddAsync(T entity)
  {
    _context.Add(entity);
    try
    {
      await _context.SaveChangesAsync();
      return new Response<T>
      {
        IsSuccess = true,
        Result = entity
      };
    }
    catch (DbUpdateException)
    {
      return DbUpdateExceptionActionResponse();
    }
    catch (Exception exception)
    {
      return ExceptionActionResponse(exception);
    }

  }
  public virtual async Task<Response<T>> DeleteAsync(int? id)
  {
    var row = await _entity.FindAsync(id);
    if (row == null)
    {
      return new Response<T>
      {
        IsSuccess = false,
        Message = "Resource not found"
      };
    }

    _entity.Remove(row);
    try
    {
      await _context.SaveChangesAsync();
      return new Response<T>
      {
        IsSuccess = true,
      };
    }
    catch
    {
      return new Response<T>
      {
        IsSuccess = false,
        Message = "Cannot be deleted because it has related records"
      };
    }
  }
  public virtual async Task<Response<T>> GetAsync(int id)
  {
    var row = await _entity.FindAsync(id);
    if (row != null)
    {
      return new Response<T>
      {
        IsSuccess = true,
        Result = row
      };
    }
    return new Response<T>
    {
      IsSuccess = false,
      Message = "Resource not found"
    };
  }
  public virtual async Task<Response<IEnumerable<T>>> GetAsync()
  {
    return new Response<IEnumerable<T>>
    {
      IsSuccess = true,
      Result = await _entity.ToListAsync()
    };
  }
  public virtual async Task<Response<T>> UpdateAsync(T entity)
  {
    try
    {
      _context.Update(entity);
      await _context.SaveChangesAsync();
      return new Response<T>
      {
        IsSuccess = true,
        Result = entity
      };
    }
    catch (DbUpdateException)
    {
      return DbUpdateExceptionActionResponse();
    }
    catch (Exception exception)
    {
      return ExceptionActionResponse(exception);
    }
  }
  private Response<T> ExceptionActionResponse(Exception exception)
  {
    return new Response<T>
    {
      IsSuccess = false,
      Message = exception.Message
    };
  }
  private Response<T> DbUpdateExceptionActionResponse()
  {
    return new Response<T>
    {
      IsSuccess = false,
      Message = "The record you are trying to create already exists."
    };
  }
}
