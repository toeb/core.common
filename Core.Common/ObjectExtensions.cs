using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
  public static class ObjectExtensions
  {
    /// <summary>
    /// makes a nullable from any valuetype
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="valueType"></param>
    /// <returns></returns>
    public static Nullable<T> MakeNullable<T>(T valueType) where T : struct
    {
      return new Nullable<T>(valueType);
    }
    /// <summary>
    /// Makes a lazy from  any value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Lazy<T> MakeLazy<T>(this T value)
    {
      return new Lazy<T>(() => value);
    }

    /// <summary>
    /// executes the action a if o is of type T 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="o"></param>
    /// <param name="a"></param>
    /// <returns></returns>
    public static bool IfIsOfType<T>(this object o, Action<T> a)
    {
      if (o is T)
      {
        a((T)o);
        return true;
      }
      return false;
    }
  }
}
