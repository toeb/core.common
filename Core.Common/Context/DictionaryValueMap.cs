using System;
using System.Collections.Generic;

namespace Core
{
  public class DictionaryValueMap : IValueMap
  {
    private Dictionary<string, object> dictionary = new Dictionary<string, object>();
    public object Get(string key)
    {
      return dictionary[key];
    }

    public void Set(string key, object value)
    {
      dictionary[key] = value;
    }

    public bool Has(string key)
    {
      return dictionary.ContainsKey(key);
    }
  }
}
