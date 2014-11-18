using Core.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ManagedObjects
{


  public static class ManagedProperty
  {
    public static IManagedProperty Memory(string name, Type type)
    {
      return new MemoryManagedProperty(name, true, true, type, null);
    }
    public static IManagedProperty Memory(IPropertyInfo info, object initialValue)
    {
      return new MemoryManagedProperty(new ManagedPropertyInfo(info), initialValue);
    }

    public static IManagedProperty<T> Memory<T>(string name, T initialValue = default(T))
    {
      return new MemoryManagedProperty<T>(name, initialValue);
    }
    public static IManagedProperty<T> Delegate<T>(string name, ProduceValueDelegate<T> getter, ConsumeValueDelegate<T> setter)
    {
      return new DelegateManagedProperty<T>(name, getter, setter);
    }
  }
}
