using System;
using System.Collections.Generic;

namespace Core.Common
{


  public interface IValueMap
  {
    bool TryGet(string key, out object value);
    bool TrySet(string key, object value);

    object Get(string key);
    void Set(string key, object value);
    bool Has(string key);
  }
}
