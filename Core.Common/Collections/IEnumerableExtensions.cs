using Core.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
  public static class IEnumerableExtension
  {
    // preserves order
    public static IEnumerable<T> RemoveDuplicates<T>(this IEnumerable<T> source, Func<T, int> getHashCode = null, Func<T, T, bool> comparison = null)
    {
      if (getHashCode == null) getHashCode = t => t.GetHashCode();
      if (comparison == null) comparison = (a, b) => a.Equals(b);
      ISet<int> hashes = new HashSet<int>();

      foreach (var item in source)
      {
        if (hashes.Add(getHashCode(item))) yield return item;
      }


    }

    public static IEnumerable<T> TakeWhile<T>(this IEnumerable<T> source, Func<T, bool> predicate, bool inclusive)
    {
      foreach (T item in source)
      {
        if (predicate(item))
        {
          yield return item;
        }
        else
        {
          if (inclusive) yield return item;

          yield break;
        }
      }
    }
    public static bool IsNullOrEmpty(this IEnumerable enumerable)
    {
      return enumerable == null || enumerable.IsEmpty();
    }
    public static bool IsEmpty(this IEnumerable enumerable)
    {
      return enumerable.GetEnumerator().MoveNext();
    }

    public static IEnumerable<T> ObservableWhere<T>(this IEnumerable enumerable, Predicate<T> predicate)
    {
      FilteredEnumerable<T> filtered = new FilteredEnumerable<T>(enumerable, predicate);
      return filtered;
    }
    public static IEnumerable<TResult> ObservableSelect<TSource, TResult>(this IEnumerable<TSource> enumerable, Func<TSource, TResult> selector, Predicate<TSource> predicate = null)
    {
      var result = new EnumerableSelection<TResult, TSource>(enumerable, selector, predicate);
      return result;
    }
    // Creates a dictionary with multiple key entries per element
    public static IDictionary<TKey, TElement> ToMultiDictionary<TKey, TElement>(this IEnumerable<TElement> elements, params Func<TElement, TKey>[] selectors)
    {
      IDictionary<TKey, TElement> result = new Dictionary<TKey, TElement>();
      foreach (var element in elements)
      {
        foreach (var selector in selectors)
        {
          result[selector(element)] = element;
        }
      }
      return result;
    }


    public static IEnumerable<T> Do<T>(this IEnumerable<T> self, Action<T> action)
    {
      foreach (var it in self)
      {
        action(it);
      }
      return self;
    }
    /// <summary>
    /// returns true if enumerable's elements are all unique according to the compare function
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self"></param>
    /// <param name="compare"></param>
    /// <returns></returns>
    public static bool AreUnique<T>(this IEnumerable<T> self, Func<T, T, bool> compare)
    {
      return self.Distinct(compare).Count() == self.Count();
    }

    public static IEnumerable<T> Distinct<T>(this IEnumerable<T> self, Func<T, T, bool> compare)
    {
      var comparer = new DelegateEqualityComparer<T>(compare);
      return self.Distinct(comparer);
    }

    /// <summary>
    /// returns the source enumerable without the specified value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static IEnumerable<T> Except<T>(this IEnumerable<T> source, T value)
    {
      return source.Except(new[] { value });
    }
    /// <summary>
    /// returns true if the count of the enumerable is equal to count
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self"></param>
    /// <param name="cardinality"></param>
    /// <returns></returns>
    public static bool CountIs<T>(this IEnumerable<T> self, int count)
    {
      return self.Count() == count;
    }
    /// <summary>
    /// returns the number of uniquer elements in the enumerable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self"></param>
    /// <returns></returns>
    public static int Cardinality<T>(this IEnumerable<T> self)
    {
      return self.Distinct().Count();
    }



    /// <summary>
    /// checks to see if all elements of what are contained in the enumerable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self"></param>
    /// <param name="what"></param>
    /// <returns></returns>
    public static bool ContainsAll<T>(this IEnumerable<T> self, IEnumerable<T> what)
    {
      return self.Distinct().Intersect(what.Distinct()).Count() == what.Distinct().Count();
    }

    /// <summary>
    /// returns true if self contains any of the elements in what
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self"></param>
    /// <param name="what"></param>
    /// <returns></returns>
    public static bool ContainsAny<T>(this IEnumerable<T> self, IEnumerable<T> what)
    {
      return self.Distinct().Any(i => what.Contains(i));
    }

    /// <summary>
    /// returns true if any of the varargs are contained in the ienumerable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static bool ContainsAny<T>(this IEnumerable<T> self, params T[] args)
    {
      return self.ContainsAny((IEnumerable<T>)args);
    }


    /// <summary>
    /// checks to see if all params are contained in enumerable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static bool ContainsAll<T>(this IEnumerable<T> self, params T[] args)
    {
      return self.ContainsAll((IEnumerable<T>)args);
    }
    /// <summary>
    /// makes a size 1 array of T out of a single T element
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_this"></param>
    /// <returns></returns>
    public static IEnumerable<T> MakeArray<T>(this T _this, bool emptyIfNull)
    {
      if (_this == null && emptyIfNull)
      {
        return new T[0];
      }
      return new T[] { _this };
    }
    public static IEnumerable<T> MakeArray<T>(this T _this)
    {
      return new T[] { _this };
    }

    /// <summary>
    /// creates an enumarable out of this
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self"></param>
    /// <returns></returns>
    public static IEnumerable<T> MakeEnumerable<T>(this T self, bool emptyIfNull)
    {
      if (self == null && emptyIfNull) yield break;
      yield return self;
    }
    public static IEnumerable<T> MakeEnumerable<T>(this T self)
    {
      yield return self;
    }

    /// <summary> 
    /// alias for !.Any()  also synonomouse with IsEmpty
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_this"></param>
    /// <returns></returns>
    public static bool None<T>(this IEnumerable<T> _this)
    {
      return !_this.Any();
    }
    /// <summary>
    /// concats a single item to the end of the enumerable and returns the new enumerable
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_this"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public static IEnumerable<T> Concat<T>(this IEnumerable<T> _this, T item)
    {
      if (_this == null) return item.MakeArray();
      return _this.Concat(item.MakeArray());
    }

    
    /// <summary>
    /// returns the indeox of a in an enumerable (depending on the actual type of the neumerable the index may vary 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_this"></param>
    /// <param name="a"></param>
    /// <returns></returns>
    public static int IndexOf<T>(this IEnumerable<T> _this, T a)
    {
      var i = _this.IndexOfNoThrow(a);
      if (i < 0) throw new Exception("could not find element");
      return i;
    }

    /// <summary>
    /// returns -1 if element was not found
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="this"></param>
    /// <param name="a"></param>
    /// <returns></returns>
    public static int IndexOfNoThrow<T>(this IEnumerable<T> @this, T a)
    {
      int i = 0;
      foreach (var it in @this)
      {
        if (a.Equals(it)) return i;
        i++;
      }
      return -1;
    }
    /// <summary>
    /// returns truye if a comes before b in the sequence (based on their indices)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_this"></param>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool IsInPartialOrder<T>(this IEnumerable<T> _this, T a, T b)
    {
      var indexOfA = _this.IndexOf(a);
      var indexOfB = _this.IndexOf(b);
      return indexOfA < indexOfB;
    }


    public static bool IsSubsetOf<T>(this IEnumerable<T> _this, IEnumerable<T> other)
    {
      return other.IsSupersetOf(_this);
    }
    public static bool IsSupersetOf<T>(this IEnumerable<T> _this, IEnumerable<T> other)
    {
      return !other.Except(_this).Any();
    }
    public static bool IsProperSubsetOf<T>(this IEnumerable<T> _this, IEnumerable<T> other)
    {
      return _this.IsSubsetOf(other) && other.Count() != _this.Count();
    }
    public static bool IsProperSupersetOf<T>(this IEnumerable<T> _this, IEnumerable<T> other)
    {
      return _this.IsSupersetOf(other) && other.Count() != _this.Count();
    }
  }
}