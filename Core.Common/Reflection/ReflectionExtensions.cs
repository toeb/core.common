using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core.Graph;
using Core.Extensions;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
namespace Core.Extensions.Reflection
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
      return propertyInfo.GetValue(@object);
    }

    public static object ReflectPropertyValue(this object @object, string propertyName, bool ignoreCase = false)
    {

      if (@object == null) throw new ArgumentNullException("@object");
      var type = @object.GetType();

      var propertyInfo = type.GetProperty(propertyName);
      if (propertyInfo == null) return null;
      return propertyInfo.GetValue(@object);
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
      return propertyInfo.GetValue(@object);
    }
  }
}
namespace Core
{



  public static class ReflectionExtensions
  {

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
    /// returns the first common ancestor of a and b 
    /// does not take into account interfaces
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Type GetCommonAncestorWith(this Type a, Type b)
    {
      var result =
      a.FirstCommonAncestor(b, t => t.BaseType.MakeEnumerable(true));
      return result;
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


    public static IEnumerable<Type> GetGenericIEnumerablesArguments(this Type type)
    {
      return type
              .GetInterfaces().Concat(type)
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
  }
}
