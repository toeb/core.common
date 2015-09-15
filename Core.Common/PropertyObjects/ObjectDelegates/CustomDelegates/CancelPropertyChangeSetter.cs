using System;
using System.Threading;

namespace Core.Common
{
  public class CancelPropertyChangeSetter : DelegatingPropertySetter
  {
    private Func<string, object, object, bool> ShouldCancel;
    private PropertyGetterDelegate getter;
    public CancelPropertyChangeSetter(Func<string, object, object, bool> onPropertyChanging, PropertyGetterDelegate getter, PropertySetterDelegate inner)
      : base(inner)
    {
      this.getter = getter;
      this.ShouldCancel = onPropertyChanging;
    }
    protected override bool Set(object @object, string key, Type type, object value)
    {
      object oldValue;
      var success = getter(@object, key, type, out oldValue);
      if (!success) oldValue = null;
  
  
      var shouldCancel = ShouldCancel(key, oldValue, value);
  
      if (shouldCancel) return true;
  
  
      return Inner(@object, key, type, value);
  
  
    }
  }
}
