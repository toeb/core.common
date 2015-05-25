using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Core
{
  public abstract class AbstractPropertyStore : IPropertyStore
  {
    protected abstract bool TrySetProperty(string key, object value, Type type);
    protected abstract bool TryGetProperty(string key,out object value, Type type);
    protected abstract bool HasProperty(string key,Type type);
    protected virtual object FallbackGet(string key, Type type) { throw new InvalidOperationException("The property could not be gotten"); }
    protected virtual void SetFallback(string key, Type type, object value) { throw new InvalidCastException("The property could not be set"); }

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

    protected bool TryGet<T>(out T value, [CallerMemberName] string name = null)
    {
      object val;
      var result = TryGet(name, out val);
      if (!result)
      {
        value = default(T);
        return false;
      }
      value = (T)val;
      return true;
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
      object result;
      var success = TryGet(key, out result);
      if (!success) return FallbackGet(key, type);
      return result;

    }

    public void Set(string key, object value, Type type)
    {
      if (!TrySet(key, type, value)) SetFallback(key, type,value);
      
    }

    public bool Has(string key, Type type)
    {
      return HasProperty(key, type);
    }


    public bool TryGet(string key, Type type, out object value)
    {
      return TryGetProperty(key, out value, type);
    }

    public bool TryGet(string key, out object value)
    {
      return TryGet(key, typeof(object), out value);
    }
    public bool TrySet(string key, Type type, object value)
    {
      return TrySetProperty(key, value, type);
    }
    public bool TrySet(string key, object value)
    {
      return TrySet(key, typeof(object),value);
    }
  }
}
