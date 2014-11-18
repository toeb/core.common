using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
  [Serializable]
  public class SimpleContext : IContext
  {
    [NonSerialized] private Dictionary<string, object> data = new Dictionary<string, object>();
    public object Get(string key, object value)
    {
      if (!ContainsKey(key)) return null;
      return data[key];
    }

    public void Set(string key, object value)
    {
      data[key] = value;
    }

    public bool ContainsKey(string key)
    {
      return data.ContainsKey(key);
    }

    [NonSerialized] private IContextProvider provider;
    public IContextProvider Provider
    {
      get { return provider; }
      set { provider = value; }
    }
    [NonSerialized]
    IContextService service;
    public IContextService Service
    {
      get { return service; }
      set { service = value; }
    }

    [NonSerialized]
    ContextDescriptor descriptor;

    public ContextDescriptor Descriptor
    {
      get { return descriptor; }
      set { descriptor = value; }
    }

    public Guid ContextId
    {
      get;
      set;
    }

    public object Get(string key)
    {
      if (!ContainsKey(key)) return null;
      return data[key];
    }



    public IContext Parent
    {
      get;
      set;
    }

    public void Dispose()
    {
      foreach (var item in data.Values)
      {
        var disposable = item as IDisposable;
        if (disposable != null)
        {
          disposable.Dispose();
        }
      }
      data.Clear();
    }


    public IEnumerable<string> Keys
    {
      get { return data.Keys; }
    }
  }
}
