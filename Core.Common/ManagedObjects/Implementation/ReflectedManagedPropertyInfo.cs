using System;
using System.Reflection;

namespace Core.ManagedObjects
{
  public class ReflectedManagedPropertyInfo : IPropertyInfo
  {
    public static implicit operator ReflectedManagedPropertyInfo(PropertyInfo info)
    {
      return new ReflectedManagedPropertyInfo(info);
    }
    public ReflectedManagedPropertyInfo(PropertyInfo info)
    {
      this.Info = info;
    }
    public string Name
    {
      get { return Info.Name; }
    }

    public bool IsWriteable
    {
      get { return Info.CanWrite; }
    }

    public Type ValueType
    {
      get { return Info.PropertyType; }
    }

    public bool OnlyExactType
    {
      get { return false; }
    }

    public bool IsValidValue(object value)
    {
      return ValueType.IsAssignableFromValue(value);
    }

    public bool IsReadable
    {
      get { return Info.CanRead; }
    }

    public PropertyInfo Info { get; private set; }

    public string Id
    {
      get { return Name + ":" + ValueType.Namespace.Normalize().ToString()+"."+ValueType.Name; }
    }


    public bool IsValidValueType(Type type)
    {
      return ValueType.IsAssignableFrom(type);
    }
  }
}
