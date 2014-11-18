using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Core
{
  public abstract class AbstractPropertyStore : IPropertyStore
  {
    protected abstract void SetProperty(string key, object value, Type type);
    protected abstract object GetProperty(string key,Type type);
    protected abstract bool HasProperty(string key,Type type);



    public object this[string key]
    {
      get
      {
        if (!Has(key)) return null;
        return Get(key);
      }
      set
      {
        Set(key, value);
      }
    }

    protected T Get<T>([CallerMemberName] string name = null)
    {
      if (!Has(name,typeof(T))) return default(T);
      return (T)Get(name,typeof(T));
    }



    protected void Set<T>(T value, [CallerMemberName] string name = null)
    {
      Set(name, value, typeof(T));
    }

    // implementation of IValueMap
    public object Get(string key)
    {
      return Get(key, typeof(object));
    }

    public void Set(string key, object value)
    {
      Set(key, value, typeof(object));
    }

    public bool Has(string key)
    {
      return Has(key, typeof(object));
    }
    // implementatio nof ITypedValueMap
    public object Get(string key, Type type)
    {
      return GetProperty(key,type);
    }

    public void Set(string key, object value, Type type)
    {
      SetProperty(key, value, type);
    }

    public bool Has(string key, Type type)
    {
      return HasProperty(key, type);
    }
  }
}
