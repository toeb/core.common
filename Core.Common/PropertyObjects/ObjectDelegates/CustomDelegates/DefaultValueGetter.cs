using System;

namespace Core.Common
{
  public class DefaultValueGetter : DelegatingGetter
  {
    public DefaultValueGetter(PropertyGetterDelegate inner) : base(inner) { }
  
    protected override bool Get(object @object, string key, Type type, out object value)
    {
      var success = Inner(@object, key, type, out value);
      if (!success)
      {
        value = type.IsValueType ? Activator.CreateInstance(type) : null;
      }
      return true;
    }
  }
}
