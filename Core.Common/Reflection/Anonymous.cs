using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
  public static class Anonymous
  {

    public static T Cast<T>(T blueprint, object other)
    {
      var result = Default<T>(blueprint);
      TryAssign(ref result, other);
      return result;
    }

    /// <summary>
    /// returns a default value  for (anonymous type ) T using the passed argument to infer the type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="blueprint"></param>
    /// <returns></returns>
    public static T Default<T>(T blueprint)
    {
      return default(T);
    }

    /// <summary>
    /// asssigns the possibly anonymous target the value of the untyped object
    /// returns fals if other is not of anonymous type T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="target"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static bool TryAssign<T>(ref T target, object other)
    {
      if (!(other is T)) return false;
      target = (T)other;
      return true;
    }

    /// <summary>
    /// returns true if type is anonymous
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsAnonymousType(this Type type)
    {
      bool hasCompilerGeneratedAttribute = type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Count() > 0;
      bool nameContainsAnonymousType = type.FullName.Contains("AnonymousType");
      bool isAnonymousType = hasCompilerGeneratedAttribute && nameContainsAnonymousType;

      return isAnonymousType;
    }

    /// <summary>
    /// returns a dictionary containsing the propertyies of the anonymous object
    /// </summary>
    /// <param name="anonymousObject"></param>
    /// <returns></returns>
    public static IDictionary<string, object> ToDictionary(object anonymousObject, bool catchErrors = false)
    {
      var result = new Dictionary<string, object>();
      if (anonymousObject == null) return result;
      var type = anonymousObject.GetType();
      //if (!type.IsAnonymousType()) throw new ArgumentException("anonymousObject must be an anonymous type");

      foreach (var property in type.GetProperties())
      {
        if (catchErrors)
        {
          try
          {
            result[property.Name] = property.GetValue(anonymousObject, null);
          }
          catch (Exception e) { }
        }
        else
        {
          result[property.Name] = property.GetValue(anonymousObject, null);
        }
      }
      return result;
    }



  }
}
