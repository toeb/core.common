
using Core.Repositories;


using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

using System.Reflection;


namespace Core.TypeServices
{
  public class TypeService : IIdDataSource<Guid, ObjectType>, ITypeService
  {
    private static TypeService instance;
    public static TypeService Instance { get { return instance ?? (instance = new TypeService() { Reflection = ReflectionService.Instance }); } }

    public ObjectType RequireType<T>()
    {
      var type = typeof(T);
      return RequireType(type);
    }
    public TypeService()
    {
      TypeMap = new Dictionary<Guid, ObjectType>();
    }
    IDictionary<Guid, ObjectType> TypeMap { get; set; }

    void SetObjectType(Type type, ObjectType objectType)
    {
      TypeMap[type.GUID] = objectType;
    }

    public bool HasType(Guid id)
    {
      return TypeMap.ContainsKey(id);
    }
    public ObjectType GetType(Guid id)
    {
      if (!HasType(id)) return null;
      return TypeMap[id];
    }
    public IEnumerable<PropertyInfo> GetPropertiesFor(Type type)
    {
      return type.GetProperties();
    }



    public ObjectType RequireType(Type type)
    {
      if (type == null) return null;
      if (HasType(type.GUID)) return GetType(type.GUID);
      ObjectType result = new ObjectType();
      result.Id = type.GUID;
      result.FullName = type.FullName;
      result.Name = type.Name;
      result.IsSystemType = true;
      result.SetSystemType(type);
      result.AssemblyQualifiedName = type.AssemblyQualifiedName;
      result.ClassName = type.Name;
      if (type.Name.Contains('`')) result.ClassName = type.Name.Substring(0, type.Name.IndexOf("`"));
      if (type.Name.Contains('`')) result.FullClassName = type.FullName.Substring(0, type.FullName.IndexOf("`"));
      TypeMap[type.GUID] = result;

      result.Namespace = type.Namespace;

      var classAttributes = type.GetCustomAttributes();
      result.AllAttributes = classAttributes.Select(attr => new Attribute(RequireType(attr.GetType()), attr)).ToArray();
      foreach (var attr in result.AllAttributes)
      {
        result.Attributes[attr.Name] = attr;
      }


      if (type.IsEnum)
      {
        result.Enum = new Dictionary<string, object>();
        foreach (var name in Enum.GetNames(type))
        {

          result.Enum[name] = new Dictionary<string, object>{
            {"Value", (int)Enum.Parse(type, name) }
          };
        }
      }

      result.BaseType = RequireType(type.BaseType);
      if (type.IsGenericType)
      {
        result.IsGeneric = true;
        result.GenericType = RequireType(type.GetGenericTypeDefinition());
        result.GenericTypeArguments = type.GenericTypeArguments.Select(t => RequireType(t));
        //result.GenericParameters = type.GetGenericParameterConstraints().Select(t => RequireType(t)).ToArray();
      }
      result.Interfaces = type.GetInterfaces().Select(t =>
      {
        return RequireType(t);
      }).ToArray();


      result.Properties = GetPropertiesFor(type).Select(pi =>
      {
        var property = new ObjectProperty();
        property.PropertyName = pi.Name;
        property.DeclaringType = result;
        property.PropertyType = RequireType(pi.PropertyType);
        property.CanWrite = pi.CanWrite;
        property.CanRead = pi.CanRead;

        var attributes = pi.GetCustomAttributes();


        property.AllAttributes = attributes.Select(attr => new Attribute(RequireType(attr.GetType()), attr)).ToArray();
        foreach (var attr in property.AllAttributes)
        {
          property.Attributes[attr.Name] = attr;
        }
        result.PropertyMap[property.PropertyName] = property;
        return property;
      }).ToArray();

      return result;
    }
    public ObjectType RequireType(Guid id)
    {
      if (HasType(id)) return GetType(id);
      var type = Reflection.GetById(id);
      return RequireType(type);
    }
    [Import]
    IReflectionService Reflection
    {
      get;
      set;
    }

    public IEnumerable<ObjectType> Types { get { return TypeMap.Values; } }



    public ObjectType GetById(Guid id)
    {
      return GetType(id);
    }

    public IQueryable<ObjectType> Read()
    {
      return TypeMap.Values.AsQueryable();
    }

    IQueryable IDataSource.Read()
    {
      return Read() as IQueryable<object>;
    }

    public IQueryable<Guid> ReadIds(System.Linq.Expressions.Expression<Func<ObjectType, bool>> predicate)
    {
      return Read().Where(predicate).Select(t => t.Id);
    }


  }
}
