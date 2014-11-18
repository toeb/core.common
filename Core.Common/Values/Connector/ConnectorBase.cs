using System;
using System.ComponentModel;

namespace Core.Values
{



  public abstract class ConnectorBase : AbstractConnectable, IConnector
  {
    protected ConnectorBase(IConnectorInfo info)
    {
      this.ConnectorInfo = info;
    }

    public virtual bool IsValidValue(object value)
    {
      return ConnectorInfo.IsValidValue(value);
    }


    public IConnectorInfo ConnectorInfo
    {
      get;
      protected set;
    }
  }
}
