using System;
using System.Collections.Generic;

namespace Core
{
  public interface IContextDictionary : IDictionary<string, object>
  {
    object Get(string key);
    void Set(string key, object value);
    new IEnumerable<string> Keys { get; }
    new IEnumerable<object> Values { get; }
  }
}
