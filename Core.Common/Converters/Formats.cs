using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Converters
{
  public static class Formats
  {
    public static IFormat Derive(this IFormat format, Type type)
    {
      return Create(type, format);
    }
    public static IFormat Default<T>()
    {
      return Default(typeof(T));
    }
    public static IFormat Default(Type type)
    {
      return Create(type, "Default");
    }
    public static IFormat Create(Type type, string name)
    {
      return new Format(type,name);
    }
    public static IFormat Create(Type type, IFormat baseFormat)
    {
      return new Format(type, baseFormat.Name);
    }
    public static IFormat Create<T>(string name)
    {
      return Create(typeof(T), name);
    }

  }
}
