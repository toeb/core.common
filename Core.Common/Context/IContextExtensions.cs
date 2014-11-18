using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace Core
{
  public static class IContextExtensions
  {
    public static IContext GetCurrentContext(this IContextService service)
    {
      return service.GetContext(null);
    }

    public static bool Has<T>(this IContext context)
    {
      var key = typeof(T).GetContextKey();
      return context.ContainsKey(key);
    }
    public static T Get<T>(this IContext context, string key)
    {
      if (!context.ContainsKey(key)) return default(T);
      var result = context.Get(key);
      if (!(result is T)) throw new InvalidCastException("the object stored in context is not of type " + typeof(T).Name);
      return (T)result;
    }
    public static string GetContextKey(this Type type)
    {
      var attribute = type.GetCustomAttribute<ContextKeyAttribute>();
      if (attribute == null) return type.FullName;
      return attribute.Key;
    }
    /// <summary>
    /// uses type as context key
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T Get<T>(this IContext context)
    {
      var key = typeof(T).GetContextKey();
      return context.Get<T>(key);
    }
    /// <summary>
    /// use full typename as context key
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    public static void Set<T>(this IContext context, T value)
    {
      var key = typeof(T).GetContextKey();
      context.Set(key, value);
    }
  }
}
