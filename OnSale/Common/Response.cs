using System;

namespace OnSale.Common;

public class Response<T>
{
  public bool IsSuccess { get; set; }

  public string Message { get; set; } = null!;

  public object Result { get; set; } = null!;
}

