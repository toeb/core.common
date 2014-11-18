using System;
using System.Threading.Tasks;

namespace Core.Values
{
  public interface IConnector : IConnectable
  {
    IConnectorInfo ConnectorInfo { get; }
  }
}
