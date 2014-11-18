using System;
using System.Collections.Generic;
using System.Reflection;

namespace Core.ManagedObjects
{
  public class ReflectedManagedObject : ManagedObjectBase
  {

    public ReflectedManagedObject(object instance, IEnumerable<PropertyInfo> properties)
      :base(ManagedObjectInfo.MakeDefault())
    {
      this.Instance = instance;
      RegisterProperties(properties);
    }

    private void RegisterProperties(IEnumerable<PropertyInfo> properties)
    {
      foreach (var property in  properties)
      {
        RequireProperty(new ReflectedManagedPropertyInfo(property), new Lazy<object>(() => null));
      }
    }

    protected override IManagedProperty ConstructProperty(IPropertyInfo info, Lazy<object> initialValue)
    {
      if (info is ReflectedManagedPropertyInfo)
      {
        return new ReflectedManagedProperty(Instance, info as ReflectedManagedPropertyInfo);
      }
      return base.ConstructProperty(info, initialValue);
    }
    public object Instance { get; private set; }
  }
}
