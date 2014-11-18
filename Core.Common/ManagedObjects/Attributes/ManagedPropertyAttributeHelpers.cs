using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Core.ManagedObjects
{
  public static class ManagedPropertyAttributeHelpers
  {
    /// <summary>
    /// returns the manageable properties for a type
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static IEnumerable<PropertyInfo> GetManageableProperties(this Type type)
    {
      var properties = type.GetProperties();
      if (type.HasAttribute<ManagedObjectAttribute>()) return properties.Where(p => !p.HasAttribute<UnmanagedAttribute>());
      return properties.Where(p => p.HasAttribute<ManagedAttribute>());
    }
  }
}
