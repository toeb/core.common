using System;
using System.ComponentModel;
using Core.Common.Reflect;
namespace Core.Values
{
  public class ValueInfo : NotifyPropertyChangedBase, IValueInfo, IModifiableConnectorInfo
  {
    public static ValueInfo MakeDefault()
    {
      return new ValueInfo(false, false, null, false);
    }

    public static ValueInfo MakeReadOnly()
    {
      return new ValueInfo(true, false, null, false);
    }
    public static ValueInfo MakeReadWrite()
    {
      return new ValueInfo(true, true, null, false);
    }

    public ValueInfo(bool isReadable = true, bool isWritable = true, Type valueType = null, bool exactType = false)
    {
      IsReadable = isReadable;
      IsWriteable = isWritable;
      this.ValueType = valueType ?? typeof(object);
      OnlyExactType = exactType;
    }
    private Type valueType;
    private bool isReadable;
    private bool onlyExactType;
    private bool isWritable;
    public virtual Type ValueType
    {
      get { return valueType; }
       set { if (valueType == value)return; valueType = value; OnValueTypeChanged(); RaisePropertyChanged("ValueType"); }
    }

    protected virtual void OnValueTypeChanged() { }
    protected virtual void OnOnlyExactTypeChanged() { }
    protected virtual void OnReadableChanged() { }
    protected virtual void OnWritableChanged() { }

    public bool OnlyExactType
    {
      get { return onlyExactType; }
      set { if (this.onlyExactType == value)return; onlyExactType = value; OnOnlyExactTypeChanged(); RaisePropertyChanged("OnlyExactType"); }
    }

    public bool IsReadable
    {
      get { return isReadable; }
      set
      {
        if (isReadable == value) return; isReadable = value; RaisePropertyChanged("IsReadable");
        OnReadableChanged();
      }
    }


    public virtual bool IsValidValue(object value)
    {
      if (OnlyExactType) return ValueType.IsExactlyAssignableFromValue(value);
      else return ValueType.IsAssignableFromValue(value);
    }
    public virtual bool IsValidValueType(Type type)
    {
      if (OnlyExactType) return ValueType == type;
      else return ValueType.IsAssignableFrom(type);
    }


    public bool IsWriteable
    {
      get { return isWritable; }
      set
      {
        if (value == isWritable) return;
        isWritable = value;
        OnWritableChanged();
        RaisePropertyChanged("IsWriteable");

      }
    }
  }
}
