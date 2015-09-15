using System;
using System.Collections.Generic;

namespace Core.Common
{
  public static class ExceptionHelpers
  {
    public static void AggregateThrowForeach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
      var exceptions = new List<Exception>();
      foreach (var item in enumerable)
      {
        try
        {
          action(item);
        }
        catch (Exception e)
        {
          exceptions.Add(e);
        }
      }
      if (exceptions.Count == 0) return;
      throw new AggregateException(string.Format("failed with {0} inner exceptions", exceptions.Count), exceptions);
    }
  }
}
