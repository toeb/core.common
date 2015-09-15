using System;

namespace Core.Common
{
  public abstract class DelegatingPropertySetter
  {
    protected DelegatingPropertySetter(PropertySetterDelegate inner)
    {
      Inner = inner;
    }
    public static implicit operator PropertySetterDelegate(DelegatingPropertySetter self)
    {
      return self.Set;
    }
  
    protected abstract bool Set(object @object, string key, Type type, object value);
  
  
    public PropertySetterDelegate Inner { get; set; }
  
  }
}
