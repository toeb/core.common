using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Core.ManagedObjects
{
  public class ReflectedManagedNotifyingObject : ReflectedManagedObject
  {
    public ReflectedManagedNotifyingObject(object instance, IEnumerable<PropertyInfo> properties)
      : base(instance, properties)
    {

      if (!(instance is INotifyPropertyChanged)) throw new ArgumentException("instance is not of type INotifyPropertyChanged");
      Notifier = instance as INotifyPropertyChanged;
    }

    protected override IManagedProperty ConstructProperty(IPropertyInfo info, Lazy<object> initialValue)
    {
      if (info is ReflectedManagedPropertyInfo)
      {
        return new ReflectedNotifyingManagedProperty(Instance, info as ReflectedManagedPropertyInfo);
      }
      return base.ConstructProperty(info, initialValue);
    }
    private INotifyPropertyChanged Notifier { get; set; }

  }
}
