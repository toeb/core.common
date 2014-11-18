using System;
using System.ComponentModel;

namespace Core.Values
{
  public delegate void SetterDelegate(object value);
  public delegate object GetterDelegate();
  public class DelegateValue : AbstractValue
  {
    private SetterDelegate setter;
    private GetterDelegate getter;
    public SetterDelegate Setter
    {
      get { return setter; }
      set
      {
        if (value == null) Info.IsWriteable = false;
        else if (setter == null) Info.IsWriteable = true;
        if (setter == value) return;
        setter = value;
        if (Getter != null && setter != null) setter(Getter());
        RaisePropertyChanged("Setter");
      }
    }
    public GetterDelegate Getter
    {
      get { return getter; }
      set
      {
        if (value == null) Info.IsReadable = false;
        else if (getter == null) Info.IsReadable = true;
        if (getter == value) return;
        if (getter != null && Info.IsWriteable) this.Value = getter();
        getter = value;
        RaisePropertyChanged("Getter");
      }
    }

    public DelegateValue(GetterDelegate getter, SetterDelegate setter) : base(getter != null, setter != null, typeof(object), false) { Setter = setter; Getter = getter; }
    protected override void ConsumeValue(object value)
    {
      if (Setter != null) Setter(value);
      NotifyValueChanged();
    }
    protected override object ProduceValue()
    {
      if (Getter != null) return Getter();
      throw new InvalidOperationException("Cannot retrieve value because no Getter is  set");
    }
  }
}
