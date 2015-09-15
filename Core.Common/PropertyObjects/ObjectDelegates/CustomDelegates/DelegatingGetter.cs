using System;

namespace Core.Common
{
  public abstract class DelegatingGetter
  {
    public static implicit operator PropertyGetterDelegate(DelegatingGetter self)
    {
  
      return self.Get;
    }
    protected DelegatingGetter(PropertyGetterDelegate inner)
    {
      this.Inner = inner;
    }
    protected abstract bool Get(object @object, string key, Type type, out object value);
  
  
    protected PropertyGetterDelegate Inner { get; private set; }
  }
}
