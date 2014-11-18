using Core.Merge;
using System;
using System.Threading.Tasks;

namespace Core.Values
{
  public static class ISourceExtensions
  {
    public static Lazy<T> ToLazy<T>(this ISource<T> source)
    {
      return new Lazy<T>(() => source.Value);
    }
    public static Lazy<object> ToLazy(this ISource source)
    {
      return new Lazy<object>(() => source.Value);
    }
  }
}
