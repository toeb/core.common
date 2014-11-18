using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core
{

  public static class DelegateExtensions
  {

    /// <summary>
    /// tries action as long as it does not return false.
    /// will stop after nMax tries
    /// </summary>
    /// <param name="action"></param>
    /// <param name="nMax"></param>
    /// <returns></returns>
    public static bool TryRepeat(this Func<bool> action, int nMax = 15)
    {
      int i = 0;
      while (true)
      {
        if (action()) return true;
        Thread.Sleep((i + 1) * (i + 1) * 10);
        if (i > nMax) return false;
        i++;
      }
    }

    public static T TryRepeatException<T>(this Func<T> action, int retries = 15)
    {
      T result = default(T);
      Action a = () => result = action();
      a.TryRepeatException(retries);
      return result;
    }
    /// <summary>
    /// tries action nMax times. action fails if it throws an exception
    /// </summary>
    /// <param name="action"></param>
    /// <param name="nMax"></param>
    public static void TryRepeatException(this Action action, int nMax = 15)
    {
      Exception exception = null;
      if (TryRepeat(() =>
      {
        try
        {
          action();

          return true;
        }
        catch (Exception e)
        {
          exception = e;
          return false;
        }

      }, nMax)) return;
      throw exception;
    }
  }
}