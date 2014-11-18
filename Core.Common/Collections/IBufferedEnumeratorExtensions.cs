using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Collections
{
  public static class IBufferedEnumeratorExtensions
  {
    public static IBufferedEnumerator<T> Buffered<T>(this IEnumerator<T> inner, int bufferSize = 128)
    {
      return new BufferedEnumerator<T>(inner, bufferSize);
    }
    /// <summary>
    /// buffers the specified ienumerator including the current element
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inner"></param>
    /// <param name="bufferSize"></param>
    /// <returns></returns>
    public static IBufferedEnumerator<T> BufferedCurrentInclusive<T>(this IEnumerator<T> inner, int bufferSize = 128)
    {
      return new BufferedEnumerator<T>(inner, true, bufferSize);
    }
  }
}
