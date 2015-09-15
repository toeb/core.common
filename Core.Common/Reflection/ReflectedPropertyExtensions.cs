using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Core.Common.Reflect
{
  public static class ReflectedPropertyExtensions
  {

    public static bool IsSuperclassOf(this Type type, Type other)
    {
      return other.IsSubclassOf(type);
    }
    public static bool IsSubclassOfOrSameClass(this Type type, Type other)
    {
      return other == type || type.IsSubclassOf(other);
    }
    public static bool IsSuperclassOfOrSameClass(this Type type, Type other)
    {
      return other.IsSubclassOfOrSameClass(type);
    }
    public static bool HasExplicitConversionTo(this Type type, Type other)
    {

      var methods = type.GetMethods(System.Reflection.BindingFlags.Static | BindingFlags.Public).Concat(other.GetMethods(System.Reflection.BindingFlags.Static | BindingFlags.Public));
      var conversionMethods = methods.Where(mi => mi.Name == "op_Explicit" && mi.GetParameters().Count() == 1 && mi.GetParameters()[0].ParameterType == type && mi.ReturnType == other);
      return conversionMethods.Any();
    }
    public static bool HasImplicitConversionTo(this Type type, Type other)
    {

      var methods = type.GetMethods(System.Reflection.BindingFlags.Static | BindingFlags.Public).Concat(other.GetMethods(System.Reflection.BindingFlags.Static | BindingFlags.Public));
      var conversionMethods = methods.Where(mi => mi.Name == "op_Implicit" && mi.GetParameters().Count() == 1 && mi.GetParameters()[0].ParameterType == type && mi.ReturnType == other);
      return conversionMethods.Any();
    }


    /// <summary>
    /// shorthand for getting property from object
    /// </summary>
    /// <param name="object"></param>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    public static object GetProperty(this object @object, string propertyName)
    {
      if (@object == null) throw new ArgumentNullException("@object");
      var type = @object.GetType();
      var propertyInfo = type.GetProperty(propertyName);
      if (propertyInfo == null) return null;
      return propertyInfo.GetValue(@object, empty);
    }
    public static void SetValue(this PropertyInfo property, object @object, object value)
    {
       property.SetValue(@object, value,empty);
    }
    public static object GetValue(this PropertyInfo property, object @object)
    {
      return property.GetValue(@object, empty);
    }

    public static object ReflectPropertyValue(this object @object, string propertyName, bool ignoreCase = false)
    {

      if (@object == null) throw new ArgumentNullException("@object");
      var type = @object.GetType();

      var propertyInfo = type.GetProperty(propertyName);
      if (propertyInfo == null) return null;
      return propertyInfo.GetValue(@object, empty);
    }
    public static object ReflectPropertyValueByIndex(this object @object, int index)
    {
      if (@object == null) throw new ArgumentNullException("@object");
      var type = @object.GetType();
      var properties = type.GetProperties();
      if (index < 0) throw new ArgumentException("index may not be less than 0");
      if (index >= properties.Length) throw new ArgumentOutOfRangeException("index", index, "index may not be larger than the number of properties");
      var propertyInfo = properties[index];
      if (propertyInfo == null) return null;
      return propertyInfo.GetValue(@object, empty);
    }
    public static object[] empty = new object[0];
  }
}
