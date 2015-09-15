using System;

namespace Core.Common
{


  public class NotifyPropertyChangedSetter : DelegatingPropertySetter
  {
    private Action<string> RaisePropertyChanged;
    private PropertyGetterDelegate getter;
    public NotifyPropertyChangedSetter(Action<string> onPropertyChange, PropertyGetterDelegate getter, PropertySetterDelegate inner)
      : base(inner)
    {
      this.getter = getter;
      this.RaisePropertyChanged = onPropertyChange;
      
    }
    protected override bool Set(object @object, string key, Type type, object value)
    {
      object existingValue;
      var hasValue = getter(@object, key, type, out existingValue);
  
      var success = Inner(@object, key, type, value);
      if (!success) return false;
  
      if (!hasValue)
      {
        RaisePropertyChanged(key);
        return true;
      }
  
  
  
      if (value != existingValue)
      {
        RaisePropertyChanged(key);
      }
  
  
      return true;
    }
  }
}
