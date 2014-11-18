using System;
using System.Collections.Generic;

namespace Core
{
  class ContextDictionary : IContextDictionary
  {
    private IDictionary<string, object> dictionary;
    private IDictionary<string, object> Dictionary { get { return dictionary ?? (dictionary = new Dictionary<string, object>()); } }

    public void Add(string key, object value)
    {
      Dictionary.Add(key,value);
    }

    public bool ContainsKey(string key)
    {
      return Dictionary.ContainsKey(key);
    }

    public bool Remove(string key)
    {
      return Dictionary.Remove(key);
    }

    public bool TryGetValue(string key, out object value)
    {
      return Dictionary.TryGetValue(key, out value);
    }


    public object this[string key]
    {
      get
      {
        return Dictionary[key];
      }
      set
      {
        Dictionary[key] = value;
      }
    }

    public void Add(KeyValuePair<string, object> item)
    {
      Dictionary.Add(item);
    }

    public void Clear()
    {
      Dictionary.Clear();
    }

    public bool Contains(KeyValuePair<string, object> item)
    {
      return Dictionary.Contains(item);
    }

    public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
    {
      CopyTo(array, arrayIndex);
    }

    public int Count
    {
      get { return Dictionary.Count; }
    }

    public bool IsReadOnly
    {
      get { return Dictionary.IsReadOnly; }
    }

    public bool Remove(KeyValuePair<string, object> item)
    {
      return Dictionary.Remove(item);
    }

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
    {
      return Dictionary.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return Dictionary.GetEnumerator();
    }

    public object Get(string key)
    {
      if (!ContainsKey(key)) return null;
      return Dictionary[key];
    }

    public void Set(string key, object value)
    {
      Dictionary[key] = value;
    }

    public IEnumerable<string> Keys
    {
      get { return Dictionary.Keys; }
    }

    public IEnumerable<object> Values
    {
      get { return Dictionary.Values; }
    }


    ICollection<string> IDictionary<string, object>.Keys
    {
      get { return Dictionary.Keys; }
    }

    ICollection<object> IDictionary<string, object>.Values
    {
      get { return Dictionary.Values; }
    }
  }
}
