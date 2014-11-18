using Core.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Core.TypeServices
{
  [DebuggerDisplay("{FullName}")]
  public class ObjectType : AttributeTarget
  {
    public ObjectType()
    {


      PropertyMap = new Dictionary<string, ObjectProperty>();
    }
    public override bool Equals(object obj)
    {
      var other = obj as ObjectType;
      if (other == null) return false;
      return other.Id == this.Id;            
    }
    public override int GetHashCode()
    {
      return Id.GetHashCode();
    }
    public static implicit operator ObjectType(Type type){
      return TypeService.Instance.RequireType(type);
    }
    public static implicit operator Type(ObjectType type)
    {
      if (!type.IsSystemType) return null;
      return type.systemType;
    }
    public override string ToString()
    {
      return FullName;
    }

    public IDictionary<string, object> Enum { get; set; }
    public string FullName { get; set; }
    public string AssemblyQualifiedName { get; set; }
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string FullClassName { get; set; }
    /// <summary>
    /// difference to Name:  no generic type in name (can be same for different classes)
    /// </summary>
    public string ClassName { get; set; }
    public ObjectType BaseType { get; set; }
    public IEnumerable<ObjectType> Interfaces { get; set; }
    public IEnumerable<ObjectProperty> Properties { get; set; }
    public bool IsGeneric { get; set; }
    public ObjectType GenericType { get; set; }
    public IEnumerable<ObjectType> GenericTypeArguments { get; set; }
    private Type systemType;
    public ObjectType[] GenericParameters { get; set; }
    public string Namespace { get; set; }
    public bool IsSystemType { get; set; }
    public string TypeProvider { get; set; }
    public IDictionary<string, ObjectProperty> PropertyMap { get; private set; }

    internal void SetSystemType(Type type)
    {
      TypeProvider = "clr";
      systemType = type;
    }

  }
}
