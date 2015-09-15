using System;
using System.Collections.Generic;

namespace Core.Common
{
  /// <summary>
  /// uses a simple key value store to map object operations
  /// </summary>
  public class DefaultObjectOperations : IObjectOperations
  {
    private IDictionary<string, object> values;
    public DefaultObjectOperations()
    {
      values = new Dictionary<string, object>();
    }
    public bool Getter(object @object, string key, Type type, out object value)
    {
      return values.TryGetValue(key, out value);
    }
  
    public bool Setter(object @object, string key, Type type, object value)
    {
      values[key] = value;
      return true;
    }
  
    public bool Assign(object @object, object value)
    {
      throw new NotImplementedException();
    }
  
    public object Call(object @object, object[] parameters)
    {
      throw new NotImplementedException();
    }
  
    public object MemberCall(object @object, string key, object[] parameters)
    {
      object value;
      var success = Getter(@object, key, typeof(object), out value);
      var callable = value as Delegate;
      if (callable == null) throw new InvalidOperationException("cannot call member '{0}' it is not a delegate type");
      var returnValue = callable.DynamicInvoke(parameters);
  
      return returnValue;
    }
  
    public IEnumerable<string> GetKeys(object @object)
    {
      return values.Keys;
    }
  
    public bool HasKey(object @object, string key)
    {
      return values.ContainsKey(key);
    }
  
  
    public bool Remove(object @object, string key)
    {
      return values.Remove(key);
    }


    public void Dispose(object @object)
    {
      if (@object is IDisposable)
      {
        (@object as IDisposable).Dispose();
      }
    }
  }
}