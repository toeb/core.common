using System;
using System.ComponentModel;

namespace Core.Values
{
  public class SinkInfo : ConnectorInfo, ISinkInfo
  {

    public SinkInfo(bool isWriteable, Type valueType, bool onlyExact)
      : base(valueType, onlyExact)
    { 
      IsWriteable = isWriteable;
    }
    private bool isWriteable;
    public bool IsWriteable
    {
      get { return isWriteable; }
      internal set
      {
        ChangeIfDifferentAndCallback(ref isWriteable, value, IsWritableChanging, IsWritableChanged);

      }
    }

    private void IsWritableChanged(bool oldValue, bool newValue) { }

    private void IsWritableChanging(bool oldValue, bool newValue) { }
  }
}
