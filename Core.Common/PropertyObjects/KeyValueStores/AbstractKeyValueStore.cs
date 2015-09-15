using System.Collections.Generic;

namespace Core
{
  public abstract class AbstractKeyValueStore : IKeyValueStore
  {
    private IKeyValueStore store;
    protected AbstractKeyValueStore(IKeyValueStore store)
    {
      this.store = store;
    }
  
    public bool TryGet(string key, out object value)
    {
      var success = store.TryGet(key, out value);
      return success;
    }
  
    public bool TrySet(string key, object value)
    {
      var success = store.TrySet(key, value);
  
      return success;
    }
  
    public bool HasKey(string key)
    {
      var result = store.HasKey(key);
      return result;
    }
  
    public IEnumerable<string> GetKeys()
    {
      var keys = store.GetKeys();
      return keys;
    }
  }
}
