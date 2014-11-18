using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Extensions;
namespace Core
{

  public static class LinqExtensions
  {
    public static IEnumerable<TSource> Except<TSource>(this IEnumerable<TSource> first,
        IEnumerable<TSource> second, Func<TSource, TSource, bool> comparer)
    {
      return first.Except(second, new LambdaComparer<TSource>(comparer));
    }
  }
  
  // from http://brendan.enrick.com/post/linq-your-collections-with-iequalitycomparer-and-lambda-expressions.aspx
  /// <summary>
  /// Compares two T using a lambda function. usable in Comparator
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public class LambdaComparer<T> : IEqualityComparer<T>
  {
    private readonly Func<T, T, bool> _lambdaComparer;
    private readonly Func<T, int> _lambdaHash;

    public LambdaComparer(Func<T, T, bool> lambdaComparer) :
      this(lambdaComparer, o => 0)
    {
    }

    public LambdaComparer(Func<T, T, bool> lambdaComparer, Func<T, int> lambdaHash)
    {
      if (lambdaComparer == null)
        throw new ArgumentNullException("lambdaComparer");
      if (lambdaHash == null)
        throw new ArgumentNullException("lambdaHash");

      _lambdaComparer = lambdaComparer;
      _lambdaHash = lambdaHash;
    }

    public bool Equals(T x, T y)
    {
      return _lambdaComparer(x, y);
    }

    public int GetHashCode(T obj)
    {
      return _lambdaHash(obj);
    }
  }

}
