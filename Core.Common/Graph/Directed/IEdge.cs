using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Graph.Directed
{

  public interface IEdge<out TNode,out TEdge> : IEdgeBase<TNode, TEdge>
    where TEdge : IEdgeBase<TNode, TEdge>
    where TNode : INodeBase<TNode, TEdge>
  {
  }
}
