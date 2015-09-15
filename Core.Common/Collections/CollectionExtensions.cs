using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Common.Collections
{
  public static class CollectionExtensions
  {
    public static void SetCollection<T, T2>(this ICollection<T> collection, IEnumerable<T2> items) where T2 : T
    {
      collection.Clear();
      foreach (var item in items) collection.Add(item);
    }
  
    public static IEnumerable<T> Concat<T>(this IEnumerable<T> enumerable, T item)
    {
      return enumerable.Concat(item.ToEnumerable());
    }
    public static IEnumerable<T> ToEnumerable<T>(this T item)
    {
      yield return item;
    }
  
  }
}
