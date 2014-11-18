using System;
using System.Collections.Generic;

namespace Core
{
  public class PropertyStoreBase : AbstractPropertyStore
  {
    protected PropertyStoreBase(IValueMap implementation = null)
    {
      Implementation = implementation;
    }

    private IValueMap implementation;
    protected IValueMap Implementation
    {
      get
      {
        return implementation;
      }
      set
      {
        if (value == this) throw new ArgumentException("may not use same store as store's implementation because it would cause infinite recursion");
        if (value == null) value = new DictionaryValueMap();
        this.implementation = value;
      }
    }

    protected override void SetProperty(string key, object value, Type Type)
    {
      Implementation.Set(key, value);
    }

    protected override object GetProperty(string key, Type type)
    {
      return Implementation.Get(key);
    }

    protected override bool HasProperty(string key, Type type)
    {
      return Implementation.Has(key);
    }
  }
}
