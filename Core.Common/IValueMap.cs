using System;
using System.Collections.Generic;

namespace Core
{
  public interface IValueMap
  {
    object Get(string key);
    void Set(string key, object value);
    bool Has(string key);
  }
  public interface ITypedValueMap : IValueMap
  {
    object Get(string key, Type expectedType);
    
    void Set(string key, object value, Type expectedType);
    bool Has(string key, Type expectedType);
  }
}
