using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.ObjectModel;

namespace Core.Common.Reflect
{



  public static class Reflection
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="object"></param>
    /// <returns></returns>
    public static string ObjectHash(object @object)
    {
      string value = "null";
      if (@object != null)
      {
        StringBuilder builder = new StringBuilder();
        builder.Append(@object.GetType().FullName);
        @object.GetType();
        foreach (var i in Reflection.Bfs(@object))
        {
          ObjectValueHash(builder, i);
        }
        value = builder.ToString();
      }
      var hash = Crypto.Cryptography.GetMd5Hash(value);
      return hash;
    }
    public static string ObjectValueHash(object @object)
    {
      var sb = new StringBuilder();
      ObjectValueHash(sb, @object);
      return sb.ToString();
    }
    /// <summary>
    /// calculates the hash for all properties of the specified object
    /// </summary>
    /// <param name="sb"></param>
    /// <param name="object"></param>
    public static void ObjectValueHash(StringBuilder sb, object @object)
    {
      if (@object == null) return;
      var type = @object.GetType();
      foreach (var property in type.GetProperties())
      {
        if (property.PropertyType != typeof(string) && property.PropertyType.IsClass) continue;
        var value = property.GetValue(@object);
        sb.Append(property.Name);
        if (value != null) sb.Append(value.GetHashCode());


      }
    }
    public static IEnumerable<object> Bfs(object @object)
    {

      var queue = new Queue<object>();
      queue.Enqueue(@object);
      HashSet<object> visited = new HashSet<object>();
      while (queue.Count > 0)
      {
        var current = queue.Dequeue();
        if (current == null) continue;
        if (visited.Contains(current)) continue;
        visited.Add(current);
        yield return current;

        var successors = GetSuccessors(current).ToArray();
        foreach (var successor in successors)
        {
          queue.Enqueue(successor);
        }
      }
    }
    public static IEnumerable<object> GetSuccessors(object @object)
    {
      if (@object == null) yield break;
      
      var type = @object.GetType();
      if (type == typeof(string)) yield break;
      if (@object is IEnumerable && type.GetGenericEnumerableElementType().IsClass && type.GetGenericEnumerableElementType() !=typeof(string))
      {
        foreach (var child in @object as IEnumerable)
        {
          yield return child;
        }
        yield break;
      }      
      var properties = type.GetProperties();
      
      foreach (var property in properties)
      {
        if (!property.CanRead) continue;
        if (!property.CanWrite) continue;
        if (!property.GetGetMethod().IsPublic) continue;
        if (property.GetIndexParameters().Count() > 0) continue;
        var value = property.GetValue(@object);


        if (property.PropertyType == typeof(string))
        {
          continue;
        }
        else if (property.PropertyType.IsEnumerable())
        {
          foreach (var element in GetSuccessors(value))
          {
            yield return element;
          }
        }
        else
        {
          if (!property.PropertyType.IsClass) continue;
          yield return value;
        }

      }
    }


    public static IEnumerable<T> GetCustomAttributes<T>(this ICustomAttributeProvider attributeProvider, bool inherit = true) where T : System.Attribute
    {
      return attributeProvider.GetCustomAttributes(inherit).Where(atr => atr is T).Cast<T>().ToArray();
    }
    public static Type GetNullableTypeArgument(this Type type)
    {
      if (!type.IsNullable()) return null;
      return type.GetGenericArguments().First();
    }

    public static bool IsNullable(this Type type)
    {
      if (!type.IsGenericType) return false;
      var definition = type.GetGenericTypeDefinition();
      if (definition.FullName != "System.Nullable`1") return false;
      return true;
    }
    public static MemberInfo[] GetEnumMembers(this Type enumType)
    {
      var values = enumType.GetEnumNames();
      var enumMembers = values.Select(enumValue => enumType.GetMember(enumValue).Single()).ToArray();
      return enumMembers;
    }

    public static int GetIntEnumValue(this Type enumType, string name)
    {
      return (int)Enum.Parse(enumType, name);
    }

