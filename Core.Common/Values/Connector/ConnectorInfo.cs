using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Core.Values
{
  public class ConnectorInfo : NotifyPropertyChangedBase, IConnectorInfo, IModifiableConnectorInfo
  {
    protected ConnectorInfo(Type valueType, bool onlyExactType)
    {
      if (valueType == null) valueType = typeof(object);
      ValueType = valueType;
      OnlyExactType = onlyExactType;
    }
    private Type valueType;
    private bool onlyExactType;

    public Type ValueType
    {
      get { return valueType; }
      set { ChangeIfDifferentAndCallback(ref valueType, value, ValueTypeChanging, ValueTypeChanged); }
    }

    protected virtual void ValueTypeChanging(Type oldValue, Type newValue) { }
    protected virtual void ValueTypeChanged(Type oldValue, Type newValue) { }

    public bool OnlyExactType
    {
      get { return onlyExactType; }
      set { ChangeIfDifferentAndCallback(ref onlyExactType, value, OnlyExactTypeChanging, OnlyExactTypeChanged); }

    }
    protected virtual void OnlyExactTypeChanging(bool oldValue, bool newValue) { }
    protected virtual void OnlyExactTypeChanged(bool oldValue, bool newValue) { }

    bool IConnectorInfo.IsValidValue(object value)
    {
      if (OnlyExactType) return ValueType.IsExactlyAssignableFromValue(value);
      else return ValueType.IsAssignableFromValue(value);
    }


    public bool IsValidValueType(Type type)
    {
      if (OnlyExactType) return ValueType == type;
      else return valueType.IsAssignableFrom(type);
    }


  }
}
