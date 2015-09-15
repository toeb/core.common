using System;

namespace Core.Common
{
  public class PropertyObject : IDisposable
  {
    public PropertyObject(IObjectOperations operations)
    {
      setter = operations.Setter;
      getter = operations.Getter;
      assign = operations.Assign;
      call = operations.Call;
      memberCall = operations.MemberCall;
      hasKey = operations.HasKey;
      dispose = operations.Dispose;
    }
    public PropertyObject()
      : this(new DefaultObjectOperations())
    {
  
    }
    protected PropertySetterDelegate setter;
    protected PropertyGetterDelegate getter;
    protected ObjectAssignDelegate assign;
    protected ObjectCallDelegate call;
    protected ObjectMemberCallDelegate memberCall;
    protected ObjectHasKey hasKey;
    protected ObjectDisposeDelegate dispose;
  
    public static implicit operator CallDelegate(PropertyObject value)
    {
      return value.Call;
    }
  
    public object Call(params object[] parameters) { return call(this, parameters); }
  
    protected bool TrySet(string key, Type type, object value)
    {
      return setter(this, key, type, value);
    }
    protected bool TryGet(string key, Type type, out object value)
    {
      return getter(this, key, type, out value);
    }
  
    public object Get(string key, Type type)
    {
  
      object value;
      var success = TryGet(key, type, out value);
      if (!success) throw new InvalidOperationException(string.Format("failed to  get property '{0}'", key));
      return value;
    }
    public void Set(string key, Type type, object value)
    {
  
      var success = TrySet(key, type, value);
      if (!success) throw new InvalidOperationException(string.Format("failed to set property '{0}'", key));
    }
  
    public object MemberCall(string key, params object[] parameters)
    {
      return memberCall(this, key, parameters);
    }
    public CallDelegate this[string key, bool doCall]
    {
      get
      {
        return parameters => this.MemberCall(key, parameters);
      }
    }
  
    public object this[string key]
    {
      get
      {
        return Get(key, typeof(object));
      }
      set
      {
        Set(key, typeof(object), value);
      }
    }
  
    public void Dispose()
    {
      dispose(this);
    }
  }
}
