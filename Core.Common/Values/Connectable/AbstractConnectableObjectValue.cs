using Core.Values;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Core.Extensions;

namespace Core.Values
{
  /// <summary>
  /// Base class for values which are also connectable objects ( can have associated sinks, source, values \in IConnector)
  /// </summary>
  public abstract class AbstractConnectableObjectValue : AbstractValue, IConnectableObjectValue
  {
    /// <summary>
    /// forwards valueInfo to base class
    /// </summary>
    /// <param name="valueInfo"></param>
    public AbstractConnectableObjectValue(ValueInfo valueInfo)
      : base(valueInfo)
    {

    }
    /// <summary>
    /// when edges change then connectors change as well
    /// </summary>
    protected override void EdgesChanged()
    {
      base.EdgesChanged();
      RaisePropertyChanged("Connectors");
    }

    /// <summary>
    /// returns all connected connectors (sinks , sources, values)
    /// </summary>
    public IEnumerable<IConnector> Connectors
    {
      get
      {
        return Edges.Select(c => c.Head).OfType<IConnector>();
      }
    }
  }
}
