using Core.Graph.Directed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Values
{
  public abstract class AbstractConnectableObject : AbstractConnectable, IConnectableObject
  {
    protected override void EdgesChanged()
    {
      RaisePropertyChanged("Connectors");
    }
    public IEnumerable<IConnector> Connectors
    {
      get
      {
        return Edges.Select(c => c.Head).OfType<IConnector>();
      }
    }
  }
}
