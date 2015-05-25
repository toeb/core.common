using System;
using System.Collections.Generic;

namespace Core
{
  public abstract class AbstractScope : AbstractPropertyStore,  IScope
  {
    private AbstractScope parent;
    internal void SetParent(AbstractScope parent)
    {
      this.parent = parent;
    }
    internal void SetValue(string key, object value)
    {
      StoreValue(key, value);
    }

    public IScope Parent
    {
      get { return parent; }
    }
    protected abstract bool HasValue(string key);
    protected abstract object LoadValue(string key);
    protected abstract void StoreValue(string key, object value);
    protected AbstractScope GetScope(string key)
    {
      if (HasValue(key)) return this;
      if (parent == null) return null;
      return parent.GetScope(key);
    }
    public object this[string key]
    {
      get
      {
        var scope = GetScope(key);
        if (scope == null) return null;
        return scope.LoadValue(key);
      }
      set
      {
        var scope = this;//GetScope(key);
        //if (scope == null) scope = this;
        scope.StoreValue(key, value);
      }
    }

    protected override bool TrySetProperty(string key, object value, Type type)
    {
      
      this[key] = value;
      return true;
    }

    
    protected override bool TryGetProperty(string key, out object value, Type type)
    {      
      value = this[key];
      return true;
    }

    protected override bool HasProperty(string key,Type type)
    {
      return GetScope(key) != null;
    }
  }

  public abstract class AbstractScope<TScope> : AbstractScope, IScope<TScope> where TScope : AbstractScope
  {

    public new TScope Parent
    {
      get { return ((IScope)this).Parent as TScope; }
    }
  }

}
