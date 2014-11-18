using System;
using System.ComponentModel;

namespace Core.Values
{
  public class SourceInfo : ConnectorInfo, ISourceInfo
  {
    public SourceInfo(bool isReadable, Type valueType, bool onlyExact) : base(valueType, onlyExact) { IsReadable = isReadable; }
    private bool isReadable;
    public bool IsReadable
    {
      get { return isReadable; }
      internal set { ChangeIfDifferentAndCallback(ref isReadable, value, IsReadableChanging, IsReadableChanged); }
    }

    protected virtual void IsReadableChanged(bool oldValue, bool newValue) { }
    protected virtual void IsReadableChanging(bool oldValue, bool newValue) { }
  }
}
