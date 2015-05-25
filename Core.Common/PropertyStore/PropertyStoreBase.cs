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

    protected override bool TrySetProperty(string key, object value, Type Type)
    {
      return Implementation.TrySet(key, value);
    }





    protected override bool HasProperty(string key, Type type)
    {
      return Implementation.Has(key);
    }

    protected override bool TryGetProperty(string key, out object value, Type type)
    {
      return Implementation.TryGet(key, out value);
    }
  }
}
