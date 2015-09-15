using System;
using System.Collections.Generic;

namespace Core.Common
{
  public interface IObjectOperations
  {
    bool Getter(object @object, string key, Type type, out object value);
    bool Setter(object @object, string key, Type type, object value);
    bool Assign(object @object, object value);
    object Call(object @object, object[] parameters);
    object MemberCall(object @object, string key, object[] parameters);
    IEnumerable<string> GetKeys(object @object);
    bool HasKey(object @object, string key);
    bool Remove(object @object, string key);
    void Dispose(object @object);
  }
}