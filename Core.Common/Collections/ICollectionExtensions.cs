using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Collections
{

  public static class CollectionExtensions
  {
    /// <summary>
    /// adds all items to collection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self"></param>
    /// <param name="items"></param>
    public static void AddRange<T>(this ICollection<T> self, IEnumerable<T> items)
    {
      var copy = items.ToArray();
      foreach (var item in items)
      {
        self.Add(item);
      }

    }
    /// <summary>
    /// adds the varaargs to the collection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self"></param>
    /// <param name="items"></param>
    public static void AddRange<T>(this ICollection<T> self, params T[] items)
    {
      self.AddRange(items.AsEnumerable());
    }

    public static IEnumerable<T> View<T>(this ICollection<T> _this, Func<T, bool> predicate)
    {
      return null;
    }

    public static ICollectionView<T> EditableView<T>(this ICollection<T> _this, Func<T, bool> predicate)
    {
      return new ObservableCollectionView<T>(_this, predicate);
    }
    public static IEnumerable<TOut> Transform<TIn, TOut>(this IEnumerable<TIn> _this, Func<TIn, TOut> convert)
    {
      return null;
    }
    public static ICollection<TOut> EditableTransform<TIn, TOut>(this ICollection<TIn> _this, Func<TIn, TOut> convertTo, Func<TOut, TIn> convertFrom)
    {
      return new CollectionTransform<TOut, TIn>(_this, convertTo, convertFrom);
    }
  }
}
