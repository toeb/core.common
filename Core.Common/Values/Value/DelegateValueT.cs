using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Values
{

  public class TypedDelegateValue<T> : DelegateValue, IValue<T>
  {
    public TypedDelegateValue(GetterDelegate<T> getter, SetterDelegate<T> setter)
      : base(null, null)
    {
      Getter = getter;
      Setter = setter;
      if (getter != null && Info.IsWriteable) Value = Getter();

    }
    private GetterDelegate<T> getter;
    private SetterDelegate<T> setter;
    public new GetterDelegate<T> Getter
    {
      get
      {
        return getter;
      }
      set
      {
        getter = value;
        if (getter == null) base.Getter = null;
        else base.Getter = DoGet;
      }


    }

    public new SetterDelegate<T> Setter
    {
      get
      {
        return setter;
      }
      set
      {
        setter = value;
        if (setter == null) base.Setter = null;
        else base.Setter = DoSet;
      }
    }



    private void DoSet(object value)
    {
      if (!(value is T)) throw new ArgumentException("cannot call delegate setter because value is not of type T");
      Setter((T)value);
    }
    private object DoGet()
    {
      return Getter();
    }

    public new T Value
    {
      get
      {
        return (T)base.Value;
      }
      set
      {
        base.Value = value;
      }
    }
  }
}