    public static object GetDefaultValue(this Type type)
    {
      if (type.IsValueType)
      {
        return Activator.CreateInstance(type);
      }
      return null;
    }
    /// <summary>
    /// returns the attribute targets with the specified attribute
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="attributeTargets"></param>
    /// <param name="attributeType"></param>
    /// <param name="inherit"></param>
    /// <returns></returns>
    public static IEnumerable<T> With<T>(this IEnumerable<T> attributeTargets, Type attributeType, bool inherit = true) where T : ICustomAttributeProvider
    {
      return attributeTargets.Where(target => target.HasAttribute(attributeType, inherit));
    }
    /// <summary>
    /// sets all values for the properties which have the specified attribute.  Fails quietly if type does not match or proeprty cannot write
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    /// <param name="object"></param>
    /// <param name="value"></param>
    /// <param name="onlyPublic"></param>
    /// <param name="inherit"></param>
    public static void SetAttributedPropertyValues<TAttribute>(this object @object, object value, bool onlyPublic = true, bool inherit = true) where TAttribute : Attribute
    {
      if (@object == null) return;
      var type = @object.GetType();
      var flags = BindingFlags.Public | BindingFlags.Instance;
      if (!onlyPublic) flags |= BindingFlags.NonPublic;
      var targets = type.GetProperties(flags).With(typeof(TAttribute), inherit);
      foreach (var target in targets)
      {
        if (!target.CanWrite) continue;
        if (!target.PropertyType.IsAssignableFromValue(value)) continue;
        target.SetValue(@object, value);
      }

    }


    /// <summary>
    /// returns the Guid of the assembly (defined in assembly properties)
    /// </summary>
    /// <param name="assembly"></param>
    /// <returns></returns>
    public static Guid GetGuid(this Assembly assembly)
    {
      string id = "";
      foreach (object attr in assembly.GetCustomAttributes(true))
      {
        if (attr is GuidAttribute)
          id = ((GuidAttribute)attr).Value;
      }
      var guid = Guid.Parse(id);
      return guid;
    }


    /// <summary>
    /// returns tuples of PropertyInfos and Attributes of type TAttribute
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    /// <param name="type"></param>
    /// <param name="inherit"></param>
    /// <returns></returns>
    public static IEnumerable<Tuple<PropertyInfo, TAttribute>> PropertiesWith<TAttribute>(this Type type, bool inherit = true) where TAttribute : System.Attribute
    {
      foreach (var prop in type.PropertiesWith(typeof(TAttribute), inherit))
      {
        yield return Tuple.Create(prop, prop.GetCustomAttribute<TAttribute>());
      }
      yield break;
    }

    public static T GetCustomAttribute<T>(this ICustomAttributeProvider provider, bool inherit = true) where T : System.Attribute
    {
      return provider.GetCustomAttributes<T>().FirstOrDefault();
    }


    public static IEnumerable<Tuple<MethodInfo, TAttribute>> MethodsWith<TAttribute>(this Type type, bool inherit = true) where TAttribute : System.Attribute
    {
      return type.MethodsWith(typeof(TAttribute), inherit).Select(method => Tuple.Create(method, method.GetCustomAttribute<TAttribute>()));
    }

    public static IEnumerable<MethodInfo> MethodsWith(this Type type, Type attributeType, bool inherit = true)
    {
      foreach (var method in type.GetMethods())
      {
        var attribute = method.GetCustomAttribute(attributeType, inherit);
        if (attribute == null) continue;
        yield return method;
      }
    }


    public static MethodInfo FirstMethodWith<T>(this Type type, bool inherit = true) where T : System.Attribute
    {
      return type.FirstMethodWith(typeof(T), inherit);
    }
    public static MethodInfo FirstMethodWith(this Type type, Type attributeType, bool inherit = true)
    {
      return type.MethodsWith(attributeType, inherit).FirstOrDefault();
    }

    public static PropertyInfo FirstPropertyWith<T>(this Type type, bool inherit = true) where T : System.Attribute
    {
      return type.FirstPropertyWith(typeof(T), inherit);
    }
    public static PropertyInfo FirstPropertyWith(this Type type, Type attributeType, bool inherit = true)
    {
      return PropertiesWith(type, attributeType, inherit).FirstOrDefault();
    }

