using Core.Graph.Directed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Values
{
  public abstract class AbstractConnection : EdgeBase<AbstractConnectable, AbstractConnection>, IConnection
  {
    IConnectable IEdgeBase<IConnectable, IConnection>.Head
    {
      get { return Head; }
    }

    IConnectable IEdgeBase<IConnectable, IConnection>.Tail
    {
      get { return Tail; }
    }
  }
}
