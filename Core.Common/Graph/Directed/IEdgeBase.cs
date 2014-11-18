using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Graph.Directed
{


  public interface IEdgeBase<out TNode,out TEdge>
  {
    TNode Head { get; }
    TNode Tail { get; }
  }
}
