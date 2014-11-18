using System;
using System.Collections.Generic;

namespace Core
{
  public class ScopeBase : AbstractScope<ScopeBase>
  {
    private IDictionary<string, object> data = new Dictionary<string, object>();
    protected sealed override bool HasValue(string key)
    {
      return data.ContainsKey(key);
    }

    protected sealed override object LoadValue(string key)
    {
      return data[key];
    }

    protected sealed override void StoreValue(string key, object value)
    {
      data[key] = value;
    }
  }



  public class ScopeBase<TScope> : ScopeBase, IScope<TScope> where TScope : ScopeBase
  {
    

    public new TScope Parent
    {
      get { return base.Parent as TScope; }
    }
  }
}
