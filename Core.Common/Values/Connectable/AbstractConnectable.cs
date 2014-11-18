using Core.Graph.Directed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Values
{
  public abstract class AbstractConnectable : NodeBase<AbstractConnectable, AbstractConnection>, IConnectable
  {

    IEnumerable<IConnection> INodeBase<IConnectable, IConnection>.Edges
    {
      get { return Edges; }
    }
  }
}
