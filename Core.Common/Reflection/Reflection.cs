using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
  public static class Reflection
  {
    private static object[] emptyArray = new object[0];

    /// <summary>
    /// Returns the Calling Properties Name.  (you can increase to get the proerty name a up stackframes below)
    /// 
    /// </summary>
    /// <param name="up"></param>
    /// <returns></returns>
    public static string GetCallingPropertyName(int up = 1)
    {
      StackFrame frame = new StackFrame(up + 1);
      var name = frame.GetMethod().Name.Substring(4);
      return name;
    }
    public static IEnumerable<Type> GetSuperTypes(Type from, Type to)
    {
      var ret = new List<Type>();
      var current = from;
      ret.Add(from);
      while (current != to)
      {
        if (current.BaseType == null)
        {
          return new List<Type>();
        }
        current = current.BaseType;
        ret.Add(current);
      }
      return ret;

    }


    public static Type[] GetSubclassesOf(this IEnumerable<Type> types, Type superType)
    {
      var result = from Type t
                  in types
                   where GetSuperTypes(t, superType).Any(t2 => t2 == superType)
                   select t;
      return result.ToArray();
    }
    public static IList<Type> GetClassesImplementing(this IEnumerable<Type> types, string interfaceName)
    {
      var query = from Type t in types where t.GetInterfaces().Any(i => i.Name.EndsWith(interfaceName)) select t;
      return query.ToList();
    }
    public static IEnumerable<Type> GetParentTypes(Type typeToDisplay)
    {
      var currentType = typeToDisplay;
      List<Type> ret = new List<Type>();
      ret.Add(currentType);
      while (currentType.BaseType != null)
      {
        ret.AddRange(currentType.GetInterfaces());
        ret.Add(currentType.BaseType);
        currentType = currentType.BaseType;
      }
      ret.Reverse();
      return ret;
    }

    public static T GetStaticPropertyValue<T>(Type t, string propertyName)
    {
      try
      {
        var propInfo = t.GetProperty(propertyName);
        var methodInfo = propInfo.GetGetMethod();
        if (!methodInfo.IsStatic) throw new Exception("Method was not static");
        object result = methodInfo.Invoke(null, new object[0]);
        return (T)result;
      }
      catch (Exception e)
      {
        return default(T);
      }
    }
    public static object CallStaticMethod(Type t, string methodName)
    {
      try
      {
        object result = null;
        var info = GetStaticMethod(t, methodName);

        result = info.Invoke(null, emptyArray);
        return result;
      }
      catch (Exception e)
      {

        return null;
      }
    }
    public static MethodInfo GetStaticMethod(Type type, string methodName)
    {
      try
      {
        MethodInfo result = null;
        result = type.GetMethod(methodName);
        if (!result.IsStatic) return null;
        return result;

      }
      catch (Exception e)
      {
        return null;
      }
    }



    public static void DeepCopy(Type type, object source, object destination)
    {
      if (!destination.GetType().IsAssignableFrom(type)) throw new ArgumentException("source and target type must be compatible");

      DeepCopyRecursive(type, new Dictionary<object, object>() { { source, destination } }, source, destination);

    }
    private static void DeepCopyRecursive(Type type, Dictionary<object, object> seen, object source, object destination)
    {


      foreach (var property in type.GetProperties())
      {
        if (!property.CanRead) continue;
        if (!property.CanWrite) continue;
        var propertyType = property.PropertyType;
        if (propertyType.IsValueType || typeof(string) == propertyType)
        {
          property.SetValue(destination, property.GetValue(source));
          continue;
        }
        var value = property.GetValue(source);

        if (value == null)
        {
          property.SetValue(destination, null);
          continue;
        }

        if (seen.ContainsKey(value))
        {
          property.SetValue(destination, seen[value]);
          continue;
        }

        var newInstance = System.Activator.CreateInstance(propertyType);
        property.SetValue(destination, newInstance);

        seen[value] = newInstance;

        DeepCopyRecursive(propertyType, seen, value, newInstance);
      }



    }
    public static void DeepCopy<T, TDestination>(T source, TDestination destination) where TDestination : T
    {
      DeepCopy(typeof(T), source, destination);
    }
    public static void DeepCopy(object source, object destination)
    {
      if (source == null) throw new ArgumentNullException();
      if (destination == null) throw new ArgumentNullException();
      var type = source.GetType();

      DeepCopy(type, source, destination);
    }

    public static IList CreateGenericCollection(Type elementType)
    {
      var genericCollectionTypeDefinition =  typeof(List<>);
      var collectionType = genericCollectionTypeDefinition.MakeGenericType(elementType);
      if (!typeof(ICollection).IsAssignableFrom(collectionType)) return null;
      var collection = System.Activator.CreateInstance(collectionType) as IList;
      return collection;
    }
  }
}
