using System;
using System.Collections.Generic;

namespace Core
{

  
  public interface IValueMap
  {
    bool TryGet(string key, out object value);
    bool TrySet(string key, object value);

    object Get(string key);
    void Set(string key, object value);
    bool Has(string key);
  }
  public interface ITypedValueMap : IValueMap
  {
    object Get(string key, Type expectedType);
    bool TryGet(string key, Type type, out object value);
    void Set(string key, object value, Type expectedType);
    bool Has(string key, Type expectedType);
  }
}