    /// <summary>
    /// returns all property infos for a type which have an attribute of type "attributeType"
    /// </summary>
    /// <param name="type"></param>
    /// <param name="attributeType"></param>
    /// <param name="inherit"></param>
    /// <returns></returns>
    public static IEnumerable<PropertyInfo> PropertiesWith(this Type type, Type attributeType, bool inherit = true)
    {
      foreach (var property in type.GetProperties())
      {
        var attribute = property.GetCustomAttribute(attributeType, inherit);
        if (attribute == null) continue;
        yield return property;
      }
      yield break;
    }

    public static object GetCustomAttribute(this ICustomAttributeProvider provider, Type type, bool inherit)
    {
      return provider.GetCustomAttributes(type, inherit).FirstOrDefault();
    }

    /// <summary>
    /// returns the memberinfo of the member that expression evaluates to
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="model"></param>
    /// <param name="expression"></param>
    /// <returns></returns>
    public static MemberInfo MemberInfoFor<TModel, TValue>(this TModel model, Expression<Func<TModel, TValue>> expression)
    {
      return expression.MemberInfoFor();
    }
    public static MemberInfo MemberInfoFor<TModel, TValue>(this Expression<Func<TModel, TValue>> expression)
    {
      dynamic body = expression.Body;
      dynamic member = body.Member;
      if (!(member is MemberInfo)) return null;
      var memberinfo = member as MemberInfo;
      return memberinfo;

    }
    public static PropertyInfo PropertyInfoFor<TModel, TValue>(this TModel model, Expression<Func<TModel, TValue>> expression)
    {
      return model.MemberInfoFor(expression) as PropertyInfo;
    }
    public static PropertyInfo PropertyInfoFor<TModel, TValue>(this Expression<Func<TModel, TValue>> expression)
    {
      return expression.MemberInfoFor() as PropertyInfo;
    }

    public static IEnumerable<Type> GetParentTypes(this Type currentType)
    {
      List<Type> ret = new List<Type>();

      do
      {
        ret.Add(currentType);
        ret.AddRange(currentType.GetInterfaces());
        currentType = currentType.BaseType;
      } while (currentType != null);
      return ret;
    }


