using System;
using System.Collections.Generic;

namespace Core.Common
{
  public interface ITypedValueMap : IValueMap
  {
    object Get(string key, Type expectedType);
    bool TryGet(string key, Type type, out object value);
    void Set(string key, object value, Type expectedType);
    bool Has(string key, Type expectedType);
  }
}
