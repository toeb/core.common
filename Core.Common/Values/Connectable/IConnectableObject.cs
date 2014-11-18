using Core.Graph.Directed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Values
{
  /// <summary>
  /// a connectable object can have sinks, sources and values associate with it
  /// </summary>
  public interface IConnectableObject : IConnectable
  {
    IEnumerable<IConnector> Connectors { get; }
  }

  /// <summary>
  /// an interface which simultaneously represents a connectable object and a value
  /// </summary>
  public interface IConnectableObjectValue : IConnectableObject, IValue
  {

  }


}