    public static Type GetByQuery(this IEnumerable<Type> self, string typequery)
    {
      var type = self.GetByAssemblyQualifiedName(typequery);
      if (type != null) return type;
      type = self.GetByFullName(typequery, true);
      if (type != null) return type;

      Guid guid;
      if (Guid.TryParse(typequery, out guid))
      {
        type = self.GetById(guid);
        return type;
      }

      var query = self.GetByName(typequery, true);
      if (query.Count() > 1) return null;

      type = self.GetByName(typequery, true).SingleOrDefault();

      return type;
    }
    public static IEnumerable<Type> GetByName(this IEnumerable<Type> self, string name, bool ignoreCase)
    {
      name = ignoreCase ? name.ToLower() : name;
      return self.Where(type => (ignoreCase ? type.FullName.ToLower() : type.FullName).EndsWith(name));
    }
    public static Type GetByFullName(this IEnumerable<Type> self, string fullTypeName, bool ignoreCase = false)
    {
      fullTypeName = ignoreCase ? fullTypeName.ToLower() : fullTypeName;
      return self.SingleOrDefault(t => (ignoreCase ? t.FullName.ToLower() : t.FullName) == fullTypeName);
    }
    public static Type GetByAssemblyQualifiedName(this IEnumerable<Type> self, string fullTypeName)
    {
      return self.SingleOrDefault(t => t.AssemblyQualifiedName == fullTypeName);
    }
    public static Type GetById(this IEnumerable<Type> self, Guid id)
    {
      return self.SingleOrDefault(t => t.GUID == id);
    }
    public static IQueryable<Type> NotAbstract(this IQueryable<Type> self)
    {
      return self.Where(it => !it.IsAbstract);
    }
    public static IQueryable<Type> FindTypesImplementing<TType>(this IQueryable<Type> self)
    {
      return self.FindTypesImplementing(typeof(TType));
    }
    public static IQueryable<Type> FindTypesImplementing(this IQueryable<Type> self, Type superclass)
    {
      return self.Where(t => superclass.IsAssignableFrom(t));
    }
    public static bool HasAttribute(this ICustomAttributeProvider self, Type attributeType, bool inherit = true)
    {
      return self.GetCustomAttributes(attributeType, true).Any();
    }
    public static bool HasAttribute<T>(this ICustomAttributeProvider self, bool inherit = true) where T : System.Attribute
    {
      return self.HasAttribute(typeof(T), inherit);
    }
    public static bool ImplementsInterface(this Type self, Type interfaceType)
    {
      if (!interfaceType.IsInterface) throw new ArgumentException("type is not an interface", "interfaceType");
      return self.GetInterface(interfaceType.FullName) != null;
    }
    public static bool ImplementsInterface<T>(this Type self)
    {
      var interfaceType = typeof(T);
      return self.ImplementsInterface(interfaceType);
    }
    public static bool IsEnumerable(this Type self)
    {
      return self.GetEnumerableInterface() != null || self.IsGenericEnumerable();
    }
    public static bool IsGenericEnumerable(this Type self)
    {
      return self.GetGenericEnumerableInterface() != null;
    }
    public static Type GetEnumerableInterface(this Type type)
    {
      return type.GetInterface("IEnumerable");
    }
    public static Type GetGenericEnumerableInterface(this Type type)
    {
      return type.GetGenericEnumerableInterfaces().FirstOrDefault();
    }
    public static IEnumerable<Type> GetGenericEnumerableInterfaces(this Type type)
    {
      var enumerableTypes = type.GetInterfaces()
              .Where(t => t.IsGenericType == true
                  && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));
      return enumerableTypes;
    }
    public static Type GetGenericEnumerableElementType(this Type type)
    {
      return type.GetGenericIEnumerablesArguments().FirstOrDefault();
    }

    public static bool IsGenericCollection(this Type type)
    {
      return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ICollection<>);
    }

    public static Type GetCollectionItemType(this Type type)
    {
      if (!type.IsGenericCollection()) return null;
      return type.GetGenericArguments().Single();
    }
    public static ICollection CreateObservableCollection(Type itemType)
    {
      var genericType = typeof(ObservableCollection<>).MakeGenericType(itemType);
      var instance = System.Activator.CreateInstance(genericType);
      return instance as ICollection;
    }



    public static IEnumerable<Type> GetGenericIEnumerablesArguments(this Type type)
    {
      return type
              .GetInterfaces().Concat(new[] { type })
              .Where(t => t.IsGenericType == true
                  && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
              .Select(t => t.GetGenericArguments()[0]);
    }

    /// <summary>
    /// returns true if the type is assignable from the passed value
    /// this includes the special case if value is null, which is only assignable to reference types.
    /// else it just gets the type of the value and checks wether the  type is assignable from it
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsAssignableFromValue(this Type type, object value)
    {
      if (value == null && !type.IsValueType) return true;
      if (value == null) return false;
      return type.IsAssignableFrom(value.GetType());
    }
    public static bool IsExactlyAssignableFromValue(this Type type, object value)
    {
      if (value == null && !type.IsValueType) return true;
      return value.GetType() == type;
    }
    public static Expression<Func<TModel, TValue>> Expression<TModel, TValue>(this TModel model, Expression<Func<TModel, TValue>> expression)
    {
      return expression;
    }

    public static string GetDisplayName(this Type type)
    {
      return GetDisplayName((ICustomAttributeProvider)type, type.Name);
    }
    public static string GetDisplayName(this ICustomAttributeProvider provider, string defaultName = null)
    {
      var displayNameAttribute = provider.GetCustomAttribute<DisplayNameAttribute>(true);
      if (displayNameAttribute != null && displayNameAttribute.DisplayName != null) return displayNameAttribute.DisplayName;
      var displayAttribute = provider.GetCustomAttribute<DisplayAttribute>(true);
      if (displayAttribute != null && displayAttribute.Name != null) return displayAttribute.Name;

      return defaultName;

    }
  }
}
