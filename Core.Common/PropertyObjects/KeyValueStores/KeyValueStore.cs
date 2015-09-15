using System.Collections.Generic;

namespace Core
{
  public class KeyValueStore : IKeyValueStore
  {
    private IDictionary<string, object> values = new Dictionary<string, object>();
    public bool TryGet(string key, out object value)
    {
      return values.TryGetValue(key, out value);
    }
  
    public bool TrySet(string key, object value)
    {
      values[key] = value;
      return true;
    }
  
    public bool HasKey(string key)
    {
      return values.ContainsKey(key);
    }
  
    public IEnumerable<string> GetKeys()
    {
      return values.Keys;
    }
  }
}
